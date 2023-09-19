using GroupMeBot.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
