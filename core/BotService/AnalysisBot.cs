using GroupMeBot.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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