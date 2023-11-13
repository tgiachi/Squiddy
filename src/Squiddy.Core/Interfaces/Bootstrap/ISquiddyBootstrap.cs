using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Squiddy.Core.Interfaces.Bootstrap;

public interface ISquiddyBootstrap
{

    Task RunHostAsync(string[] args);

    Task ConfigureServices(Func<IServiceCollection, IServiceCollection> services);

    Task LoadServices(IServiceProvider serviceProvider, IServiceCollection serviceCollection);

    Task RunApplicationAsync(IServiceProvider serviceProvider, IHost host);
}
