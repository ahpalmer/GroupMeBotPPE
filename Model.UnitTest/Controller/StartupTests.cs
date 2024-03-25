using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Moq;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using GroupMeBot.Model;
using GroupMeBot.Controller;
using System;

namespace GroupMeBot.Tests
{
    [TestClass]
    public class StartupTests
    {
        [TestMethod]
        public void Configure_RegistersExpectedServices()
        {
            // Arrange
            var services = new ServiceCollection();
            var mockBuilder = new Mock<IFunctionsHostBuilder>();
            mockBuilder.Setup(b => b.Services).Returns(services);

            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["BotPostUrl"]).Returns("https://example.com/botpost");
            mockConfiguration.Setup(c => c.GetSection("ResponseFilePaths").Get<ResponseFilePaths>()).Returns(new ResponseFilePaths());

            //var mockResponseFilePathsSection = new Mock<IConfigurationSection>();
            //mockResponseFilePathsSection.Setup(s => s.Get<ResponseFilePaths>()).Returns(new ResponseFilePaths());

            //mockConfiguration.Setup(c => c.GetSection("ResponseFilePaths")).Returns(mockResponseFilePathsSection.Object);


            var mockContext = new Mock<FunctionsHostBuilderContext>();
            mockContext.Setup(c => c.ApplicationRootPath).Returns(System.IO.Path.GetTempPath());
            mockBuilder.Setup(b => b.GetContext()).Returns(mockContext.Object);

            var startup = new Startup();

            // Act
            startup.GetType().GetMethod("Configure", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).Invoke(startup, new object[] { mockBuilder.Object });

            // Assert
            Assert.IsTrue(services.Any(service => service.ServiceType == typeof(HttpClient)));
            Assert.IsTrue(services.Any(service => service.ServiceType == typeof(IAnalysisBot)));
            Assert.IsTrue(services.Any(service => service.ServiceType == typeof(IMessageBot)));
            Assert.IsTrue(services.Any(service => service.ServiceType == typeof(IMessageIncoming)));
            Assert.IsTrue(services.Any(service => service.ServiceType == typeof(IMessageOutgoing)));
            Assert.IsTrue(services.Any(service => service.ServiceType == typeof(ResponseFilePaths)));
        }
    }
}