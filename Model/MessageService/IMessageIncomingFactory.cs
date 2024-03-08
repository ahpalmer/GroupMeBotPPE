using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GroupMeBot.Model;

public interface IMessageIncomingFactory
{
    /// <summary>
    /// This method will allow me to inject an httprequest object into the message incoming class, but still allow message incoming to be initiated during dependency injection.
    /// </summary>
    /// <param name="req">The entire Http Request</param>
    IMessageIncoming Create(HttpRequest req, ILogger logger);
}
