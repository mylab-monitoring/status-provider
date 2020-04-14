using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using MyLab.StatusProvider;
using Newtonsoft.Json;
using TestServer;
using Xunit;
using Xunit.Abstractions;
using TaskStatus = MyLab.StatusProvider.TaskStatus;

namespace FuncTests
{
    public class IncomingMqStatusProvidingBehavior : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _clientFactory;
        private readonly ITestOutputHelper _output;

        /// <summary>
        /// Initializes a new instance of <see cref="IncomingMqStatusProvidingBehavior"/>
        /// </summary>
        public IncomingMqStatusProvidingBehavior(WebApplicationFactory<Startup> clientFactory, ITestOutputHelper output)
        {
            _clientFactory = clientFactory;
            _output = output;
        }

        [Fact]
        public async Task ShouldApplyQueueConnection()
        {
            //Arrange
            var cl = _clientFactory.CreateClient();
            var r = await cl.PostAsync("/mq/incoming/connect?queuename=foo", null);
            r.EnsureSuccessStatusCode();

            //Act
            var status = await GetStatus(cl);

            //Assert
            Assert.Contains(status.Queues, s => s == "foo");
        }

        [Fact]
        public async Task ShouldApplyIncomingMsg()
        {
            //Arrange
            var cl = _clientFactory.CreateClient();
            var r = await cl.PostAsync("/mq/incoming/send-msg?queuename=foo", null);
            r.EnsureSuccessStatusCode();

            //Act
            var status = await GetStatus(cl);

            //Assert
            Assert.Equal("foo", status.LastIncomingMessageQueue);
            Assert.True(status.LastIncomingMessageTime.HasValue);
            Assert.True(status.LastIncomingMessageTime.GetValueOrDefault().AddSeconds(1) > DateTime.Now);
            Assert.Null(status.LastIncomingMessageError);
        }

        [Fact]
        public async Task ShouldApplyIncomingMsgCompletion()
        {
            //Arrange
            var cl = _clientFactory.CreateClient();
            var r0 = await cl.PostAsync("/mq/incoming/send-msg?queuename=foo", null);
            r0.EnsureSuccessStatusCode();
            var r = await cl.PostAsync("/mq/incoming/complete-msg", null);
            r.EnsureSuccessStatusCode();

            //Act
            var status = await GetStatus(cl);

            //Assert
            Assert.Equal("foo", status.LastIncomingMessageQueue);
            Assert.True(status.LastIncomingMessageTime.HasValue);
            Assert.True(status.LastIncomingMessageTime.GetValueOrDefault().AddSeconds(1) > DateTime.Now);
            Assert.Null(status.LastIncomingMessageError);
        }

        [Fact]
        public async Task ShouldApplyIncomingMsgFail()
        {
            //Arrange
            var cl = _clientFactory.CreateClient();

            var r0 = await cl.PostAsync("/mq/incoming/send-msg?queuename=foo", null);
            r0.EnsureSuccessStatusCode();
            var r = await cl.PostAsync("/mq/incoming/fail-msg?msg=bar", null);
            r.EnsureSuccessStatusCode();

            //Act
            var status = await GetStatus(cl);

            //Assert
            Assert.Equal("foo", status.LastIncomingMessageQueue);
            Assert.True(status.LastIncomingMessageTime.HasValue);
            Assert.True(status.LastIncomingMessageTime.GetValueOrDefault().AddSeconds(1) > DateTime.Now);
            Assert.NotNull(status.LastIncomingMessageError);
            Assert.Equal("bar", status.LastIncomingMessageError.Message);
        }

        private async Task<QueueConsumerStatus> GetStatus(HttpClient client)
        {
            var resp = await client.GetAsync("/status");
            resp.EnsureSuccessStatusCode();

            var restStr = await resp.Content.ReadAsStringAsync();

            _output.WriteLine(restStr);

            return JsonConvert.DeserializeObject<ApplicationStatus>(restStr).Mq;
        }
    }
}
