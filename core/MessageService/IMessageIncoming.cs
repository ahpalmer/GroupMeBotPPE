using GroupMeUtilities.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MessageService;

public interface IMessageIncoming
{
    public MessageItem Message { get; set; }

    /// <summary>
    /// Parses an incoming http request and sends it to the message bot or the analysis bot
    /// </summary>
    /// <returns>returns an HttpResponse that will be sent to the GroupMe</returns>
    public Task<HttpResponseMessage> ParseIncomingRequestAsync();

    /// <summary>
    /// Parses an incoming http request and prints out all of the req headers
    /// </summary>
    /// <returns>will return a string of all of the headers</returns>
    public Task<string> ParseIncomingHeadersAsync();

}
