namespace Excellent
{
    using System.Threading.Tasks;
    using Annotations;
    using AudioHandlers;
    using Interfaces;
    using NAudio.Wave;
    
    public class AudioService : IAudioService
    {
        private readonly AudioHandler handlers;

        public AudioService()
        {

            var waveAudioHandler = new WaveAudioHandler();

            var byteArrayAudioHandler = new ByteArrayAudioHandler();

            waveAudioHandler.SetSuccessor(byteArrayAudioHandler);

            this.handlers = waveAudioHandler;
        }

        private WaveOutEvent ooter;

        public void PlaySound([NotNull] WaveStream reader)
        {
            var outer = new WaveOutEvent();            

            reader.Position = 0;

            outer.Init(reader);

            outer.Play();            
        }

        public Task PlaySoundAsync(object obj) => Task.Run(() =>
        {
            var reader = this.handlers.HandleRequest(obj);

            this.PlaySound(reader);
        });
    }
}