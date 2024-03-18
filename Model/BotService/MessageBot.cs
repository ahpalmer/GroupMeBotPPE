using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using GroupMeBot.Model;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace GroupMeBot.Model;

public class MessageBot : IMessageBot
{
    private IMessageOutgoing _messageOutgoing;
    private ResponseFilePaths _responseFilePaths;
    private ILogger _log;

    private static readonly Regex _botCannedResponseRegex = new Regex(@"((?i)(?=.*\bbot\b)(?=.*\bmessage\b)(?=.*\bresponse\b)(?-i))");
    private const string _botPostUrl = "https://api.groupme.com/v3/bots/post";
    private const string _botId = "a4165ae5f7ad5ab682e2c3dd52";

    public MessageBot(IMessageOutgoing messageOutgoing, ResponseFilePaths responseFilePaths, ILogger log)
    {
        _messageOutgoing = messageOutgoing;
        _responseFilePaths = responseFilePaths;
        _log = log;
    }

    public async Task<HttpStatusCode> HandleIncomingTextAsync(MessageItem message)
    {
        try
        {
            _log.LogInformation("MessageBot-HandleIncomingTextAsync method start");
            if (message.Text == null)
            {
                _log.LogWarning("MessageBot-message.text is null");
                return HttpStatusCode.BadRequest;
            }

            // Todo: Make this into a bitwise operator (?)
            if (message.UserId == "4635437") return await ChooseUniqueUserTextAsync("Andrew");
            else if (message.UserId == "20597076") return await ChooseUniqueUserTextAsync("Logan");
            else if (message.UserId == "7663415") return await ChooseUniqueUserTextAsync("Sean");
            else if (message.UserId == "11900950") return await ChooseUniqueUserTextAsync("Jordan");
            else if (message.UserId == "84706251") return await ChooseUniqueUserTextAsync("Hayden");
            else return await ChooseUniqueUserTextAsync();
        }
        catch (Exception ex)
        {
            _log.LogError($"MessageBot-HandleIncomingTextAsync method failed, {ex}");
            return HttpStatusCode.BadRequest;
        }
    }

    public async Task<HttpStatusCode> ChooseUniqueUserTextAsync(string user = "")
    {
        try
        {
            _log.LogInformation($"MessageBot-ChooseUniqueUserTextAsync method start for user: {user}");
            string path;

            switch (user)
            {
                case "Andrew":
                    path = _responseFilePaths.Andrew;
                    break;
                case "Logan":
                    path = _responseFilePaths.Logan;
                    break;
                case "Sean":
                    path = _responseFilePaths.Sean;
                    break;
                case "Jordan":
                    path = _responseFilePaths.Jordan;
                    break;
                case "Hayden":
                    path = _responseFilePaths.Hayden;
                    break;
                default:
                    path = _responseFilePaths.Responses;
                    break;
            }

            string response = RetrieveRandomResponse(path);

            _log.LogInformation($"MessageBot-Response for user: {user} retrieved: {response}");

            return await _messageOutgoing.PostAsync($"{response}", _botId);
        }
        catch(Exception ex)
        {
            _log.LogError($"MessageBot-ChooseUniqueUserTextAsync method failed, {ex}");
            return HttpStatusCode.BadRequest;
        }
    }


    public string RetrieveRandomResponse(string person)
    {
        try
        {
            _log.LogInformation("RetrieveRandomResponse");

            //string dir = Directory.GetCurrentDirectory();
            //path = dir + $"\\..\\..\\..\\Responses\\{person}Responses.json";



            using (StreamReader sr = new StreamReader(person))
            {
                string json = sr.ReadToEnd();
                List<string> responses = JsonConvert.DeserializeObject<List<string>>(json)!;
                int random = RandomNumberGenerator.GetInt32(0, responses.Count);
                return responses[random].ToString();
            }
        }
        catch(Exception ex)
        {
            _log.LogError($"MessageBot-RetrieveRandomResponse method failed, {ex}");
            // Todo: bad return
            return null;
        }
    }

}
