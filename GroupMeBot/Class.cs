namespace GroupMeBot;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class SendResponse
{
    public static async Task Run(HttpRequest req, ILogger log)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("");

            HttpResponseMessage response = await client.GetAsync("https://example");

            string responseBody = await response.Content.ReadAsStringAsync();

            log.LogInformation(responseBody);
        }
    }
}
