using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace GroupMeBot.Model;

public class MessageBot
{
    private MessageItem _message { get; set; }
    private static readonly Regex BotCannedResponseRegex = new Regex("((?i)(?=.*\bbot\b)(?=.*\bmessage\b)(?=.*\bresponse\b)(?-i))");
    private IMessageOutgoing _botPoster;
    private const string BotPostUrl = "https://api.groupme.com/v3/bots/post";

    public MessageBot(MessageItem message)
    {
        _message = message;
        _botPoster = new MessageOutgoing(BotPostUrl);
    }

    public async Task<HttpStatusCode> HandleIncomingTextAsync()
    {
        if (_message.Text == null)
        {
            return HttpStatusCode.BadRequest;
        }

        Match regexMatch = BotCannedResponseRegex.Match(_message.Text);
        if (regexMatch.Success)
        {
            return await _botPoster.PostAsync("Received Message Response Request", "a4165ae5f7ad5ab682e2c3dd52");
        }

        return HttpStatusCode.BadRequest;
    }

}
