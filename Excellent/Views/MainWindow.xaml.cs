namespace Excellent.Views
{
    #region Usings

    using System.Windows;
    using ViewModels.Interfaces;

    #endregion

    public partial class MainWindow : Window
    {
        public MainWindow(IMainWindowViewModel viewModel)
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

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