using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyLab.StatusProvider.Log;
using Newtonsoft.Json;

namespace MyLab.StatusProvider
{
    /// <summary>
    /// Extension methods for abilities integration
    /// </summary>
    public static class StatusProviderIntegration
    {
        /// <summary>
        /// Integrates <see cref="IAppStatusService"/> singleton service for API
        /// </summary>
        public static IServiceCollection AddAppStatusProviding(this IServiceCollection services, IConfigurationRoot configuration = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var logHolder = new StatusProviderLogHolder();

            return services
                .AddSingleton<IAppStatusService>(DefaultAppStatusService.Create(configuration))
                .AddLogging(logBuilder => logBuilder.AddProvider(new StatusProviderLoggerProvider(logHolder)))
                .AddSingleton(logHolder);
        }
        
        /// <summary>
        /// Integrate status url handling
        /// </summary>
        public static void UseStatusApi(this IApplicationBuilder app, string path = "/status", JsonSerializerSettings serializerSettings = null)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            var detector = new StatusRequestDetector(path);
            var urlHandler = new StatusProviderUrlHandler(detector, serializerSettings);
            
            app.MapWhen(ctx =>
                    detector.DetectAndGetRelatedPath(ctx.Request) != null,
                appB =>
            {
                appB.Run(async context => await urlHandler.Handle(app, context));
            });
        }
    }
}
