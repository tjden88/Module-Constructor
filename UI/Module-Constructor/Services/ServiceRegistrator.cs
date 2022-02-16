using Microsoft.Extensions.DependencyInjection;
using Module_Constructor.Services.Interfaces;

namespace Module_Constructor.Services
{
    public static class ServiceRegistrator
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => 
            services
                .AddScoped<I3DVisualizer, Visualizer>()
            
            ;
    }
}
