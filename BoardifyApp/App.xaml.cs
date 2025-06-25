// ReSharper disable AsyncVoidMethod

using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BoardifyApp;

public partial class App : Application
{
    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<MainWindow>();
                //
                // services.AddSingleton<MainViewModel>();
                // services.AddTransient<BoardViewModel>();
            })
            .Build();
    }

    private static IHost? AppHost { get; set; }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();
        var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
        startupForm.Show();
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();
        base.OnExit(e);
    }
}