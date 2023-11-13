using ReactiveUI;

namespace Squiddy.Ui.Core.Interfaces.Controls;

public interface IBaseControl<TViewModel> : IViewFor<TViewModel> where TViewModel : class, IReactiveObject
{
}
