using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReactiveUI;
using Serilog;
using Serilog.Formatting.Json;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;
using Squiddy.Core.Attributes.Services;
using Squiddy.Core.Data.Configs;
using Squiddy.Core.Data.Directories;
using Squiddy.Core.Interfaces.Bootstrap;
using Squiddy.Core.Interfaces.Services;
using Squiddy.Core.MethodEx.Services;
using Squiddy.Core.MethodEx.Utils;
using Squiddy.Core.Services.Interfaces;
using Squiddy.Gui.Controls.ViewModels;
using Squiddy.Gui.Controls.Views;
using Squiddy.Gui.Impl.Services;
using Squiddy.Gui.Windows.ViewModels;
using Squiddy.Gui.Windows.Views;
using Squiddy.Ui.Core.MethodEx;
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
        _loggerConfiguration = loggerConfiguration
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

    private void LoadConfig(IServiceCollection services)
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

        var config = File.ReadAllText(configFileName).FromJson<SquiddyConfig>();

        services.AddSingleton<IOptions<SquiddyConfig>>(new OptionsWrapper<SquiddyConfig>(config));
    }

    public async Task<IHost> BuildHostAsync(string[] args)
    {
        return Host.CreateDefaultBuilder()
            .ConfigureServices(
                services =>
                {
                    LoadConfig(services);

                    BuildLogger(services);

                    services.UseMicrosoftDependencyResolver();
                    var resolver = Locator.CurrentMutable;
                    resolver.InitializeSplat();
                    resolver.InitializeReactiveUI();

                    services.AddHttpClient();

                    services = services
                        .AddSingleton<ISquiddyBootstrap>(this)
                        .RegisterMessageBus();
                    //.AddHostedService<SquiddyBootstrapInterceptor>();

                    //Register services
                    services
                        .AddSingleton<IMessageBusService, MessageBusService>();

                    services.RegisterViewAndViewModel<MainWindow, MainWindowViewModel>();

                    services.RegisterViewAndViewModel<MainConsoleUserControl, MainConsoleUserControlViewModel>();

                    services.AddSingleton(services);

                    _servicesFunc.Invoke(services);
                }
            )
            .Build();
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

    public async Task RunApplicationAsync(IServiceProvider serviceProvider, IHost host)
    {
    }
}
