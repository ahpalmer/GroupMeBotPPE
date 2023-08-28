using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace GroupMeBot
{
    public class GroupMeBot
    {
        private const string BotPostUrl = "https://api.groupme.com/v3/bots/post";
        private string botId = "a4165ae5f7ad5ab682e2c3dd52";
        private readonly ILogger _logger;

        public string BotId
        {
            get
            {
                return this.botId;
            }
            set
            {
                this.botId = value;
            }
        }

        public GroupMeBot(ILogger logger)
        {
            _logger = logger;
        }

        [FunctionName("GroupMeBot")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(Route = $"GroupMeBot/a4165ae5f7ad5ab682e2c3dd52")] HttpRequest req,
            ILogger log, string botId)
        {
            log.LogInformation("GroupMeBot trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass in a name."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
