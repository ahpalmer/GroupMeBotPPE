using GroupMeUtilities.Model;
using System.Net.Http.Headers;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Core.IncomingService
{
    public class IncomingMessage
    {

        public IncomingMessage() 
        {
        }

        public async Task<bool> ParseIncomingRequestAsync(HttpClient client)
        {
            if (req == null)
            {
                return false;
            }

            string content = String.Empty;
            using (StreamReader sr = new StreamReader(req.Body))
            {
                content = await sr.ReadToEndAsync();
            }

            Message = JsonConvert.DeserializeObject<MessageItem>(content);
            return Message?.Text != null;
        }
    }
}
