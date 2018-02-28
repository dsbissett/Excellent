namespace Excellent.Wpf.Views
{
    using System.Windows;
    using ViewModels.Interfaces;

    public partial class MainView : Window
    {
        public MainView(IMainViewModel viewModel)
        {
            this.InitializeComponent();

            this.ViewModel = viewModel;
        }

        public IViewModel ViewModel
        {
            get => (IViewModel) this.DataContext;
            set => this.DataContext = value;
        }
    }
}