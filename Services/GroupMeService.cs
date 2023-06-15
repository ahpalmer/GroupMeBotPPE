using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GroupMeBot.Services
{
    /// <summary>
    /// Used to access GroupMe service functions
    /// </summary>
    public class GroupMeService
    {
        private const string PageParameterName = "page";
        private const string PerPageParameterName = "per_page";
        private const string AccessTokenHeader = "X-Access-Token";
        private const string UserAgentHeader = "User-Agent";
        private const string UserAgentHeaderValue = "twitterbot";

        private const string GroupsUrl = "https://api.groupme.com/v3/groups";
        private const string UserUrl = "https://api.groupme.com/v3/users/me";
    }
}
