using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Terminal.Gui;

namespace Squiddy.Ui.Core.MethodEx;

public static class RegisterWindowAndViewModelMethodEx
{
    public static IServiceCollection RegisterWindowAndViewModel<TView, TViewModel>(this IServiceCollection serviceCollection)
        where TView : View where TViewModel : class, IReactiveObject =>
        serviceCollection.AddTransient<TView>().AddTransient<TViewModel>();
}
