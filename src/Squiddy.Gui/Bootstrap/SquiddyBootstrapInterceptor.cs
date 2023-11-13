using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Squiddy.Core.Interfaces.Bootstrap;

namespace Squiddy.Gui.Bootstrap;

public class SquiddyBootstrapInterceptor : IHostedService
{
    private readonly ILogger _logger;
    private readonly ISquiddyBootstrap _bootstrap;
    private readonly IServiceProvider _serviceProvider;
    private readonly IServiceCollection _serviceCollection;

    public SquiddyBootstrapInterceptor(
        ILogger<SquiddyBootstrapInterceptor> logger, IHostApplicationLifetime applicationLifetime, IServiceCollection serviceCollection,
        ISquiddyBootstrap bootstrap, IServiceProvider serviceProvider
    )
    {
        _logger = logger;
        _bootstrap = bootstrap;
        _serviceProvider = serviceProvider;
        _serviceCollection = serviceCollection;
        applicationLifetime.ApplicationStarted.Register(OnStarted);
    }

    private void OnStarted()
    {
        _logger.LogInformation("Squiddy has started!");
        _bootstrap.LoadServices(_serviceProvider, _serviceCollection);
    }

    public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
