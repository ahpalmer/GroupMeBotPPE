namespace GroupMeUtilities.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

/// <summary>
/// A generic message item from the server - support for group and direct messages
/// </summary>
[DataContract]
public class MessageItem
{
    /// <summary>
    /// Gets or sets the the server ID for this message
    /// </summary>
    [DataMember(Name = "serverId", EmitDefaultValue = false)]
    public string MessageId { get; set; }

    /// <summary>
    /// Gets or sets the source GUID for the message
    /// </summary>
    [DataMember(Name = "sourceGuid", EmitDefaultValue = false)]
    public string SourceGuid { get; set; }

    /// <summary>
    /// Gets or sets the time (since epoch) at which this message was created
    /// </summary>
    [DataMember(Name = "createdAt", EmitDefaultValue = false)]
    public long CreatedAtEpoch { get; set; }

    /// <summary>
    /// Gets or sets the ID of the group to which this message belongs (group only)
    /// </summary>
    [DataMember(Name = "groupId", EmitDefaultValue = false)]
    public string GroupId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user that sent the message
    /// </summary>
    [DataMember(Name = "userId", EmitDefaultValue = false)]
    public string UserId { get; set; }

    /// <summary>
    /// Gets or sets the display name of the user who sent this message
    /// </summary>
    [DataMember(Name = "displayName", EmitDefaultValue = false)]
    public string DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the Text of the message
    /// </summary>
    [DataMember(Name = "text", EmitDefaultValue = false)]
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets the list of users that have liked this message
    /// </summary>
    [DataMember(Name = "favorited_by", EmitDefaultValue = false)]
    public string[] LikedBy { get; set; }

    //Todo: this requires a separate attachment class:
    ///// <summary>
    ///// Gets or sets the list of attachments for this message
    ///// </summary>
    //[DataMember(Name = "attachments")]
    //public Attachment[] Attachments { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the message was sent by the system or not
    /// </summary>
    [DataMember(Name = "system", EmitDefaultValue = false)]
    public bool IsSystem { get; set; }

    /// <summary>
    /// Gets or sets the ID of the system that sent the message
    /// </summary>
    [DataMember(Name = "sender_id", EmitDefaultValue = false)]
    public string SystemSenderId { get; set; }

    /// <summary>
    /// Gets or sets the type of system that sent the message
    /// </summary>
    [DataMember(Name = "sender_type", EmitDefaultValue = false)]
    public string SystemSenderType { get; set; }
}