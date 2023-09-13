//namespace GroupMeUtilities.Service;

//using GroupMeUtilities.Model;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Text.Json;
//using System.Text.Json.Serialization;
//using System.Threading.Tasks;


//public class BotPoster : IBotPoster
//{
//    private string botPostUrl;

//    public BotPoster(string BotPostUrl)
//    {
//        botPostUrl = BotPostUrl;
//    }

//    /// <inheritdoc/>
//    public async Task<HttpStatusCode> PostAsync(string text, string botId)
//    {
//        var post = new CreateBotPostRequest
//        {
//            BotId = botId,
//            Text = text
//        };
//        return await PostBotMessage(post);
//    }

//    /// <inheritdoc/>
//    private async Task<HttpStatusCode> PostBotMessage(CreateBotPostRequest request)
//    {
//        if (request == null)
//        {
//            throw new ArgumentNullException(nameof(request));
//        }

//        //TODO: this is where you left off on 10 May 2023
//        //Understand this before you finalize it in your code.
//        //Here is the explanation: You use the using () method as a form of garbage collection.
//        //The using() method is actually shorthand for a try{} finally{garbage collection} block
//        //You must use the using() method for any class that is based on the IDisposable base class.
//        //StringContent is based on a class that is based on a class that is based on IDisposable.
//        //Therefore, to use StringContent you must utilize the using() method
//        using (StringContent content = JsonSerializer.SerializeToJson(request))
//        {
//            var client = new HttpClient();
//            HttpResponseMessage result = await client.PostAsync(_botPostUrl, content);
//            return result != null ? result.StatusCode : HttpStatusCode.ServiceUnavailable;
//        }

//    }
//}

