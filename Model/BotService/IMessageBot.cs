using Microsoft.AspNetCore.Mvc;

namespace GroupMeBot.Model;

public interface IMessageBot
{
    public MessageItem Message { get; set; }

    /// <summary>
    /// Parses incoming text and decides how to respond
    /// </summary>
    /// <returns>returns an ObjectResult (that inherits from IActionResult) that will be sent to the GroupMe</returns>
    public Task<IActionResult> HandleIncomingTextAsync();
}
