using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Squiddy.Core.Interfaces.Bootstrap;
using Terminal.Gui;

namespace Squiddy.Gui.Bootstrap;

public class SquiddyBootstrapInterceptor : IHostedService
{
    private readonly ILogger _logger;
    private readonly ISquiddyBootstrap _bootstrap;
    private readonly IServiceProvider _serviceProvider;
    private readonly IServiceCollection _serviceCollection;
    private readonly IHost _host;

    public SquiddyBootstrapInterceptor(
        ILogger<SquiddyBootstrapInterceptor> logger, IHostApplicationLifetime applicationLifetime,
        IServiceCollection serviceCollection,
        ISquiddyBootstrap bootstrap,
        IServiceProvider serviceProvider,
        IHost host
    )
    {
        _host = host;
        _logger = logger;
        _bootstrap = bootstrap;
        _serviceProvider = serviceProvider;
        _serviceCollection = serviceCollection;
        applicationLifetime.ApplicationStarted.Register(OnStarted);
        applicationLifetime.ApplicationStopping.Register(OnStopping);
    }

    private void OnStopping()
    {
        Application.Shutdown();
    }

    private void OnStarted()
    {
        _logger.LogInformation("Squiddy has started!");
        _bootstrap.LoadServices(_serviceProvider, _serviceCollection);
        _bootstrap.RunApplicationAsync(_serviceProvider, _host).GetAwaiter().GetResult();
    }

    public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
