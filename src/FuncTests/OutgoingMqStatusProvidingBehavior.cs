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
    public class OutgoingMqStatusProvidingBehavior : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _clientFactory;
        private readonly ITestOutputHelper _output;

        /// <summary>
        /// Initializes a new instance of <see cref="OutgoingMqStatusProvidingBehavior"/>
        /// </summary>
        public OutgoingMqStatusProvidingBehavior(WebApplicationFactory<Startup> clientFactory, ITestOutputHelper output)
        {
            _clientFactory = clientFactory;
            _output = output;
        }

        [Fact]
        public async Task ShouldApplyMsgSending()
        {
            //Arrange
            var cl = _clientFactory.CreateClient();
            var r = await cl.PostAsync("/mq/outgoing/start-send?queuename=foo", null);
            r.EnsureSuccessStatusCode();

            //Act
            var status = await GetStatus(cl);

            //Assert
            Assert.True(status.LastOutgoingMessageTime.HasValue);
            Assert.True(status.LastOutgoingMessageTime.Value.AddSeconds(1) > DateTime.Now);
            Assert.Equal("foo", status.LastOutgoingMessageQueue);
            Assert.Null(status.LastOutgoingMessageError);
        }

        [Fact]
        public async Task ShouldApplyMsgCompletion()
        {
            //Arrange
            var cl = _clientFactory.CreateClient();
            var r0 = await cl.PostAsync("/mq/outgoing/start-send?queuename=foo", null);
            r0.EnsureSuccessStatusCode();
            var r = await cl.PostAsync("/mq/outgoing/complete-sending", null);
            r.EnsureSuccessStatusCode();

            //Act
            var status = await GetStatus(cl);

            //Assert
            Assert.True(status.LastOutgoingMessageTime.HasValue);
            Assert.True(status.LastOutgoingMessageTime.Value.AddSeconds(1) > DateTime.Now);
            Assert.Equal("foo", status.LastOutgoingMessageQueue);
            Assert.Null(status.LastOutgoingMessageError);
        }

        [Fact]
        public async Task ShouldApplyMsgFail()
        {
            //Arrange
            var cl = _clientFactory.CreateClient();
            var r0 = await cl.PostAsync("/mq/outgoing/start-send?queuename=foo", null);
            r0.EnsureSuccessStatusCode();
            var r = await cl.PostAsync("/mq/outgoing/fail-sending?msg=bar", null);
            r.EnsureSuccessStatusCode();

            //Act
            var status = await GetStatus(cl);

            //Assert
            Assert.True(status.LastOutgoingMessageTime.HasValue);
            Assert.True(status.LastOutgoingMessageTime.Value.AddSeconds(1) > DateTime.Now);
            Assert.Equal("foo", status.LastOutgoingMessageQueue);
            Assert.NotNull(status.LastOutgoingMessageError);
            Assert.Equal("bar", status.LastOutgoingMessageError.Message);
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
