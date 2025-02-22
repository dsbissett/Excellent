namespace Excellent.ViewModels.Interfaces;

using Button = System.Windows.Controls.Button;
using ReactiveUI;
using System.Reactive;

public interface IMainWindowViewModel : IViewModel
{
    ReactiveCommand<Button, Unit> ButtonClickCommand { get; }
    ReactiveCommand<bool, Unit> ToggleColorChangeCommand { get; }
    void RegisterButton(Button button);
    void Cleanup();
}