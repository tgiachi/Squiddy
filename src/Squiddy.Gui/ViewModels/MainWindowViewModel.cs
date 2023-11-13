using Squiddy.Gui.Views;
using Squiddy.Ui.Core.Impl.Windows;

namespace Squiddy.Gui.ViewModels;

public class MainWindowViewModel : AbstractBaseViewModel<MainWindow>
{
    public string Greeting => "Welcome to Avalonia!";


    public override void Initialize()
    {
        View.Title = "Sto cazzo";
        base.Initialize();
    }
}
