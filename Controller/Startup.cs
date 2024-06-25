using GroupMeBot.Model;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(GroupMeBot.Controller.Startup))]

namespace GroupMeBot.Controller;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        try
        {
            var context = builder.GetContext();

            // Load configurations
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(context.ApplicationRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<Startup>(optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var botPostUri = configuration["GroupMePostUri"];
            var giphyBotId = configuration["GiphyBotId"];
            var groupMeBotId = configuration["GroupMeBotId"];


            // Register HttpClient
            builder.Services.AddHttpClient();

            // Add the services to the DI container
            builder.Services.AddSingleton<IAnalysisBot, AnalysisBot>();
            builder.Services.AddSingleton<IMessageBot, MessageBot>();
            builder.Services.AddSingleton<IGifBot, GifBot>();
            builder.Services.AddSingleton<IMessageIncoming, MessageIncoming>();
            builder.Services.AddSingleton<IMessageOutgoing, MessageOutgoing>();
            builder.Services.AddSingleton<IBotPostConfiguration>(new BotPostConfiguration(botPostUri, groupMeBotId));
            builder.Services.AddSingleton<IGiphyBotPostConfig>(new GiphyBotPostConfig(giphyBotId));
            builder.Services.AddLogging();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
