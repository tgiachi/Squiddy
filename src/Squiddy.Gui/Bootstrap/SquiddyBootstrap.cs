using System.Reactive.Concurrency;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReactiveUI;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.SpectreConsole;
using Squiddy.Core.Attributes.Services;
using Squiddy.Core.Data.Configs;
using Squiddy.Core.Data.Directories;
using Squiddy.Core.Interfaces.Bootstrap;
using Squiddy.Core.Interfaces.Services;
using Squiddy.Core.MethodEx.Services;
using Squiddy.Core.MethodEx.Utils;
using Squiddy.Core.Services.Interfaces;
using Squiddy.Gui.Impl.Services;
using Squiddy.Gui.Schedulers;
using Squiddy.Gui.Windows;
using Squiddy.Gui.Windows.ViewModels;
using Squiddy.Gui.Windows.Views;
using Squiddy.Ui.Core.MethodEx;
using Terminal.Gui;
using ILogger = Serilog.ILogger;

namespace Squiddy.Gui.Bootstrap;

public class SquiddyBootstrap : ISquiddyBootstrap
{
    private ILogger _logger;

    private Func<IServiceCollection, IServiceCollection> _servicesFunc = services => services;
    private LoggerConfiguration _loggerConfiguration;
    private DirectoriesConfig _directoriesConfig;

    public SquiddyBootstrap(LoggerConfiguration loggerConfiguration)
    {
        _loggerConfiguration = loggerConfiguration.WriteTo.SpectreConsole(
                "{Timestamp:HH:mm:ss} [{Level:u4}] {Message:lj}{NewLine}{Exception}",
                minLevel: LogEventLevel.Information
            )
            .WriteTo.Console();
    }

    private void BuildLogger(IServiceCollection services)
    {
        _loggerConfiguration = _loggerConfiguration.WriteTo.File(
            formatter: new JsonFormatter(),
            path: Path.Combine(_directoriesConfig[DirectoryNameType.Logs], "squiddy_.log"),
            rollingInterval: RollingInterval.Day,
            rollOnFileSizeLimit: true
        );

        _logger = _loggerConfiguration.CreateLogger();
        _logger.Information("Starting up...");

        services.AddLogging(
            builder => builder
                .ClearProviders()
                .AddSerilog(_logger)
        );
    }

    private async Task LoadConfig(IServiceCollection services)
    {
        var rootDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        // If linux or osx get .config directory
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            rootDirectory = Path.Combine(rootDirectory, ".config");
        }

        rootDirectory = Path.Combine(rootDirectory, "squiddy");

        if (!Directory.Exists(rootDirectory))
        {
            Directory.CreateDirectory(rootDirectory);
        }

        _directoriesConfig = new DirectoriesConfig();
        _directoriesConfig.Initialize(rootDirectory);

        services.AddSingleton(_directoriesConfig);
        var configFileName = Path.Combine(_directoriesConfig[DirectoryNameType.Root], "squiddy_config.json");

        if (!File.Exists(configFileName))
        {
            File.WriteAllTextAsync(configFileName, new SquiddyConfig().ToJson());
        }

        var config = (await File.ReadAllTextAsync(configFileName)).FromJson<SquiddyConfig>();

        services.AddSingleton<IOptions<SquiddyConfig>>(new OptionsWrapper<SquiddyConfig>(config));
    }

    public Task RunHostAsync(string[] args)
    {
        return Host.CreateDefaultBuilder()
            .ConfigureServices(
                services =>
                {
                    LoadConfig(services).GetAwaiter().GetResult();

                    BuildLogger(services);
                    services = services
                        .AddSingleton<ISquiddyBootstrap>(this)
                        .RegisterMessageBus()
                        .AddHostedService<SquiddyBootstrapInterceptor>();

                    //Register services
                    services
                        .AddSingleton<IMessageBusService, MessageBusService>();


                    services.RegisterWindowAndViewModel<MainWindowView, MainWindowViewModel>();

                    services.AddSingleton(services);

                    _servicesFunc.Invoke(services);
                }
            )
            .RunConsoleAsync();
    }


    public Task ConfigureServices(Func<IServiceCollection, IServiceCollection> services)
    {
        _servicesFunc = services;

        return Task.CompletedTask;
    }

    public async Task LoadServices(IServiceProvider serviceProvider, IServiceCollection serviceCollection)
    {
        var servicesToStart = serviceCollection.AsParallel()
            .Where(
                k => k.ImplementationType?.GetCustomAttribute<ServiceOrderAttribute>() != null
            )
            .Select(
                s => (s.ImplementationType.GetCustomAttribute<ServiceOrderAttribute>().Order,
                    s.ImplementationType.GetCustomAttribute<ServiceOrderAttribute>(), s)
            )
            .OrderBy(j => j.Item1)
            .ToList();

        foreach (var service in servicesToStart)
        {
            _logger.Information("Starting service {Service}", service.Item3.ImplementationType.Name);
            var serviceImpl = serviceProvider.GetRequiredService(service.s.ServiceType);
            if (serviceImpl is ISquiddyService squiddyService)
            {
                await squiddyService.InitializeAsync();
                await squiddyService.StartAsync();
            }
        }
    }

    public Task RunApplicationAsync(IServiceProvider serviceProvider)
    {
        Application.QuitKey = Key.CtrlMask | Key.C;
        Application.Init();
        RxApp.MainThreadScheduler = TerminalScheduler.Default;
        RxApp.TaskpoolScheduler = TaskPoolScheduler.Default;
        Application.Run(serviceProvider.GetRequiredService<MainWindowView>(), ErrorHandler);
        Application.Shutdown();
        return Task.CompletedTask;
    }

    private bool ErrorHandler(Exception arg)
    {
        _logger.Error(arg, "Error in application");
        return true;
    }
}
