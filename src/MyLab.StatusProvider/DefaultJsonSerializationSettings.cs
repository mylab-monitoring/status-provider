using Newtonsoft.Json;

namespace MyLab.StatusProvider
{
    static class DefaultJsonSerializationSettings
    {
        public static JsonSerializerSettings Create()
        {
            return new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.None
            };
        }
    }
}