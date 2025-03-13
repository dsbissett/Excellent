using Application = System.Windows.Application;

namespace Excellent.ViewModels.Implementations;

using System.Reactive;
using System.Reactive.Linq;
using Button = System.Windows.Controls.Button;
using Interfaces;
using ReactiveUI;
using Excellent.Interfaces;
using System.Reactive.Subjects;

public class MainWindowViewModel : IMainWindowViewModel
{
    private readonly IAudioService _audioService;
    private readonly IButtonService _buttonService;
    private IDisposable? _colorUpdateSubscription;
    private readonly Dictionary<string, Button> _buttonCache = new();
    private readonly BehaviorSubject<double> _pitchSubject = new(1.0);
    public double CurrentPitch
    {
        get => _pitchSubject.Value;
        set
        {
            _pitchSubject.OnNext(value);
            SetPitchCommand.Execute(value).Subscribe();
        }
    }

    public ReactiveCommand<Button, Unit> ButtonClickCommand { get; }
    public ReactiveCommand<bool, Unit> ToggleColorChangeCommand { get; }
    public ReactiveCommand<double, Unit> SetPitchCommand { get; }
    public ReactiveCommand<Unit, Unit> StopSoundsCommand { get; }
    public ReactiveCommand<Unit, Unit> ResetPitchCommand { get; }

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
            _pitchSubject.OnNext(clampedPitch);
        });

        ToggleColorChangeCommand = ReactiveCommand.Create<bool>(isEnabled => 
        {
            _colorUpdateSubscription?.Dispose();
            
            if (isEnabled)
            {
                _colorUpdateSubscription = Observable
                    .Interval(TimeSpan.FromSeconds(1.5))
                    .Subscribe(_ => UpdateButtonColors());
            }
            else
            {
                ResetButtonsToDefaultColor();
            }
        });

        // Start with color change disabled by default
        ToggleColorChangeCommand.Execute(false).Subscribe();

        ResetPitchCommand = ReactiveCommand.Create(() =>
        {
            _pitchSubject.OnNext(1.0);
            SetPitchCommand.Execute(1.0).Subscribe();
        });
    }

    public void RegisterButton(Button button)
    {
        if (!_buttonCache.ContainsKey(button.Name))
        {
            _buttonCache[button.Name] = button;
            // Don't set initial random color - will be handled by ToggleColorChangeCommand
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

    private void ResetButtonsToDefaultColor()
    {
        foreach (var button in _buttonCache.Values)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                button.ClearValue(Button.BackgroundProperty);
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