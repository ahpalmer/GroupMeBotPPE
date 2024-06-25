using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupMeBot.Model;

public class GiphyBotPostConfig : IGiphyBotPostConfig
{
    public string GiphyBotId { get; set; }

    public GiphyBotPostConfig(string giphyBotId)
    {
        GiphyBotId = giphyBotId;
    }
}
