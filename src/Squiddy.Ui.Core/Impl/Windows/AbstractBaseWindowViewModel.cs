using Avalonia.Controls;
using ReactiveUI;
using Squiddy.Ui.Core.Interfaces.Windows;

namespace Squiddy.Ui.Core.Impl.Windows;

public abstract class AbstractBaseViewModel<TWindow> : ReactiveObject, IBaseWindowViewModel<TWindow> where TWindow : Window
{
    public TWindow View { get; set; }

    public virtual void Initialize()
    {
    }
}
