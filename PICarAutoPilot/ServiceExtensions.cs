
using Microsoft.Extensions.DependencyInjection;
using L298NLibrary;
using Ultraborg.Library;

namespace PICarAutoPilotLib;

    public static class ServiceExtensions
    {
        public static IServiceCollection AddPICarAutoPilotLibrary(this IServiceCollection services)
        {
            services.AddSingleton<IPICarAutoPilot, PICarAutoPilot>();    
            services.AddL298NLibrary();    
            services.AddUltraBorgLibrary();    
            return services;
        }   
    }