namespace GroupMeUtilities.Service;

using GroupMeUtilities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


public class BotPoster : IBotPoster
{
    private string botPostUrl;

    public BotPoster(string BotPostUrl)
    {
        botPostUrl = BotPostUrl;
    }

    /// <summary>
    /// Posts a basic text message
    /// </summary>
    /// <param name="text">Text to send to the group</param>
    /// <param name="botId">ID of the bot that sends the message</param>
    /// <returns>The status of the outgoing operation to post the message</returns>
    public async Task<HttpStatusCode> PostAsync(string text, string botId)
    {
        var post = new CreateBotPostRequest
        {
            BotId = botId,
            Text = text
        };
        return await PostBotMessage(post);
    }

    /// <summary>
    /// Posts a bot message to the service
    /// </summary>
    /// <param name="request">Request to post</param>
    /// <returns>Response code from the GroupMe service</returns>
    private async Task<HttpStatusCode> PostBotMessage(CreateBotPostRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        //TODO: this is where you left off on 10 May 2023
        //Understand this before you finalize it in your code.
        //using (StringContent content = JsonSerializer.SerializeToJson(request))
        //{
        //    var client = new HttpClient();
        //    HttpResponseMessage result = await client.PostAsync(_botPostUrl, content);
        //    return result != null ? result.StatusCode : HttpStatusCode.ServiceUnavailable;
        //}

    }
}

