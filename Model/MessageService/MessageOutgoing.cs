using Microsoft.Extensions.Options;
using System.Net;

namespace GroupMeBot.Model;

public class MessageOutgoing : IMessageOutgoing
{
    private IBotPostConfiguration _botPostConfiguration;

    public MessageOutgoing(BotPostConfiguration botPostConfiguration)
    {
        _botPostConfiguration = botPostConfiguration;
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

        using (StringContent content = JsonSerializer.SerializeToJson(request))
        {
            var client = new HttpClient();
            HttpResponseMessage result = await client.PostAsync(_botPostConfiguration.BotPostUrl, content);
            return result != null ? result.StatusCode : HttpStatusCode.BadRequest;
        }

    }
}

