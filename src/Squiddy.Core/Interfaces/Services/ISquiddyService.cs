namespace Squiddy.Core.Interfaces.Services;

public interface ISquiddyService
{
    Task<bool> InitializeAsync();

    Task<bool> StartAsync();

    Task<bool> StopAsync();
}
