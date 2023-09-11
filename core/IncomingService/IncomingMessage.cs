using GroupMeUtilities.Model;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using System.Text.RegularExpressions;

namespace Core.IncomingService
{
    public class IncomingMessage
    {
        public MessageItem? Message { get; set; }
        private readonly HttpRequest _req;
        private static readonly Regex BotRequestRegex = new Regex("(.*\\D|^)(\\d+) (dino|dinolike|dino-like).*");


        public IncomingMessage(HttpRequest req) 
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

            string content = String.Empty;
            using (StreamReader sr = new StreamReader(_req.Body))
            {
                content = await sr.ReadToEndAsync();
            }

            Message = JsonConvert.DeserializeObject<MessageItem>(content);

            if(Message!.UserId == "a4165ae5f7ad5ab682e2c3dd52")
            {
                response.StatusCode = HttpStatusCode.OK;
                response.Content = new StringContent("No response required, message is from bot");
                return response;
            }

            if (Message!.Text == null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("No text was received");
                return response;
            }

            Match callBot = BotRequestRegex.Match(Message!.Text);
            if(callBot.Success)
            {

            }

            return Message?.Text != null;
        }
    }
}
