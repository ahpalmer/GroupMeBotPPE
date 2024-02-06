using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using GroupMeBot.Model;
using System.Text.Json.Nodes;
using System;
using System.Security.Cryptography;
using Newtonsoft.Json;

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

        // Todo: Delete this once HandleIncomingText is working
        //Log.LogInformation("MessageBot-attempt regex match");
        //Match regexMatch = BotCannedResponseRegex.Match(_message.Text);
        //if (regexMatch.Success)
        //{
        //    Log.LogInformation("MessageBot-regex match was a success");
        //    // TODO: More logic that will handle HTTPStatuscodes from the botposter class
        //    await MessageOutgoing.PostAsync("Received Message Response Request", _botId);
        //    return HttpStatusCode.OK;
        //}

        // Todo: Make this into a bitwise operator (?)
        if (_message.UserId == "Andrew") return await HandleAndrewTextAsync();
        else if (_message.UserId == "Logan") return await HandleLoganTextAsync();
        else return await HandleAnonymousTextAsync();
    }

    public async Task<HttpStatusCode> HandleAnonymousTextAsync()
    {
        Log.LogInformation("MessageBot-HandleAndrewTextAsync method start");
        string response = RetrieveRandomResponse();

        return await MessageOutgoing.PostAsync($"{response}", _botId);
    }

    public async Task<HttpStatusCode> HandleAndrewTextAsync()
    {
        Log.LogInformation("MessageBot-HandleAndrewTextAsync method start");
        string response = RetrieveRandomResponse("Andrew");

        return await MessageOutgoing.PostAsync($"{response}", _botId);
    }

    public async Task<HttpStatusCode> HandleLoganTextAsync()
    {
        Log.LogInformation("MessageBot-HandleLoganTextAsync method start");
        string response = RetrieveRandomResponse("Logan");

        return await MessageOutgoing.PostAsync($"{response}", _botId);
    }

    public string RetrieveRandomResponse(string person = "")
    {
        _log.LogInformation("RetrieveRandomResponse");
        string path;

        string dir = Directory.GetCurrentDirectory();
        path = dir + $"\\..\\..\\..\\Responses\\{person}Responses.json";

        using (StreamReader sr = new StreamReader(path))
        {
            string json = sr.ReadToEnd();
            List<string> responses = JsonConvert.DeserializeObject<List<string>>(json)!;
            int random = RandomNumberGenerator.GetInt32(0, responses.Count);
            return responses[random].ToString();
        }
    }

}
