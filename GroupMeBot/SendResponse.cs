using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GroupMeBot.Controller;

public class SendResponse
{
    public static HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("https://api.groupme.com/v3/bots/post"),
    };

    public static async Task PostAsync(HttpClient httpClient)
    {
        using StringContent jsonContent = new(
        JsonSerializer.Serialize(new
        {
            bot_id = "a4165ae5f7ad5ab682e2c3dd52",
            text = "Hello world"
        }),
        Encoding.UTF8,
        "application/json");

        using HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress, jsonContent);

        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"{jsonResponse}\n");
    }


    //public static async Task Run(HttpRequest req, ILogger log)
    //{
    //    using (HttpClient client = new HttpClient())
    //    {
    //        BaseAddress = new Uri()
    //        client.DefaultRequestHeaders.Add("");

    //        HttpResponseMessage response = await client.GetAsync("https://example");

    //        string responseBody = await response.Content.ReadAsStringAsync();

    //        log.LogInformation(responseBody);
    //    }
    //}
}
