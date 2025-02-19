namespace Excellent
{
    using Grace.DependencyInjection;
    using Interfaces;
    using ViewModels.Implementations;
    using ViewModels.Interfaces;

    public class Bootstrapper
    {
        public DependencyInjectionContainer CreateContainer()
        {
            var container = new DependencyInjectionContainer();

            container.Configure(x => x.Export<AudioService>().As<IAudioService>().Lifestyle.Singleton());
            container.Configure(x => x.Export<ButtonService>().As<IButtonService>());
            container.Configure(x => x.Export<CatFactsService>().As<ICatFactsService>());
            container.Configure(x => x.Export<MainWindowViewModel>().As<IMainWindowViewModel>());

            return container;
        }
    }
}