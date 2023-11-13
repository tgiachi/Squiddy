using Microsoft.Extensions.Logging;

using Squiddy.Core.Attributes.Windows;
using Squiddy.Gui.Windows.ViewModels;
using Squiddy.Ui.Core.Impl.Windows;
using Terminal.Gui;

namespace Squiddy.Gui.Windows.Views;

[Window("Squiddy")]
public class MainWindowView : AbstractBaseWindow<MainWindowViewModel>
{
    public MainWindowView(ILogger<MainWindowView> logger, MainWindowViewModel viewModel) : base(logger, viewModel)
    {
        var textField = new TextField(10, 10, 20, "");
        var button = new Button(10, 12, "Click me!");
        button.Clicked += () =>
        {
            textField.Text = "Hello World!";
            MessageBox.Query(20, 30, "Hello", "World", "Ok");
        };

        Add(textField, button);
    }
}
