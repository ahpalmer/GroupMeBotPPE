using System;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;

public class PostResponse
{
	public PostResponse()
	{
	}

    public class CreateBotPostRequest
    {
        /// <summary>
        /// Gets or sets the ID of the bot that is sending the message
        /// </summary>
        [JsonPropertyName("BotId")]
        public string BotId { get; set; }

        /// <summary>
        /// Gets or sets the text of the message
        /// </summary>
        [JsonPropertyName("Text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the attachments for the message
        /// </summary>
        [JsonPropertyName("Attachments")]
        public Attachment[] Attachments { get; set; }
    }
}
