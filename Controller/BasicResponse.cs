using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using GroupMeBot.Model;


namespace GroupMeBot.Controller;

// Todo: update to Net 8.0
public class BasicResponse
{
    private readonly IMessageIncoming _messageIncoming;
    private readonly ILogger _logger;

    public BasicResponse(IMessageIncoming messageIncoming, ILogger<BasicResponse> logger)
    {
        _messageIncoming = messageIncoming;
        _logger = logger;
    }

    /// <summary>
    /// This is the main azure function.  It is only used to receive an HTTPRequest, pass it to the Model, and return an appropriate HTTPResponse to the client.
    /// </summary>
    public MessageItem Message { get; set; }

    [FunctionName("BasicResponse")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "GroupMeBot/BasicResponse")] HttpRequest req)
    {
        _logger.LogInformation("GroupMeBot trigger processed a request.");
        //MessageItem incMessage = new MessageItem(req.ContentType.ToString());



        _logger.LogInformation($"GroupMeBot trigger message attempt to parse incoming request");

        _logger.LogInformation($"GroupMeBot trigger message: http request body: {req}");
        IActionResult httpResponse = await _messageIncoming.ParseIncomingRequestAsync(req);

        return httpResponse;
    }
}
