using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Moq;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using GroupMeBot.Model;
using GroupMeBot.Controller;
using System;
using Microsoft.AspNetCore.Hosting;

namespace GroupMeBot.Tests
{
    [TestClass]
    public class StartupTests
    {
        //[TestMethod]
        //public void Configure_RegistersExpectedServices()
        //{
        //    // Arrange
        //    var mockFunctionsHostBuilder = new Mock<IFunctionsHostBuilder>();
        //    var mockFunctionsHostBuilderContext = new Mock<FunctionsHostBuilderContext>();
        //    var mockConfigurationRoot = new Mock<IConfigurationRoot>();
        //    var configuration = new ConfigurationBuilder().Build(); // This is an empty configuration for testing purposes
        //    mockFunctionsHostBuilder.Setup(builder => builder.GetContext()).Returns(mockFunctionsHostBuilderContext.Object);
        //    mockFunctionsHostBuilderContext.Setup(context => context.Configuration).Returns(configuration);
        //    var startup = new Startup();

        //    // Act
        //    startup.Configure(mockFunctionsHostBuilder.Object);

        //    // Assert
        //    mockFunctionsHostBuilder.Verify(builder => builder.Services.AddHttpClient(), Times.Once);
        //    mockFunctionsHostBuilder.Verify(builder => builder.Services.AddSingleton<IAnalysisBot, AnalysisBot>(), Times.Once);
        //    mockFunctionsHostBuilder.Verify(builder => builder.Services.AddSingleton<IMessageBot, MessageBot>(), Times.Once);
        //    mockFunctionsHostBuilder.Verify(builder => builder.Services.AddSingleton<IMessageIncoming, MessageIncoming>(), Times.Once);
        //    mockFunctionsHostBuilder.Verify(builder => builder.Services.AddSingleton<IMessageOutgoing>(It.IsAny<Func<IServiceProvider, IMessageOutgoing>>()), Times.Once);
        //}

        [TestMethod]
        public void Startup_RegistersExpectedServices()
        {
            var webHost = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();

            Assert.IsNotNull(webHost);
            Assert.IsNotNull(webHost.Services.GetRequiredService<IAnalysisBot>());
            Assert.IsNotNull(webHost.Services.GetRequiredService<IMessageBot>());
            Assert.IsNotNull(webHost.Services.GetRequiredService<IMessageIncoming>());
            Assert.IsNotNull(webHost.Services.GetRequiredService<IMessageOutgoing>());
        }
    }
}