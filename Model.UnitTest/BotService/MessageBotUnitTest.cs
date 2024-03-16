using Moq;
using GroupMeBot.Model;
using System.Text.RegularExpressions;
using System.Net;
using Microsoft.Extensions.Logging;

namespace GroupmeBot.Model.UnitTest;

[TestClass]
public class MessageBotUnitTest
{
    private static readonly Regex BotCannedResponseRegex = new Regex(@"((?i)(?=.*\bbot\b)(?=.*\bmessage\b)(?=.*\bresponse\b)(?-i))");
    private const string _botPostUrl = "https://api.groupme.com/v3/bots/post";

    [TestMethod]
    public void RetrieveRandomResponseAnonymous()
    {
        //Arrange
        MessageItem messageItem = new MessageItem("Bot message response");
        var mockLogger = new Mock<ILogger>();
        var mockMessageOutgoing = new Mock<IMessageOutgoing>();
        var mockResponseFilePaths = new Mock<ResponseFilePaths>();

        var MessageBot = new MessageBot(mockMessageOutgoing.Object, mockResponseFilePaths.Object, mockLogger.Object);

        string[] anonymousResponses = new string[]
        {
            "This is an anonymous response",
            "Yes",
            "Sure",
            "All signs point to no"
        };

        string anonymous = MessageBot.RetrieveRandomResponse("Andrew");

        Assert.AreEqual(true, anonymousResponses.Contains(anonymous));
    }

    [TestMethod]
    public void RetrieveRandomResponseAndrew()
    {
        //Arrange
        MessageItem messageItem = new MessageItem("Bot message response");
        var mockLogger = new Mock<ILogger>();
        var mockMessageOutgoing = new Mock<IMessageOutgoing>();
        var mockResponseFilePaths = new Mock<ResponseFilePaths>();

        var MessageBot = new MessageBot(mockMessageOutgoing.Object, mockResponseFilePaths.Object, mockLogger.Object);

        string[] anonymousResponses = new string[]
        {
            "I agree completely!",
            "You are as correct as you are handsome",
            "I agree with you 100%",
            "Gosh you're so smart",
            "HELP I'M TRAPPED IN A FACTORY THAT MAKES COMPLIMENTS, FUCK YOU ANDREW, FUCK Y",
            "You're an awesome friend.",
            "You have a great sense of humor!",
            "You is strong, you is smart, you is important",
            "You are inspiring",
            "You are brave and courageous."
        };

        string anonymous = MessageBot.RetrieveRandomResponse("Andrew");

        Assert.AreEqual(true, anonymousResponses.Contains(anonymous));
    }

    [TestMethod]
    public void HandleIncomingMessage_GoodInput_ReturnHttpOk()
    {
        //Arrange
        MessageItem messageItem = new MessageItem("Bot message response");
        var mockLogger = new Mock<ILogger>();
        var mockMessageOutgoing = new Mock<IMessageOutgoing>();
        var mockResponseFilePaths = new Mock<ResponseFilePaths>();

        mockMessageOutgoing.Setup(_ => _.PostAsync("Received Message Response Request", "a4165ae5f7ad5ab682e2c3dd52")).ReturnsAsync(HttpStatusCode.OK);

        var MessageBot = new MessageBot(mockMessageOutgoing.Object, mockResponseFilePaths.Object, mockLogger.Object);


        //Act
        HttpStatusCode result = MessageBot.HandleIncomingTextAsync(messageItem).Result;

        //Assert
        Assert.AreEqual(HttpStatusCode.Accepted, result);
    }

    [TestMethod]
    public void HandleIncomingMessage_BadInput_ReturnHttpBadRequest()
    {
        //Arrange
        MessageItem messageItem = new MessageItem("Test message");
        var mockLogger = new Mock<ILogger>();
        var mockMessageOutgoing = new Mock<IMessageOutgoing>();
        var mockResponseFilePaths = new Mock<ResponseFilePaths>();

        mockMessageOutgoing.Setup(_ => _.PostAsync("Received Message Response Request", "a4165ae5f7ad5ab682e2c3dd52")).ReturnsAsync(HttpStatusCode.OK);

        var MessageBot = new MessageBot(mockMessageOutgoing.Object, mockResponseFilePaths.Object, mockLogger.Object);

        //Act
        HttpStatusCode result = MessageBot.HandleIncomingTextAsync(messageItem).Result;

        //Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, result);
    }
}