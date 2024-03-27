using Microsoft.Extensions.Logging;

namespace GroupMeBot.Model;

public class AnalysisBot: IAnalysisBot
{
    public ILogger<AnalysisBot> Log { get; set; }
    public AnalysisBot(ILogger<AnalysisBot> log)
    {
        Log = log;
    }
}