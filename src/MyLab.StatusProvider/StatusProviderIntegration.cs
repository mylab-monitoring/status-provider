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
        /// Integrates <see cref="IAppStatusService"/> singleton service
        /// </summary>
        public static IServiceCollection AddStatusProviding(this IServiceCollection services)
        {
            return services.AddSingleton<IAppStatusService>(DefaultAppStatusService.Create());
        }
        
        /// <summary>
        /// Integrate status url handling
        /// </summary>
        public static void AddStatusProviding(this IApplicationBuilder app, string path = null)
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
