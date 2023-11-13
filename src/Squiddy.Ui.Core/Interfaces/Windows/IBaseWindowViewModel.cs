using Avalonia.Controls;
using ReactiveUI;

namespace Squiddy.Ui.Core.Interfaces.Windows;

public interface IBaseWindowViewModel<TView> where TView : class
{
    TView View { get; set; }

    void Initialize();
}
