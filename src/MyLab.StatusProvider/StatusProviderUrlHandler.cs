using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace MyLab.StatusProvider
{
    class StatusProviderUrlHandler
    {
        private readonly JsonSerializerSettings _serializerSettings;

        /// <summary>
        /// Initializes a new instance of <see cref="StatusProviderUrlHandler"/>
        /// </summary>
        public StatusProviderUrlHandler(JsonSerializerSettings serializerSettings)
        {
            _serializerSettings = serializerSettings ?? DefaultJsonSerializationSettings.Create();
        }

        public async Task Handle(IApplicationBuilder app, HttpContext context)
        {
            var statusService = (IAppStatusService)app.ApplicationServices.GetService(typeof(IAppStatusService));
            if (statusService == null)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("No status found");
            }
            else
            {
                var status = statusService.GetStatus();
                var statusTxt = JsonConvert.SerializeObject(status, _serializerSettings);

                context.Response.StatusCode = 200;
                context.Response.Headers.Append("Content-Type", "application/json");
                await context.Response.WriteAsync(statusTxt);
            }
        }
    }
}