using Microsoft.AspNetCore.Mvc;

namespace GroupMeBot.Model;

public interface IMessageIncoming
{
    public MessageItem Message { get; set; }

    /// <summary>
    /// Parses an incoming http request and sends it to the message bot or the analysis bot
    /// </summary>
    /// <returns>Returns an ObjectResult (that inherits from IActionResult) that will be sent to the client as an HttpResponse</returns>
    public Task<IActionResult> ParseIncomingRequestAsync();

    /// <summary>
    /// Parses an incoming http request and prints out all of the req headers
    /// </summary>
    /// <returns>will return a string of all of the headers</returns>
    public Task<string> ParseIncomingHeadersAsync();

}
