namespace Core.PostService;

using GroupMeUtilities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public interface IBotPoster
{
    /// <summary>
    /// Posts a text message to the group
    /// </summary>
    /// <param name="text"></param>
    /// <param name="botId"></param>
    /// <returns></returns>
    public Task<HttpStatusCode> PostAsync(string text, string botId);

    /// <summary>
    /// Posts a bot message to the service
    /// </summary>
    /// <param name="request">Request to post</param>
    /// <returns>Response code from the GroupMe service</returns>
    public Task<HttpStatusCode> PostBotMessage(CreateBotPostRequest request);

}
