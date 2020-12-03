using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using MyLab.StatusProvider;
using MyLab.StatusProvider.Config;
using MyLab.StatusProvider.Log;
using Newtonsoft.Json;
using TestServer;
using Xunit;
using Xunit.Abstractions;

namespace FuncTests
{
    public class AppStatusProvidingBehaviour  : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _clientFactory;
        private readonly ITestOutputHelper _output;

        /// <summary>
        /// Initializes a new instance of <see cref="AppStatusProvidingBehaviour"/>
        /// </summary>
        public AppStatusProvidingBehaviour(WebApplicationFactory<Startup> clientFactory, ITestOutputHelper output)
        {
            _clientFactory = clientFactory;
            _output = output;

            Environment.SetEnvironmentVariable("APP_VERSION", "1.2.3");
        }

        [Theory]
        [InlineData("/status", true)]
        [InlineData("/non-status", false)]
        public async Task ShouldProvideStatus(string path, bool found)
        {
            //Arrange
            var client = _clientFactory.CreateClient();

            //Act
            var resp = await client.GetAsync(path);
            var restStr = await resp.Content.ReadAsStringAsync();

            _output.WriteLine(restStr);

            var res = JsonConvert.DeserializeObject<ApplicationStatus>(restStr);

            //Assert
            Assert.Equal(found ? HttpStatusCode.OK : HttpStatusCode.NotFound, resp.StatusCode);
            if(found)
                Assert.NotNull(res);
        }

        [Fact]
        public async Task ShouldProvideConfig()
        {
            //Arrange
            var client = _clientFactory.CreateClient();

            //Act
            var resp = await client.GetAsync("/status/config");
            var restStr = await resp.Content.ReadAsStringAsync();

            _output.WriteLine(restStr);

            //var res = JsonConvert.DeserializeObject<ConfigurationModel>(restStr);

            //Assert
            Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
            Assert.NotNull(restStr);
        }

        [Fact]
        public async Task ShouldProvideLog()
        {
            //Arrange
            var client = _clientFactory.CreateClient();

            //Act
            var resp = await client.GetAsync("/status/log");
            var restStr = await resp.Content.ReadAsStringAsync();

            _output.WriteLine(restStr);

            //var res = JsonConvert.DeserializeObject<LogEntity[]>(restStr);

            //Assert
            Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
            Assert.NotNull(restStr);
        }

        [Fact]
        public async Task ShouldProvideName()
        {
            //Arrange
            

            //Act
            var status = await GetStatus();

            //Assert
            Assert.Equal("ReSharperTestRunner64", status.Name);
        }

        [Fact]
        public async Task ShouldProvideVersion()
        {
            //Arrange


            //Act
            var status = await GetStatus();

            //Assert
            Assert.Equal("1.2.3", status.Version);
        }

        [Fact]
        public async Task ShouldProvideStartTime()
        {
            //Arrange


            //Act
            var status = await GetStatus();

            //Assert
            Assert.True(status.StartAt.AddSeconds(10) > DateTime.Now);
        }

        [Fact]
        public async Task ShouldProvideServerTime()
        {
            //Arrange


            //Act
            var status = await GetStatus();

            //Assert
            Assert.True(status.ServerTime.AddSeconds(1) > DateTime.Now);
        }

        [Fact]
        public async Task ShouldProvideUpTime()
        {
            //Arrange


            //Act
            var status = await GetStatus();

            //Assert
            Assert.True(status.UpTime.Add(TimeSpan.FromSeconds(1)) > DateTime.Now - status.StartAt);
        }

        private async Task<ApplicationStatus> GetStatus()
        {
            var client = _clientFactory.CreateClient();

            var resp = await client.GetAsync("/status");
            var restStr = await resp.Content.ReadAsStringAsync();

            _output.WriteLine(restStr);

            return JsonConvert.DeserializeObject<ApplicationStatus>(restStr);
        }
    }
}
