namespace Excellent.ViewModels.Implementations
{
    using System.Windows.Controls;
    using Interfaces;
    using ReactiveUI;

    public class MainWindowViewModel : IMainWindowViewModel
    {
        public MainWindowViewModel(IButtonService btnService)
        {
            this.ButtonClickCommand =
                ReactiveCommand.CreateFromObservable<Button, Button>(btnService.ButtonClickedObservable);
        }

        public ReactiveCommand ButtonClickCommand { get; }

        public static void DoStuff()
        {
            
        }
    }
}