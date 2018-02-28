namespace Excellent.Wpf
{
    using System.Diagnostics;
    using System.Windows;
    using Bootstrapper;
    using Serilog;
    using Views;

    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.UseLocalConsole();

            var initializer = new Initializer();

            Log.Information("Application started.");

            var mainView = initializer.Container.Locate<MainView>();

            mainView.Show();
        }

        [Conditional("DEBUG")]
        private void UseLocalConsole()
        {
            ConsoleManager.ShowConsoleWindow();
        }
    }
}