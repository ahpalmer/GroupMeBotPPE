using System;
using System.IO;
using System.Threading.Tasks;
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
            //TODO: Handle an HTTPresponse that does not contain headers or a body.  Right now it's breaking my code and azure probably won't handle it super well.
            MessageItem message = null;
            BasicResponse basicResponse= new BasicResponse();
            log.LogInformation("GroupMeBot trigger processed a request.");

            //log.LogInformation($"GroupMeBot trigger headers:  {req.Headers.ToString()}");

            string name = req.Query["name"];

            log.LogInformation($"GroupMeBot trigger message attempt to parse incoming request:");
            var working = basicResponse.ParseIncomingRequestAsync(req);

            //This async logic right now is pointless.  It runs the ParseIncomingRequest method "asynchronously" and then waits for it to be done before moving on.  I'm doing it to allow for future functionality
            await Task.WhenAll(working);
            log.LogInformation($"GroupMeBot trigger message: {working.ToString()}");

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

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

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

            //string text = JsonConvert.DeserializeObject(content).ToString();
            //Todo: this is from the example and it's probably more interesting, but I can't get it to work yet.
            Message = JsonConvert.DeserializeObject<MessageItem>(content);
            return Message?.Text != null;
        }
    }
}
