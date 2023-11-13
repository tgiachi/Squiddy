using Microsoft.Extensions.Logging;
using Squiddy.Gui.Controls.ViewModels;
using Squiddy.Ui.Core.Impl.Controls;
using Terminal.Gui;

namespace Squiddy.Gui.Controls.Views;

public class MenuBarControlView : AbstractBaseControl<MenuBarControlViewModel>
{
    private readonly MenuBar _menuBar;

    public MenuBarControlView(ILogger<MenuBarControlView> logger, MenuBarControlViewModel viewModel) : base(
        logger,
        viewModel
    )
    {
        Width = Dim.Fill();
        Height = 1;
        var s = new MenuBarItem(
            "_File",
            new[]
            {
                new MenuBarItem("_Quit", "", () => Application.RequestStop())
            }
        );
        _menuBar = new MenuBar(new[] { s }) { Width = Dim.Fill(), Height = 1};

        Add(_menuBar);
    }
}
