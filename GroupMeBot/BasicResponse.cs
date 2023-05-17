using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using GroupMeUtilities;
using GroupMeUtilities.Model;
using Microsoft.AspNetCore.Http.Features;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Hosting.Server;
using System.Collections.Specialized;
using System.Text;

namespace GroupMeBot
{
    public class BasicResponse
    {
        private const string BotPostUrl = "https://api.groupme.com/v3/bots/post";

        /// <summary>
        /// Gets or sets the message to parse
        /// </summary>
        public MessageItem Message { get; set; }

        [FunctionName("BasicResponse")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "GroupMeBot/BasicResponse")] HttpRequest req,
            ILogger log)
        {
            BasicResponse basicResponse= new BasicResponse();
            log.LogInformation("GroupMeBot trigger processed a request.");

            string name = req.Query["name"];

            log.LogInformation($"GroupMeBot trigger message attempt to parse incoming request:");
            var working = basicResponse.ParseIncomingRequestAsync(req);
            await Task.WhenAll(working);
            log.LogInformation($"GroupMeBot trigger message: {working.Result}");

            try
            {
                string text = basicResponse.Message.Text;
                log.LogInformation($"GroupMeBot trigger message body text: {text}");
            }
            catch (NullReferenceException nex)
            {
                log.LogInformation($"Error: there was probably no HTTP body which prevented the app from getting text {nex.Message}");
            }
            catch (Exception ex)
            {
                log.LogInformation(ex.Message);
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            log.LogInformation($"GroupMeBot trigger message name: {name}");

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

        //TODO: Put all of the below methods in this class into a separate utility (or whatever) class that has an interface that you can test.

        /// <summary>
        /// Parses an incoming HTTP request into a GroupMe message
        /// </summary>
        /// <param name="req">Incoming request</param>
        /// <returns>True if a message was properly parsed from the request</returns>
        public async Task<bool> ParseIncomingRequestAsync(HttpRequest req)
        {
            if (req == null)
            {
                return false;
            }

            string content = String.Empty;
            using (StreamReader sr = new StreamReader(req.Body))
            {
                content = await sr.ReadToEndAsync();
            }

            Message = JsonConvert.DeserializeObject<MessageItem>(content);
            return Message?.Text != null;
        }

        /// <summary>
        /// Parses an incoming http request and prints out all of the req headers
        /// </summary>
        /// <param name="req">Incoming request</param>
        /// <returns>will return a string of all of the headers</returns>
        public async Task<string> ParseIncomingHeadersAsync(HttpRequest req)
        {
            if (req == null)
            {
                return "";
            }

            var builder = new StringBuilder(Environment.NewLine);
            foreach (var header in req.Headers)
            {
                builder.AppendLine($"{header.Key}: {header.Value}");
            }
            var headersDump = builder.ToString();

            return headersDump;
        }

        /// <summary>
        /// Checks to see if a specific word is present in the body of the message
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public async Task<HttpResponse> AnswerYesOrNo(HttpRequest req, ILogger log, string triggerWord = "test")
        {
            if (req == null)
            {

                return await CreateEmptyResponse(req, log);
            }

            bool containsTrigger = Message.Text.ToString().Contains(triggerWord, StringComparison.OrdinalIgnoreCase);
            log.LogInformation($"Does the message body contain this triggerWord: {triggerWord}? {containsTrigger}");
            if (containsTrigger)
            {
                //Create and send a message back

                return containsTrigger;
            }
            else
            {
                return false;
            }
        }

        public async Task<HttpResponse> CreateEmptyResponse(HttpRequest req, ILogger log)
        {
            HttpResponse response = new HttpResponse(HttpStatusCode.BadRequest);


        }

        public async Task<HttpResponse> SendResponseToGroupMe(HttpRequest req, ILogger log)
        {

        }

        public async Task<HttpResponse> CreateJSONResponse(HttpRequest req, ILogger log)
        {

        }

        public static async Task Run(HttpRequest req, ILogger log)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("");

                HttpResponseMessage response = await client.GetAsync("https://example");

                string responseBody = await response.Content.ReadAsStringAsync();

                log.LogInformation(responseBody);
            }
        }
    }
}
