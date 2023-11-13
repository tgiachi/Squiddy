using Squiddy.Gui.Windows.Views;
using Squiddy.Ui.Core.Impl.Windows;

namespace Squiddy.Gui.Windows.ViewModels;

public class MainWindowViewModel : AbstractBaseViewModel<MainWindow>
{


    public override void Initialize()
    {
        View.Title = "Squiddy";
        base.Initialize();
    }
}
