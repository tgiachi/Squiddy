using Microsoft.Extensions.Logging;

using Squiddy.Core.Attributes.Windows;
using Squiddy.Gui.Controls.Views;
using Squiddy.Gui.Windows.ViewModels;
using Squiddy.Ui.Core.Impl.Windows;
using Terminal.Gui;

namespace Squiddy.Gui.Windows.Views;

[Window("Squiddy v")]
public class MainWindowView : AbstractBaseWindow<MainWindowViewModel>
{
    public MainWindowView(ILogger<MainWindowView> logger, MainWindowViewModel viewModel, MenuBarControlView menuBarControlView) : base(logger, viewModel)
    {
        Logger.LogInformation("Creating main window view...");
        var textField = new TextField(10, 10, 20, "");
        var button = new Button(10, 12, "Click me!");
        button.Clicked += () =>
        {
            textField.Text = "Hello World!";
            MessageBox.Query(20, 30, "Hello", "World", "Ok");
        };


        Add(menuBarControlView, textField, button );
    }
}
