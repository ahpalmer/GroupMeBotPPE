using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace GroupMeBot.Model;

public class MessageBot
{
    private MessageItem _message { get; set; }
    private static readonly Regex BotCannedResponseRegex = new Regex("((?i)(?=.*\bbot\b)(?=.*\bmessage\b)(?=.*\bresponse\b)(?-i))");
    private IMessageOutgoing _botPoster;
    private const string BotPostUrl = "https://api.groupme.com/v3/bots/post";
    private ILogger _log;

    public MessageBot(MessageItem message, ILogger log)
    {
        _message = message;
        _botPoster = new MessageOutgoing(BotPostUrl);
        _log = log;
    }

    public ILogger Log
    {
        get { return _log; }
        set { _log = value; }
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
            return await _botPoster.PostAsync("Received Message Response Request", "a4165ae5f7ad5ab682e2c3dd52");
        }

        Log.LogInformation("MessageBot-attempt regex match");
        return HttpStatusCode.BadRequest;
    }

}
