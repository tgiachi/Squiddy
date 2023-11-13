using ReactiveUI;

namespace Squiddy.Ui.Core.Interfaces.Windows;

public interface IBaseWindow<TViewModel> : IViewFor<TViewModel> where TViewModel : class, IReactiveObject
{
}
