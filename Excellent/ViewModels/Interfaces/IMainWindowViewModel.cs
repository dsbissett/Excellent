namespace Excellent.ViewModels.Interfaces
{
    using ReactiveUI;

    public interface IMainWindowViewModel : IViewModel
    {
        ReactiveCommand ButtonClickCommand { get; }
    }
}