using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MyLab.StatusProvider.Config;
using MyLab.StatusProvider.Log;
using Newtonsoft.Json;

namespace MyLab.StatusProvider
{
    class StatusProviderUrlHandler
    {
        private readonly StatusRequestDetector _detector;
        private readonly JsonSerializerSettings _serializerSettings;

        /// <summary>
        /// Initializes a new instance of <see cref="StatusProviderUrlHandler"/>
        /// </summary>
        public StatusProviderUrlHandler(StatusRequestDetector detector, JsonSerializerSettings serializerSettings)
        {
            _detector = detector;
            _serializerSettings = serializerSettings ?? DefaultJsonSerializationSettings.Create();
        }

        public async Task Handle(IApplicationBuilder app, HttpContext context)
        {
            var path = _detector.DetectAndGetRelatedPath(context.Request);

            object statusObj;

            switch (path)
            {
                case "log":
                case "/log":
                    statusObj = ReturnLog(app);
                    break;
                case "log-ext":
                case "/log-ext":
                    statusObj = ReturnLogExt(app);
                    break;
                case "config":
                case "/config":
                    statusObj = ReturnConfig(app);
                    break;
                case "":
                case "/":
                case null:
                    statusObj = ReturnStatus(app);
                    break;
                default:
                    statusObj = null;
                    break;
            }

            if (statusObj == null)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("No status found");
            }
            else
            {
                string statusTxt = JsonConvert.SerializeObject(statusObj, _serializerSettings);
                context.Response.StatusCode = 200;
                context.Response.Headers.Append("Content-Type", "application/json");
                await context.Response.WriteAsync(statusTxt);
            }
        }

        private object ReturnLogExt(IApplicationBuilder app)
        {
            var logHolder = (StatusProviderLogHolder)app.ApplicationServices.GetService(typeof(StatusProviderLogHolder));

            return logHolder?.GetLogs().ToArray();
        }

        private object ReturnLog(IApplicationBuilder app)
        {
            var logHolder = (StatusProviderLogHolder)app.ApplicationServices.GetService(typeof(StatusProviderLogHolder));

            return logHolder?.GetLogs().Select(l => $"[{l.DateTime:T}]({l.LogLevel.First()}) {l.Formatted}").ToArray();
        }

        object ReturnStatus(IApplicationBuilder app)
        {
            var statusService = (IAppStatusService)app.ApplicationServices.GetService(typeof(IAppStatusService));
            if (statusService == null)
                return null;

            var status = statusService.GetStatus();
            return new ApplicationStatus(status);
        }

        object ReturnConfig(IApplicationBuilder app)
        {
            var config = app.ApplicationServices.GetService(typeof(IConfiguration)) as IConfigurationRoot;

            if (config == null)
                return null;

            return ConfigurationModel.Create(config);
        }
    }
}