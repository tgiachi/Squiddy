using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SpectreConsole;
using Squiddy.Core.Attributes.Services;
using Squiddy.Core.Interfaces.Bootstrap;
using Squiddy.Core.Interfaces.Services;
using Squiddy.Core.MethodEx.Services;
using Squiddy.Core.Services.Interfaces;
using Squiddy.Gui.Impl.Services;
using ILogger = Serilog.ILogger;

namespace Squiddy.Gui.Bootstrap;

public class SquiddyBootstrap : ISquiddyBootstrap
{
    private ILogger _logger;

    private Func<IServiceCollection, IServiceCollection> _servicesFunc = services => services;
    private readonly LoggerConfiguration _loggerConfiguration;

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
        _logger = _loggerConfiguration.CreateLogger();
        _logger.Information("Starting up...");

        services.AddLogging(
            builder => builder
                .ClearProviders()
                .AddSerilog(_logger)
        );
    }

    public Task RunHostAsync(string[] args)
    {
        return Host.CreateDefaultBuilder()
            .ConfigureServices(
                services =>
                {
                    BuildLogger(services);
                    services = services
                        .AddSingleton<ISquiddyBootstrap>(this)
                        .RegisterMessageBus()
                        .AddHostedService<SquiddyBootstrapInterceptor>();

                    //Register services
                    services
                        .AddSingleton<IMessageBusService, MessageBusService>();

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
                k => k.ImplementationType != null && k.ImplementationType.GetCustomAttribute<ServiceOrderAttribute>() != null
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
}
