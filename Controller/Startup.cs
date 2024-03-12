using GroupMeBot.Model;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Model.BotService;

[assembly: FunctionsStartup(typeof(Controller.Startup))]

namespace Controller;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddHttpClient();
        builder.Services.AddSingleton<IMessageIncomingFactory, MessageIncomingFactory>();
        builder.Services.AddSingleton<IMessageOutgoing, MessageOutgoing>();
        builder.Services.AddSingleton<IAnalysisBot, AnalysisBot>();
        builder.Services.AddSingleton<IMessageBot, MessageBot>();
        builder.Services.AddModel();
        // Todo: add MessageItem to DI
        // Todo: don't forget to remove MessageItem from MessageBot constructor
    }
}
