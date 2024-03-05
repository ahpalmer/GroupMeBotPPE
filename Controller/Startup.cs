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
        // Todo: create a IMessageIncoming factory instead of the direct class so that you can inject the httprequest into the IMessageIncoming class
        builder.Services.AddSingleton<IMessageIncoming, MessageIncoming>();
        builder.Services.AddSingleton<IMessageOutgoing, MessageOutgoing>();
        builder.Services.AddSingleton<IAnalysisBot, AnalysisBot>();
        builder.Services.AddSingleton<IMessageBot, MessageBot>();
    }
}
