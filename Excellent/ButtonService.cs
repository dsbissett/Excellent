namespace Excellent
{
    using System;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using ButtonHandlers;
    using Interfaces;

    public interface IButtonService
    {
        Task WhenButtonClicked(Button btn);

        IObservable<Button> ButtonClickedObservable(Button obj);
    }

    public class ButtonService : IButtonService
    {
        private readonly ButtonHandler buttonHandler;

        public ButtonService(IAudioService audioService, ICatFactsService catFactsService)
        {
            var audioButtonHandler = new AudioButtonHandler(audioService, catFactsService);

            var catFactsButtonHandler = new CatFactsButtonHandler(audioService, catFactsService);

            audioButtonHandler.SetSuccessor(catFactsButtonHandler);

            this.buttonHandler = audioButtonHandler;
        }

        public IObservable<Button> ButtonClickedObservable(Button obj)
        {
            return Observable.Return(obj).Do(async x => await this.WhenButtonClicked(x));
        }

        public async Task WhenButtonClicked(Button btn)
        {
            await this.buttonHandler.HandleRequest(btn);
        }
    }
}