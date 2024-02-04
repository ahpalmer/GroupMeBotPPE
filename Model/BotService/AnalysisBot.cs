using Microsoft.Extensions.Logging;

namespace GroupMeBot.Model;

public class AnalysisBot: IAnalysisBot
{
    public MessageItem Message {  get; set; }
    public ILogger Log { get; set; }
    public AnalysisBot(MessageItem message, ILogger log)
    {
        Message = message;
        Log = log;
    }
}