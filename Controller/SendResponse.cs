using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GroupMeBot.Controller;

// Todo: Useless code right now.  Delete after you get the new send code up and running.
public class SendResponse
{
    private const string _botId = "a4165ae5f7ad5ab682e2c3dd52";

    public static HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("https://api.groupme.com/v3/bots/post"),
    };

    // Todo: this method is never referenced
    public static async Task PostAsync(HttpClient httpClient)
    {
        using StringContent jsonContent = new(
        JsonSerializer.Serialize(new
        {
            bot_id = _botId,
            text = "Hello world"
        }),
        Encoding.UTF8,
        "application/json");

        using HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress, jsonContent);

        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"{jsonResponse}\n");
    }

    // Todo: Complete this feature or delete it.
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
