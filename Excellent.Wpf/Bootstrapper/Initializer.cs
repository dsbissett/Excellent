namespace Excellent.Wpf.Bootstrapper
{
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Resources;
    using System.Security.AccessControl;
    using Grace.DependencyInjection;
    using Serilog;
    using ViewModels.Implementations;
    using ViewModels.Interfaces;

    public class Initializer
    {
        public Initializer()
        {
            SetupLogger();

            this.Container = SetupDi();
        }

        public DependencyInjectionContainer Container { get; }

        private static DependencyInjectionContainer SetupDi()
        {
            var container = new DependencyInjectionContainer();

            container.Configure(x => x.Export<MainViewModel>().As<IMainViewModel>());

            return container;
        }

        private static void SetupLogger()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.ColoredConsole().CreateLogger();
        }
    }
}