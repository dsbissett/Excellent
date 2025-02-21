using System;
using System.Windows;
using System.Windows.Media;
using System.Reactive;
using System.Reactive.Linq;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Excellent.ViewModels.Implementations
{
    using Interfaces;
    using ReactiveUI;
    using System.Windows.Input;
    using Excellent.Interfaces;

    public class MainWindowViewModel : IMainWindowViewModel
    {
        private readonly IAudioService audioService;
        private readonly IButtonService buttonService;
        private readonly IDisposable colorUpdateSubscription;
        private readonly Dictionary<string, Button> buttonCache = new Dictionary<string, Button>();

        public ReactiveCommand<Unit, Unit> StopSoundsCommand { get; }
        
        public ReactiveCommand<Button, Unit> ButtonClickCommand { get; }

        // private double playbackPitch = 1.0;
        // public double PlaybackPitch
        // {
        //     get => playbackPitch;
        //     set
        //     {
        //         // Clamp the value between reasonable bounds
        //         playbackPitch = Math.Clamp(value, 0.5, 2.0);
        //         this.RaisePropertyChanged(nameof(PlaybackPitch));
        //     }
        // }

        public MainWindowViewModel(IAudioService audioService, IButtonService buttonService)
        {
            this.audioService = audioService;
            this.buttonService = buttonService;
            
            StopSoundsCommand = ReactiveCommand.CreateFromTask(async () => await audioService.StopAllSounds());
            
            ButtonClickCommand = ReactiveCommand.CreateFromTask<Button>(
                async button => await buttonService.WhenButtonClicked(button));

            // Create an observable that emits every 5 seconds
            colorUpdateSubscription = Observable
                .Interval(TimeSpan.FromSeconds(1.5))
                .Subscribe(_ => UpdateButtonColors());
        }

        public void RegisterButton(Button button)
        {
            if (!buttonCache.ContainsKey(button.Name))
            {
                buttonCache[button.Name] = button;
                button.Background = new SolidColorBrush(ColorHelper.GetRandomColor());
            }
        }

        private void UpdateButtonColors()
        {
            foreach (var button in buttonCache.Values)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    button.Background = new SolidColorBrush(ColorHelper.GetRandomColor());
                });
            }
        }

        public void Cleanup()
        {
            colorUpdateSubscription?.Dispose();
        }

        public static void DoStuff()
        {
            
        }
    }
}