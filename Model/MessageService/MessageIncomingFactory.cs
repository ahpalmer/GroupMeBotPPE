using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GroupMeBot.Model;

public class MessageIncomingFactory : IMessageIncomingFactory
{
    public IMessageIncoming Create(HttpRequest req, ILogger logger)
    {
        return new MessageIncoming(req, logger);
    }
}
