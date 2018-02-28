namespace Excellent.ButtonHandlers
{
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using Interfaces;

    public abstract class ButtonHandler
    {
        protected ButtonHandler successor;

        protected readonly IAudioService audioService;

        protected readonly ICatFactsService catFactsService;

        protected ButtonHandler(IAudioService audioService, ICatFactsService catFactsService)
        {
            this.audioService = audioService;
            this.catFactsService = catFactsService;
        }

        public void SetSuccessor(ButtonHandler successor)
        {
            this.successor = successor;
        }

        public abstract Task HandleRequest(Button btn);
    }
}