using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MyLab.StatusProvider.Config
{
    /// <summary>
    /// Contains configuration node
    /// </summary>
    [JsonConverter(typeof(ConfigurationModelConverter))]
    public class ConfigurationModel : Dictionary<string, ConfigurationModel>
    {
        /// <summary>
        /// Configuration node value
        /// </summary>
        [JsonProperty]
        public string Value { get; set; }
        /// <summary>
        /// Configuration node provider
        /// </summary>
        [JsonProperty]
        public string Provider { get; set; }

        /// <summary>
        /// Creates model from configuration
        /// </summary>
        public static ConfigurationModel Create(IConfigurationRoot root)
        {
            void RecurseChildren(
                IDictionary<string, ConfigurationModel> container,
                IEnumerable<IConfigurationSection> children)
            {
                foreach (IConfigurationSection child in children)
                {
                    var childModel = new ConfigurationModel();

                    (string Value, IConfigurationProvider Provider) valueAndProvider = GetValueAndProvider(root, child.Path);

                    childModel.Provider = valueAndProvider.Provider?.ToString();
                    childModel.Value = valueAndProvider.Value;

                    RecurseChildren(childModel, child.GetChildren());

                    container.Add(child.Key, childModel);
                }
            }

            var rootConfigModel = new ConfigurationModel();

            if(root != null)
                RecurseChildren(rootConfigModel, root.GetChildren());

            return rootConfigModel;
        }

        private static (string Value, IConfigurationProvider Provider) GetValueAndProvider(
            IConfigurationRoot root,
            string key)
        {
            foreach (IConfigurationProvider provider in root.Providers.Reverse())
            {
                if (provider.TryGet(key, out string value))
                {
                    return (value, provider);
                }
            }

            return (null, null);
        }
    }
}
