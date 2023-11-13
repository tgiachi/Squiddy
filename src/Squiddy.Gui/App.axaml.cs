using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Splat.Microsoft.Extensions.DependencyInjection;
using Squiddy.Core.Interfaces.Bootstrap;
using Squiddy.Gui.Bootstrap;
using Squiddy.Gui.ViewModels;
using Squiddy.Gui.Views;

namespace Squiddy.Gui;

public partial class App : Application
{
    public IServiceProvider Container { get; private set; }
    public IHost host { get; set; }

    private ISquiddyBootstrap _bootstrap = new SquiddyBootstrap(new LoggerConfiguration());

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        host = _bootstrap.BuildHostAsync(Environment.GetCommandLineArgs()).Result;

        Container = host.Services;
        Container.UseMicrosoftDependencyResolver();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainWindow = Container.GetRequiredService<MainWindow>();
            var mainWindowViewModel = Container.GetRequiredService<MainWindowViewModel>();
            mainWindow.DataContext = mainWindowViewModel;
            mainWindowViewModel.View = mainWindow;
            mainWindowViewModel.Initialize();

            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }
}
