using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MyLab.StatusProvider;
using MyLab.StatusProvider.Config;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests
{
    public class ConfigurationModelBehavior
    {
        private readonly ITestOutputHelper _output;

        public ConfigurationModelBehavior(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void ShouldDeserialize()
        {
            //Arrange
            var initialConf = new Dictionary<string,string>
            {
                {"Key1:Key11:Key111", "foo"},
                {"Key1:Key12:Key121", "bar"},
                {"Key2:Key21", "baz"}
            };

            var conf = new ConfigurationBuilder()
                .AddInMemoryCollection(initialConf)
                .Build();

            //Act
            var model = ConfigurationModel.Create(conf);

            _output.WriteLine(JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings(){ NullValueHandling = NullValueHandling.Include}));

            //Assert
            Assert.NotNull(model);
            Assert.Equal("foo", model["Key1"]["Key11"]["Key111"].Value);
            Assert.Equal("bar", model["Key1"]["Key12"]["Key121"].Value);
            Assert.Equal("baz", model["Key2"]["Key21"].Value);
        }
    }
}
