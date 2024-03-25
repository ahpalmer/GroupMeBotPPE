using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

using GroupMeBot.Model;
using Newtonsoft.Json;
using System.IO;

namespace GroupMeBot.Controller;

// Todo: update to Net 8.0
public class BasicResponse
{
    private readonly IMessageIncoming _messageIncoming;

    // Todo: replace this with a json file variable
    private const string BotPostUrl = "https://api.groupme.com/v3/bots/post";

    public BasicResponse(IMessageIncoming messageIncoming)
    {
        _messageIncoming = messageIncoming;
    }

    /// <summary>
    /// This is the main azure function.  It is only used to receive an HTTPRequest, pass it to the Model, and return an appropriate HTTPResponse to the client.
    /// </summary>
    public MessageItem Message { get; set; }

    [FunctionName("BasicResponse")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "GroupMeBot/BasicResponse")] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("GroupMeBot trigger processed a request.");
        //MessageItem incMessage = new MessageItem(req.ContentType.ToString());



        log.LogInformation($"GroupMeBot trigger message attempt to parse incoming request");

        log.LogInformation($"GroupMeBot trigger message: http request body: {req}");
        IActionResult httpResponse = await _messageIncoming.ParseIncomingRequestAsync(req);

        return httpResponse;
    }
}
