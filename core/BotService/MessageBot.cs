using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using GroupMeBot.Model;

namespace Model.BotService;

public class MessageBot
{
    private MessageItem _message { get; set; }
    private static readonly Regex BotCannedResponseRegex = new Regex(@"((?i)(?=.*\bbot\b)(?=.*\bmessage\b)(?=.*\bresponse\b)(?-i))");
    private IMessageOutgoing _messageOutgoing;
    private const string _botPostUrl = "https://api.groupme.com/v3/bots/post";
    private ILogger _log;

    public MessageBot(MessageItem message, ILogger log)
    {
        _message = message;
        _messageOutgoing = new MessageOutgoing(_botPostUrl);
        _log = log;
    }

    public ILogger Log
    {
        get { return _log; }
        set { _log = value; }
    }

    // TODO: Convert to DI
    public IMessageOutgoing MessageOutgoing
    {
        get { return _messageOutgoing; }
    }

    public async Task<HttpStatusCode> HandleIncomingTextAsync()
    {
        Log.LogInformation("MessageBot-HandleIncomingTextAsync method start");
        if (_message.Text == null)
        {
            Log.LogInformation("MessageBot-message.text is null");
            return HttpStatusCode.BadRequest;
        }

        Log.LogInformation("MessageBot-attempt regex match");
        Match regexMatch = BotCannedResponseRegex.Match(_message.Text);
        if (regexMatch.Success)
        {
            Log.LogInformation("MessageBot-regex match was a success");
            // TODO: More logic that will handle HTTPStatuscodes from the botposter class
            await MessageOutgoing.PostAsync("Received Message Response Request", "a4165ae5f7ad5ab682e2c3dd52");
            return HttpStatusCode.OK;
        }

        Log.LogInformation("MessageBot-no regex match bad request");
        return HttpStatusCode.BadRequest;
    }

}
