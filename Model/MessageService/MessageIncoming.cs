using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.BotService;

namespace GroupMeBot.Model;

public class MessageIncoming : IMessageIncoming
{
    public MessageItem Message { get; set; }
    private readonly HttpRequest _req;
    private static readonly Regex BotAnalysisRegex = new Regex(@"((?i)(\bbot\b.*\banalysis\b)|(\banalysis\b.*\bbot\b)(?-i))");
    private static readonly Regex BotMessageRegex = new Regex(@"((?i)(\bbot\b.*\bmessage\b)|(\bmessage\b.*\bbot\b)(?-i))");
    public ILogger log { get; private set; }
    private const string _botId = "a4165ae5f7ad5ab682e2c3dd52";


    public MessageIncoming(HttpRequest req, ILogger logger)
    {
        _req = req;
        Message = new MessageItem();
        log = logger;
    }

    public async Task<IActionResult> ParseIncomingRequestAsync()
    {
        log.LogInformation("Parse Incoming Request-Attempting to parse incoming request");
        if (_req == null)
        {
            return new BadRequestObjectResult("No request was received");
        }

        //This is the problem.  I am not putting the text 
        string content = string.Empty;
        using (StreamReader sr = new StreamReader(_req.Body))
        {
            log.LogInformation("Parse Incoming Request-reading HttpRequest");
            content = await sr.ReadToEndAsync();
        }


        log.LogInformation($"Parse Incoming Request-read HttpRequest: {content}");
        if (content != null)
        {
            Message = JsonConvert.DeserializeObject<MessageItem>(content)!;
            log.LogInformation($"Parse Incoming Request-Message text content: {Message.Text}");
        }

        if (Message.UserId == _botId)
        {
            log.LogInformation($"Parse Incoming Request-returning OKObjectResult No response required, message is from bot");
            return new OkObjectResult("No response required, message is from bot");
        }
        if (Message.Text == null)
        {
            log.LogInformation($"Parse Incoming Request-returning BadRequestObjectResult No text was received");
            return new BadRequestObjectResult("No text was received");
        }

        log.LogInformation($"Parse Incoming Request-attempting regex match");
        Match analysisRegex = BotAnalysisRegex.Match(Message.Text);
        if (analysisRegex.Success)
        {
            log.LogInformation($"Parse Incoming Request-analysis regex match successful");
            AnalysisBot analysisBot = new AnalysisBot(Message, log);
            // Todo: create method: analysisBot.RunAnalysis()
            return new OkObjectResult(analysisBot);
        }

        Match messageRegex = BotMessageRegex.Match(Message.Text);
        if (messageRegex.Success)
        {
            log.LogInformation($"Parse Incoming Request-regex match successful");
            MessageBot messageBot = new MessageBot(Message, log);


            log.LogInformation($"Parse Incoming Request-HandleIncomingTextAsync");
            var status = await messageBot.HandleIncomingTextAsync();
            if (status == HttpStatusCode.OK)
            {
                log.LogInformation($"Parse Incoming Request-returning OKObjectResult because HttpStatusCode Ok");
                return new OkObjectResult(status);
            }
            else if (status == HttpStatusCode.BadRequest)
            {
                log.LogInformation($"Parse Incoming Request-returning BadRequestObjectResult because HttpStatusCode BadRequest");
                return new BadRequestObjectResult(status);
            }
        }

        log.LogInformation($"Parse Incoming Request-final log, didn't trip any if statements, return OkObjectResult");
        return new OkObjectResult("No response criteria met, no response required");
    }


    public string ParseIncomingHeadersAsync()
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
