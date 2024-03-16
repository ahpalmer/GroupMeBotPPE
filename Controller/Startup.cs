using GroupMeBot.Model;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(GroupMeBot.Controller.Startup))]

namespace GroupMeBot.Controller;

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

        // Add JSON responses as DI singletons
        var responseFilePathsConfig = configuration.GetSection("ResponseFilePaths").Get<ResponseFilePaths>();

        // Retrieve the BotPostUrl from configuration
        var botPostUrl = configuration["BotPostUrl"];

        // Register services
        builder.Services.AddSingleton<IAnalysisBot, AnalysisBot>();
        builder.Services.AddSingleton<IMessageBot, MessageBot>();
        builder.Services.AddSingleton<IMessageIncoming, MessageIncoming>();
        builder.Services.AddSingleton(responseFilePathsConfig);
        builder.Services.AddSingleton<IMessageOutgoing>(provider => new MessageOutgoing(botPostUrl));
    }
}
