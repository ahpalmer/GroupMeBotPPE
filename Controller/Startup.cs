using GroupMeBot.Model;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Logging;
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
                .AddJsonFile($"appsettings.{context.EnvironmentName}.json", optional: true)
                //.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .Build();

            // Register HttpClient
            builder.Services.AddHttpClient();

            // Retrieve the BotPostUrl from configuration
            var botPostUrl = configuration["BotPostUrl"];
            var botId = configuration["BotId"];

            // Register services
            //builder.Services.Configure<BotPostConfiguration>(options =>
            //{
            //    options.BotPostUrl = botPostUrl;
            //    options.BotId = botId;
            //});

            // Add the bot post URL and Id to the DI container:
            builder.Services.Configure<BotPostConfiguration>(configuration.GetSection("BotPostConfiguration"));

            // Add the services to the DI container
            builder.Services.AddSingleton<IAnalysisBot, AnalysisBot>();
            builder.Services.AddSingleton<IMessageBot, MessageBot>();
            builder.Services.AddSingleton<IMessageIncoming, MessageIncoming>();
            builder.Services.AddSingleton<IMessageOutgoing, MessageOutgoing>();
            builder.Services.AddLogging();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        // Add JSON responses as DI singletons.  
        // Todo: not working right now
        //var responseFilePathsConfig = configuration.GetSection("ResponseFilePaths").Get<ResponseFilePaths>();
        //builder.Services.AddSingleton(responseFilePathsConfig);

        // Attempt 2:

    }
}
