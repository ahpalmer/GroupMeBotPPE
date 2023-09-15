using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GroupMeBot.Model;

public class MessageIncoming : IMessageIncoming
{
    public MessageItem Message { get; set; }
    private readonly HttpRequest _req;
    private static readonly Regex BotAnalysisRegex = new Regex("((?i)(\bbot\b.*\banalysis\b) | (\banalysis\b.*\bbot\b)(?-i))");
    private static readonly Regex BotMessageRegex = new Regex("((?i)(\bbot\b.*\bmessage\b) | (\bmessage\b.*\bbot\b)(?-i))");

    public MessageIncoming(HttpRequest req)
    {
        _req = req;
        Message = new MessageItem();
    }

    public async Task<IActionResult> ParseIncomingRequestAsync()
    {

        if (_req == null)
        {
            return new BadRequestObjectResult("No request was received");
        }

        string content = string.Empty;
        using (StreamReader sr = new StreamReader(_req.Body))
        {
            content = await sr.ReadToEndAsync();
        }

        if (content != null)
        {
            Message = JsonConvert.DeserializeObject<MessageItem>(content)!;
        }

        if (Message.UserId == "a4165ae5f7ad5ab682e2c3dd52")
        {
            return new OkObjectResult("No response required, message is from bot");
        }

        if (Message.Text == null)
        {
            return new BadRequestObjectResult("No text was received");
        }

        Match analysisBot = BotAnalysisRegex.Match(Message.Text);
        if (analysisBot.Success)
        {
            //Run analysis code
            throw new NotImplementedException();
        }

        Match messageRegex = BotMessageRegex.Match(Message.Text);
        if (messageRegex.Success)
        {
            MessageBot messageBot = new MessageBot(Message);
            var status = await messageBot.HandleIncomingTextAsync();
            
        }

        return new OkObjectResult("No response criteria met, no response required");
    }


    public async Task<string> ParseIncomingHeadersAsync()
    {
        if (_req == null)
        {
            return "";
        }

        var builder = new StringBuilder(Environment.NewLine);
        foreach (var header in _req.Headers)
        {
            builder.AppendLine($"{header.Key}: {header.Value}");
        }
        var headersDump = builder.ToString();

        return headersDump;
    }
}
