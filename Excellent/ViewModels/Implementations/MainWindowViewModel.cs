using System.Reactive;

namespace Excellent.ViewModels.Implementations
{
    using Interfaces;
    using ReactiveUI;
    using System.Windows.Input;
    using Excellent.Interfaces;
    using System.Reactive.Linq;
    using System.Windows.Controls;

    public class MainWindowViewModel : IMainWindowViewModel
    {
        private readonly IAudioService audioService;
        private readonly IButtonService buttonService;

        public ICommand StopSoundsCommand { get; }
        
        public ReactiveCommand<Button, Unit> ButtonClickCommand { get; }

        public MainWindowViewModel(IAudioService audioService, IButtonService buttonService)
        {
            this.audioService = audioService;
            this.buttonService = buttonService;
            
            StopSoundsCommand = ReactiveCommand.Create(() => audioService.StopAllSounds());
            
            ButtonClickCommand = ReactiveCommand.CreateFromTask<Button>(
                async button => await buttonService.WhenButtonClicked(button));
        }

        public static void DoStuff()
        {
            
        }
    }
}