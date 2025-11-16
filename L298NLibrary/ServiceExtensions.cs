    
    using Microsoft.Extensions.DependencyInjection;
    namespace L298NLibrary;


    public static class ServiceExtensions
    {
        public static IServiceCollection AddL298NLibrary(this IServiceCollection services)
        { 
            services.AddSingleton<IMotorController,L298NMotorProcessor>();
         return services;
        }   
    }
