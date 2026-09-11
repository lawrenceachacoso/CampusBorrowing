using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CampusBorrowing.Application.Interfaces;
using CampusBorrowing.Application.Services;
using CampusBorrowing.Desktop.ViewModels;
using CampusBorrowing.Desktop.Views;
using CampusBorrowing.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CampusBorrowing.Desktop;

public partial class App : Avalonia.Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        Services = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = Services.GetRequiredService<MainViewModel>(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        // Repositories - Singleton so state (borrowings, availability) persists across the app's lifetime
        services.AddSingleton<IStudentRepository, InMemoryStudentRepository>();
        services.AddSingleton<IEquipmentRepository, InMemoryEquipmentRepository>();
        services.AddSingleton<IBorrowingRepository, InMemoryBorrowingRepository>();

        // Application services
        services.AddTransient<BorrowEquipmentService>();
        services.AddTransient<ReturnEquipmentService>();

        // ViewModels
        services.AddTransient<MainViewModel>();
    }
}