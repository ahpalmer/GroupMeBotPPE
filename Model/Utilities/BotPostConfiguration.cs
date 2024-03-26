using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupMeBot.Model;

/// <summary>
/// This is an controller that is used with DI to return the bot post URL and ID
/// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-8.0
/// https://stackoverflow.com/questions/77931443/how-to-bind-json-data-to-class-with-dependency-injection-like-asp-net-core
/// </summary>
public class BotPostConfiguration
{
    public string BotPostUrl { get; set; }
    public string BotId { get; set; }
}
