namespace Excellent;

using Application = System.Windows.Application;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var bootstrapper = new Bootstrapper();

        var container = bootstrapper.CreateContainer();

        var window = container.Locate<Views.MainWindow>();

        window.Show();
    }
}