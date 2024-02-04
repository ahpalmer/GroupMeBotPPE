using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using GroupMeBot.Model;
using System.Text.Json.Nodes;
using System;

namespace Model.BotService;

public class MessageBot
{
    private MessageItem _message { get; set; }
    private static readonly Regex BotCannedResponseRegex = new Regex(@"((?i)(?=.*\bbot\b)(?=.*\bmessage\b)(?=.*\bresponse\b)(?-i))");
    private IMessageOutgoing _messageOutgoing;
    private const string _botPostUrl = "https://api.groupme.com/v3/bots/post";
    private ILogger _log;
    private const string _botId = "a4165ae5f7ad5ab682e2c3dd52";

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
            Log.LogWarning("MessageBot-message.text is null");
            return HttpStatusCode.BadRequest;
        }

        Log.LogInformation("MessageBot-attempt regex match");
        Match regexMatch = BotCannedResponseRegex.Match(_message.Text);
        if (regexMatch.Success)
        {
            Log.LogInformation("MessageBot-regex match was a success");
            // TODO: More logic that will handle HTTPStatuscodes from the botposter class
            await MessageOutgoing.PostAsync("Received Message Response Request", _botId);
            return HttpStatusCode.OK;
        }

        if (_message.UserId == "")
        {
            Log.LogInformation("MessageBot-received message from Andrew");
            string path = @"..//";
        }

        Log.LogInformation("MessageBot-no regex match bad request");
        return HttpStatusCode.BadRequest;
    }

    //public async Task<HttpStatusCode> HandleIncomingLoganTextAsync()
    //{
    //    Log.LogInformation("MessageBot-HandleIncomingLoganTextAsync method start");
    //    if (_message.Text == null)
    //    {
    //        Log.LogWarning("MessageBot-message.text is null");
    //        return HttpStatusCode.BadRequest;
    //    }

    //    Log.LogInformation("MessageBot-Logan Text attempt regex match");
    //    Match regexMatch = BotCannedResponseRegex.Match(_message.Text);
    //    if (regexMatch.Success)
    //    {
    //        Log.LogInformation("MessageBot-regex match was a success");
    //        await MessageOutgoing.PostAsync("Received Message Response Request", _botId);
    //        return HttpStatusCode.OK;
    //    }
    //}

    // Todo: This is where you left off
    public string RetrieveRandomResponse(string person = "")
    {
        _log.LogInformation("stuff");
        string path;

        string dir = Directory.GetCurrentDirectory();
        path = dir + "\\..\\..\\..\\data\\input.txt";

        return File.ReadAllText(path);
    }

}
