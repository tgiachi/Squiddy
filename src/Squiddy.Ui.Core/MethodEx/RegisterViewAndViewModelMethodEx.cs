using Microsoft.Extensions.DependencyInjection;

namespace Squiddy.Ui.Core.MethodEx;

public static class RegisterViewAndViewModelMethodEx
{

    public static IServiceCollection RegisterViewAndViewModel<TView, TViewModel>(this IServiceCollection services)
        where TView : class
        where TViewModel : class
    {
        services.AddTransient<TView>();
        services.AddTransient<TViewModel>();

        return services;
    }

}
