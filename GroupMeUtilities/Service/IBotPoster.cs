namespace GroupMeUtilities.Service;

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
    Task<HttpStatusCode> PostAsync(string text, string botId);
}
