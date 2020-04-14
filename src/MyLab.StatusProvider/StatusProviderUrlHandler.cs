using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace MyLab.StatusProvider
{
    internal class StatusProviderUrlHandler
    {
        public static async Task Handle(IApplicationBuilder app, HttpContext context)
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
                var statusTxt = JsonConvert.SerializeObject(status, new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore
                });

                context.Response.StatusCode = 200;
                context.Response.Headers.Append("Content-Type", "application/json");
                await context.Response.WriteAsync(statusTxt);
            }
        }
    }
}