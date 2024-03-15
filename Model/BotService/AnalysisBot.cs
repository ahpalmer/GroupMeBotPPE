using Microsoft.Extensions.Logging;

namespace GroupMeBot.Model;

public class AnalysisBot: IAnalysisBot
{
    public ILogger Log { get; set; }
    public AnalysisBot(ILogger log)
    {
        Log = log;
    }
}