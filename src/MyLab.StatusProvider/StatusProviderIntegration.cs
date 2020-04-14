using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

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
        public static IServiceCollection AddApiStatusProviding(this IServiceCollection services)
        {
            return services.AddSingleton<IAppStatusService>(DefaultAppStatusService.Create());
        }

        /// <summary>
        /// Integrates <see cref="IAppStatusService"/> singleton service for Task-application
        /// </summary>
        public static IServiceCollection AddTaskStatusProviding(this IServiceCollection services)
        {
            return services.AddSingleton<IAppStatusService>(DefaultAppStatusService.CreateForTask());
        }

        /// <summary>
        /// Integrates <see cref="IAppStatusService"/> singleton service for MQ consumer application
        /// </summary>
        public static IServiceCollection AddMqConsumerStatusProviding(this IServiceCollection services)
        {
            return services.AddSingleton<IAppStatusService>(DefaultAppStatusService.CreateForMqConsumer());
        }

        /// <summary>
        /// Integrate status url handling
        /// </summary>
        public static void AddStatusApi(this IApplicationBuilder app, string path = null)
        {
            app.MapWhen(ctx =>
                    ctx.Request.Path == (path ?? "/status") &&
                    ctx.Request.Method == "GET",
                appB =>
            {
                appB.Run(async context => await StatusProviderUrlHandler.Handle(app, context));
            });
        }
    }
}
