using GroupMeUtilities.Entities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.MessageService
{
    public class MessageIncoming : IMessageIncoming
    {
        public MessageItem Message { get; set; }
        private readonly HttpRequest _req;
        private static readonly Regex BotAnalysisRegex = new Regex("((?i)(\bbot\b.*\banalysis\b) | (\banalysis\b.*\bbot\b)(?-i))");
        private static readonly Regex BotMessageRegex = new Regex("((?i)(\bbot\b.*\bmessage\b) | (\bmessage\b.*\bbot\b)(?-i))");

        public MessageIncoming(HttpRequest req)
        {
            _req = req;
            Message = new MessageItem();
        }

        public async Task<HttpResponseMessage> ParseIncomingRequestAsync()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            if (_req == null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("No request was received");
                return response;
            }

            string content = string.Empty;
            using (StreamReader sr = new StreamReader(_req.Body))
            {
                content = await sr.ReadToEndAsync();
            }

            if (content != null)
            {
                Message = JsonConvert.DeserializeObject<MessageItem>(content)!;
            }

            if (Message.UserId == "a4165ae5f7ad5ab682e2c3dd52")
            {
                response.StatusCode = HttpStatusCode.OK;
                response.Content = new StringContent("No response required, message is from bot");
                return response;
            }

            if (Message.Text == null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("No text was received");
                return response;
            }

            Match analysisBot = BotAnalysisRegex.Match(Message.Text);
            if (analysisBot.Success)
            {
                //Run analysis code
                throw new NotImplementedException();
            }

            Match messageBot = BotMessageRegex.Match(Message.Text);
            if (messageBot.Success)
            {
                //Run message code
                throw new NotImplementedException();
            }

            response.StatusCode = HttpStatusCode.OK;
            response.Content = new StringContent("No response criteria met, no response required");
            return response;
        }


        public async Task<string> ParseIncomingHeadersAsync()
        {
            if (_req == null)
            {
                return "";
            }

            var builder = new StringBuilder(Environment.NewLine);
            foreach (var header in _req.Headers)
            {
                builder.AppendLine($"{header.Key}: {header.Value}");
            }
            var headersDump = builder.ToString();

            return headersDump;
        }
    }
}
