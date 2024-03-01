using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using GroupMeBot.Model;
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
        try
        {
            Log.LogInformation("MessageBot-HandleIncomingTextAsync method start");
            if (_message.Text == null)
            {
                Log.LogWarning("MessageBot-message.text is null");
                return HttpStatusCode.BadRequest;
            }

            // Todo: Make this into a bitwise operator (?)
            if (_message.UserId == "4635437") return await ChooseUniqueUserTextAsync("Andrew");
            else if (_message.UserId == "20597076") return await ChooseUniqueUserTextAsync("Logan");
            else if (_message.UserId == "7663415") return await ChooseUniqueUserTextAsync("Sean");
            else if (_message.UserId == "11900950") return await ChooseUniqueUserTextAsync("Jordan");
            else if (_message.UserId == "84706251") return await ChooseUniqueUserTextAsync("Hayden");
            else return await ChooseUniqueUserTextAsync();
        }
        catch (Exception ex)
        {
            Log.LogError($"MessageBot-HandleIncomingTextAsync method failed, {ex}");
            return HttpStatusCode.BadRequest;
        }
    }

    public async Task<HttpStatusCode> ChooseUniqueUserTextAsync(string user = "")
    {
        try
        {
            Log.LogInformation($"MessageBot-ChooseUniqueUserTextAsync method start for user: {user}");
            string response = RetrieveRandomResponse(user);
            Log.LogInformation($"MessageBot-Response for user: {user} retrieved: {response}");

            return await MessageOutgoing.PostAsync($"{response}", _botId);
        }
        catch(Exception ex)
        {
            Log.LogError($"MessageBot-ChooseUniqueUserTextAsync method failed, {ex}");
            return HttpStatusCode.BadRequest;
        }

    }


    public string RetrieveRandomResponse(string person = "")
    {
        try
        {
            Log.LogInformation("RetrieveRandomResponse");
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
        catch(Exception ex)
        {
            Log.LogError($"MessageBot-RetrieveRandomResponse method failed, {ex}");
            return null;
        }
    }

}
