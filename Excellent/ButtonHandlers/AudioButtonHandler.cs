namespace Excellent.ButtonHandlers
{
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using Interfaces;
    using Properties;

    public class AudioButtonHandler : ButtonHandler
    {
        public override async Task HandleRequest(Button btn)
        {
            if (!string.IsNullOrWhiteSpace(btn.Name))
            {
                var res = Resources.ResourceManager.GetObject(btn.Name);

                if (res != null)
                {
                    await this.audioService.PlaySoundAsync(res);

                    Resources.ResourceManager.ReleaseAllResources();
                    
                    return;
                }
            }

            this.successor?.HandleRequest(btn);
        }

        public AudioButtonHandler(IAudioService audioService, ICatFactsService catFactsService) : base(audioService, catFactsService)
        {
        }
    }
}