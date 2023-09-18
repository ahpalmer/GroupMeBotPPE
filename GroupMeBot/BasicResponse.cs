using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using GroupMeBot.Model;
using System.Text;
using System.Runtime.CompilerServices;
using System.Linq;

namespace GroupMeBot.Controller;

public class BasicResponse
{
    private const string BotPostUrl = "https://api.groupme.com/v3/bots/post";

    /// <summary>
    /// This is the main azure function.  It is only used to receive an HTTPRequest, pass it to the Model, and return an appropriate HTTPResponse to the client.
    /// </summary>
    public MessageItem Message { get; set; }

    [FunctionName("BasicResponse")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "GroupMeBot/BasicResponse")] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("GroupMeBot trigger processed a request.");
        MessageIncoming incMessage = new MessageIncoming(req, log);

        //Is this how to send a response?!?
        //req.HttpContext.Response.StatusCode = 418;

        log.LogInformation($"GroupMeBot trigger message attempt to parse incoming request:");
        var httpResponse = await incMessage.ParseIncomingRequestAsync();

        try
        {
            string text = incMessage.Message.Text;
            log.LogInformation($"GroupMeBot trigger text: {incMessage.Message.Text}");
        }
        catch (NullReferenceException nex)
        {
            log.LogInformation($"Error: there was probably no HTTP body which prevented the app from getting text {nex.Message}");
        }
        catch (Exception ex)
        {
            log.LogInformation(ex.Message);
        }

        return httpResponse;
    }
}
