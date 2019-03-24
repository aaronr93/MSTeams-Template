using Microsoft.Bot.Connector;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using MSTeams.Template.Util;

namespace MSTeams.Template.Controllers
{
    [BotAuthentication(MicrosoftAppIdSettingName = "MicrosoftAppId", MicrosoftAppPasswordSettingName = "MicrosoftAppPassword")]
    public class ValuesController : ApiController
    {
        private static ILogger Logger => LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Implement this method to run diagnostics on the live version of your bot.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("healthcheck")]
        public async Task<string> HealthCheck()
        {
            return "OK";
        }

        public async Task<HttpResponseMessage> Post([FromBody]Activity activity, CancellationToken cancellationToken)
        {
            var replyText = "Hello world!";
            var reply = activity.CreateReply(replyText, activity.Locale);
            // Send the response. We need a new ConnectorClient each time so that this action is thread-safe.
            // For example, multiple teams may call the bot simultaneously; it should respond to the right conversation.
            var connectorClient = new ConnectorClient(new Uri(activity.ServiceUrl));
            await connectorClient.Conversations.ReplyToActivityAsync(reply, cancellationToken);

            Logger.Info($"<message>{activity.Text}</message><reply>{reply.Text}</reply>");

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
