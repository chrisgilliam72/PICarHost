    using Microsoft.Extensions.DependencyInjection;
    namespace BatteryLevelReporting;


    public static class ServiceExtensions
    {
        public static IServiceCollection AddBatteryServiceReporting(this IServiceCollection services)
        {
            services.AddSingleton<IBatteryLevelReporting, BatteryLevelReporting>();    
            return services;

        }   
    }