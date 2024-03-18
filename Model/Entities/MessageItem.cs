using System.Runtime.Serialization;

namespace GroupMeBot.Model;

/// <inheritdoc/>
[DataContract]
public class MessageItem : IMessageItem
{
    public MessageItem() 
    {
    }

    public MessageItem(string text)
    {
        Text = text;
    }

    /// <inheritdoc/>
    [DataMember(Name = "serverId", EmitDefaultValue = false)]
    public string? MessageId { get; set; }

    /// <inheritdoc/>
    [DataMember(Name = "groupId", EmitDefaultValue = false)]
    public string? GroupId { get; set; }

    /// <inheritdoc/>
    [DataMember(Name = "userId", EmitDefaultValue = false)]
    public string? UserId { get; set; }

    /// <inheritdoc/>
    [DataMember(Name = "displayName", EmitDefaultValue = false)]
    public string? DisplayName { get; set; }

    /// <inheritdoc/>
    [DataMember(Name = "text", EmitDefaultValue = false)]
    public string? Text { get; set; }

    /// <inheritdoc/>
    [DataMember(Name = "favorited_by", EmitDefaultValue = false)]
    public string[]? LikedBy { get; set; }

    /// <inheritdoc/>
    [DataMember(Name = "system", EmitDefaultValue = false)]
    public bool? IsSystem { get; set; }

    /// <inheritdoc/>
    [DataMember(Name = "sender_id", EmitDefaultValue = false)]
    public string? SystemSenderId { get; set; }

    /// <inheritdoc/>
    [DataMember(Name = "sender_type", EmitDefaultValue = false)]
    public string? SystemSenderType { get; set; }
}