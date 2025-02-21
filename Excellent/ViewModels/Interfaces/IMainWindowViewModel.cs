using System.Windows.Controls;

namespace Excellent.ViewModels.Interfaces
{
    using ReactiveUI;
    using System.Reactive;

    public interface IMainWindowViewModel : IViewModel
    {
        ReactiveCommand<Button, Unit> ButtonClickCommand { get; }
        void RegisterButton(Button button);
        void Cleanup();
    }
}