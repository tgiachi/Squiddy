using Microsoft.Extensions.Logging;
using ReactiveUI;
using Squiddy.Ui.Core.Interfaces.Controls;
using Terminal.Gui;

namespace Squiddy.Ui.Core.Impl.Controls;

public abstract class AbstractBaseControl<TViewModel> : View, IBaseControl<TViewModel>
    where TViewModel : class, IReactiveObject
{
    protected ILogger Logger { get; }

    protected AbstractBaseControl(ILogger<AbstractBaseControl<TViewModel>> logger, TViewModel viewModel)
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
