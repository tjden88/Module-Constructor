using System;
using Microsoft.Extensions.DependencyInjection;
using Module_Constructor.Services;
using Module_Constructor.ViewModels;

namespace Module_Constructor;

public partial class App 
{

    private static IServiceProvider _Services;

    /// <summary> Коллекция сервисов приложения </summary>
    public static IServiceProvider Services => _Services ??= ConfigureServices();


    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddLogging();

        services.AddViewModels();
        services.AddServices();

        return services.BuildServiceProvider();
    }
}