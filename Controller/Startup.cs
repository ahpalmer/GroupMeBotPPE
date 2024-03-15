using GroupMeBot.Model;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Model.BotService;

[assembly: FunctionsStartup(typeof(Controller.Startup))]

namespace Controller;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var context = builder.GetContext();

        // Load configurations
        var configuration = new ConfigurationBuilder()
            .SetBasePath(context.ApplicationRootPath)
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        // Register HttpClient
        builder.Services.AddHttpClient();

        // Register services
        builder.Services.AddSingleton<IAnalysisBot, AnalysisBot>();
        builder.Services.AddSingleton<IMessageBot, MessageBot>();
        builder.Services.AddSingleton<IMessageIncoming, MessageIncoming>();

        // Retrieve the BotPostUrl from configuration
        var botPostUrl = configuration["BotPostUrl"];

        // Register MessageOutgoing with BotPostUrl from configuration
        builder.Services.AddSingleton<IMessageOutgoing>(provider => new MessageOutgoing(botPostUrl));

        // Todo: add all JSON responses as DI singletons
    }
}
