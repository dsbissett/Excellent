namespace Excellent.ButtonHandlers
{
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using Interfaces;

    public class CatFactsButtonHandler : ButtonHandler
    {
        public CatFactsButtonHandler(IAudioService audioService, ICatFactsService catFactsService) : base(audioService, catFactsService)
        {
        }

        public override async Task HandleRequest(Button btn)
        {
            if (!string.IsNullOrWhiteSpace(btn.Name))
            {
                if (nameof(CatFactsService).Contains(btn.Name))
                {
                    await this.catFactsService.Get();
                }
                else
                {
                    this.successor?.HandleRequest(btn);
                }                
            }
        }
    }
}