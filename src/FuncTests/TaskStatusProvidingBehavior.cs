//using System;
//using System.Net.Http;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc.Testing;
//using MyLab.StatusProvider;
//using Newtonsoft.Json;
//using TestServer;
//using Xunit;
//using Xunit.Abstractions;
//using TaskStatus = MyLab.StatusProvider.TaskStatus;

//namespace FuncTests
//{
//    public class TaskStatusProvidingBehavior : IClassFixture<WebApplicationFactory<Startup>>
//    {
//        private readonly WebApplicationFactory<Startup> _clientFactory;
//        private readonly ITestOutputHelper _output;

//        /// <summary>
//        /// Initializes a new instance of <see cref="TaskStatusProvidingBehavior"/>
//        /// </summary>
//        public TaskStatusProvidingBehavior(WebApplicationFactory<Startup> clientFactory, ITestOutputHelper output)
//        {
//            _clientFactory = clientFactory;
//            _output = output;
//        }

//        [Fact]
//        public async Task ShouldApplyLogicStart()
//        {
//            //Arrange
//            var cl = _clientFactory.CreateClient();
//            var resp = await cl.PostAsync("/task/start", null);
//            resp.EnsureSuccessStatusCode();

//            //Act
//            var status = await GetStatus(cl);

//            //Assert
//            Assert.NotNull(status.LastTimeStart);
//            Assert.True(status.LastTimeStart.Value.AddSeconds(1) > DateTime.Now);
//            Assert.Null(status.LastTimeDuration);
//            Assert.Null(status.LastTimeError);
//            Assert.True(status.Processing);
//        }

//        [Fact]
//        public async Task ShouldApplyLogicCompletion()
//        {
//            //Arrange
//            var cl = _clientFactory.CreateClient();
//            var resp1 = await cl.PostAsync("/task/start", null);
//            resp1.EnsureSuccessStatusCode();
//            var resp2 = await cl.PostAsync("/task/complete", null);
//            resp2.EnsureSuccessStatusCode();

//            //Act
//            var status = await GetStatus(cl);

//            //Assert
//            Assert.NotNull(status.LastTimeDuration);
//            Assert.False(status.Processing);
//        }

//        [Fact]
//        public async Task ShouldApplyLogicError()
//        {
//            //Arrange
//            var cl = _clientFactory.CreateClient();

//            var resp1 = await cl.PostAsync("/task/start", null);
//            resp1.EnsureSuccessStatusCode();
//            var resp2 = await cl.PostAsync("/task/error?msg=foo",  null);
//            resp2.EnsureSuccessStatusCode();


//            //Act
//            var status = await GetStatus(cl);

//            //Assert
//            Assert.NotNull(status.LastTimeError);
//            Assert.Equal("foo", status.LastTimeError.Message);
//            Assert.False(status.Processing);
//        }

//        private async Task<TaskStatus> GetStatus(HttpClient client)
//        {
//            var resp = await client.GetAsync("/status");
//            resp.EnsureSuccessStatusCode();

//            var restStr = await resp.Content.ReadAsStringAsync();

//            _output.WriteLine(restStr);

//            return JsonConvert.DeserializeObject<ApplicationStatus>(restStr).Task;
//        }
//    }
//}
