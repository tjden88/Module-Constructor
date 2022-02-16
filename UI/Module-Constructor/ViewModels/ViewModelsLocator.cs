

using System;
using Microsoft.Extensions.DependencyInjection;
using Module_Constructor.ViewModels.Windows;

namespace Module_Constructor.ViewModels
{
    public class ViewModelsLocator
    {
        public MainWindowViewModel MainWindowViewModel => App.Services.GetRequiredService<MainWindowViewModel>();
    }

    public static class ViewModelsRegistrator
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services) => 
            services
                .AddSingleton<MainWindowViewModel>()
            ;
    }
}
