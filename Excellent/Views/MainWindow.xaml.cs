namespace Excellent.Views
{
    #region Usings

    using System;
    using System.Windows;
    using ViewModels.Interfaces;
    using System.Windows.Media;
    using System.Windows.Controls;

    #endregion

    public partial class MainWindow : Window
    {
        private readonly IMainWindowViewModel viewModel;

        public MainWindow(IMainWindowViewModel viewModel)
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            this.InitializeComponent();

            this.viewModel = viewModel;
            this.DataContext = viewModel;
            
            this.Loaded += MainWindow_Loaded;
            this.Closed += MainWindow_Closed;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RegisterAllButtons(this);
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            viewModel.Cleanup();
        }

        private void RegisterAllButtons(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                
                if (child is Button button && !string.IsNullOrEmpty(button.Name))
                {
                    viewModel.RegisterButton(button);
                }
                
                RegisterAllButtons(child);
            }
        }
    }
}