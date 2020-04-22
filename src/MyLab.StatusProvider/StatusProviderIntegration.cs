using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
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
        public static IServiceCollection AddAppStatusProviding(this IServiceCollection services)
        {
            return services.AddSingleton<IAppStatusService>(DefaultAppStatusService.Create());
        }
        
        /// <summary>
        /// Integrate status url handling
        /// </summary>
        public static void UseStatusApi(this IApplicationBuilder app, string path = "/status", JsonSerializerSettings serializerSettings = null)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            var urlHandler = new StatusProviderUrlHandler(serializerSettings);

            app.MapWhen(ctx =>
                    ctx.Request.Path == (path) &&
                    ctx.Request.Method == "GET",
                appB =>
            {
                appB.Run(async context => await urlHandler.Handle(app, context));
            });
        }
    }
}
