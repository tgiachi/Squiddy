using Avalonia.Controls;
using ReactiveUI;
using Squiddy.Ui.Core.Interfaces.Windows;

namespace Squiddy.Ui.Core.Impl.Controls;

public class AbstractBaseControlViewModel<TUserControl> : ReactiveObject, IBaseWindowViewModel<TUserControl>
    where TUserControl : UserControl
{
    public TUserControl View { get; set; }

    public virtual void Initialize()
    {
    }
}
