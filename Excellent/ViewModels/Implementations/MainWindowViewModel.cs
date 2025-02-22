using Application = System.Windows.Application;

namespace Excellent.ViewModels.Implementations;

using System.Reactive;
using System.Reactive.Linq;
using Button = System.Windows.Controls.Button;
using Interfaces;
using ReactiveUI;
using Excellent.Interfaces;

public class MainWindowViewModel : IMainWindowViewModel
{
    private readonly IAudioService _audioService;
    private readonly IButtonService _buttonService;
    private IDisposable? _colorUpdateSubscription;
    private readonly Dictionary<string, Button> _buttonCache = new();
    private readonly IObservable<double> _pitchObservable;

    public ReactiveCommand<Button, Unit> ButtonClickCommand { get; }
    public ReactiveCommand<bool, Unit> ToggleColorChangeCommand { get; }
    public ReactiveCommand<double, Unit> SetPitchCommand { get; }
    public ReactiveCommand<Unit, Unit> StopSoundsCommand { get; }
    
    public IObservable<double> PitchValue => _pitchObservable;

    private static double ClampPitch(double value) => Math.Clamp(value, 0.5, 2.0);

    public MainWindowViewModel(IAudioService audioService, IButtonService buttonService)
    {
        _audioService = audioService;
        _buttonService = buttonService;
        
        StopSoundsCommand = ReactiveCommand.CreateFromTask(async () => await _audioService.StopAllSounds());
        
        ButtonClickCommand = ReactiveCommand.CreateFromTask<Button>(
            async button => await _buttonService.WhenButtonClicked(button));

        SetPitchCommand = ReactiveCommand.Create<double>(pitch =>
        {
            var clampedPitch = ClampPitch(pitch);
            _audioService.SetPlaybackPitch(clampedPitch);
        });

        // Create a separate observable for the pitch value that watches the input parameter
        _pitchObservable = Observable.Merge(
            new IObservable<double>[] { 
                Observable.Return(1.0),
                SetPitchCommand.Select(x => Convert.ToDouble(x))
            }
        );

        ToggleColorChangeCommand = ReactiveCommand.Create<bool>(isEnabled => 
        {
            _colorUpdateSubscription?.Dispose();
            
            if (isEnabled)
            {
                _colorUpdateSubscription = Observable
                    .Interval(TimeSpan.FromSeconds(1.5))
                    .Subscribe(_ => UpdateButtonColors());
            }
        });

        // Start with color change enabled by default
        ToggleColorChangeCommand.Execute(true).Subscribe();
    }

    public void RegisterButton(Button button)
    {
        if (!_buttonCache.ContainsKey(button.Name))
        {
            _buttonCache[button.Name] = button;
            button.Background = new SolidColorBrush(ColorHelper.GetRandomColor());
        }
    }

    private void UpdateButtonColors()
    {
        foreach (var button in _buttonCache.Values)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                button.Background = new SolidColorBrush(ColorHelper.GetRandomColor());
            });
        }
    }

    public void Cleanup()
    {
        _colorUpdateSubscription?.Dispose();
    }

    public static void DoStuff()
    {
        
    }
}