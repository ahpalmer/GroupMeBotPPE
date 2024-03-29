﻿using Microsoft.Extensions.Options;
using System.Net;

namespace GroupMeBot.Model;

public class MessageOutgoing : IMessageOutgoing
{
    private BotPostConfiguration _botPostConfiguration;

    public MessageOutgoing(IOptions<BotPostConfiguration> botPostConfiguration)
    {
        _botPostConfiguration = botPostConfiguration.Value;
    }

    /// <inheritdoc/>
    public async Task<HttpStatusCode> PostAsync(string text, string botId)
    {
        var post = new CreateBotPostRequest(botId, text);
        return await PostBotMessage(post);
    }

    /// <inheritdoc/>
    public async Task<HttpStatusCode> PostBotMessage(CreateBotPostRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        //TODO: this is where you left off on 10 May 2023
        //Understand this before you finalize it in your code.
        //Here is the explanation: You use the using () method as a form of garbage collection.
        //The using() method is actually shorthand for a try{} finally{garbage collection} block
        //You must use the using() method for any class that is based on the IDisposable base class.
        //StringContent is based on a class that is based on a class that is based on IDisposable.
        //Therefore, to use StringContent you must utilize the using() method
        using (StringContent content = JsonSerializer.SerializeToJson(request))
        {
            var client = new HttpClient();
            HttpResponseMessage result = await client.PostAsync(_botPostConfiguration.BotPostUrl, content);
            return result != null ? result.StatusCode : HttpStatusCode.BadRequest;
        }

    }
}

