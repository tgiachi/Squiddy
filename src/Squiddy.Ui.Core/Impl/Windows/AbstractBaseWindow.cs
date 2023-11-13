using Microsoft.Extensions.Logging;
using ReactiveUI;
using Squiddy.Ui.Core.Interfaces.Windows;
using Terminal.Gui;

namespace Squiddy.Ui.Core.Impl.Windows;

public abstract class AbstractBaseWindow<TViewModel> : Window, IBaseWindow<TViewModel>
    where TViewModel : class, IReactiveObject
{
    protected ILogger Logger { get; }

    protected AbstractBaseWindow(ILogger<AbstractBaseWindow<TViewModel>> logger, TViewModel viewModel)
    {
        Logger = logger;
        ViewModel = viewModel;
    }


    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (TViewModel)value;
    }

    public TViewModel? ViewModel { get; set; }
}
