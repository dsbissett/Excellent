using System.Linq;

namespace Excellent
{
    using System.Threading.Tasks;
    using Annotations;
    using AudioHandlers;
    using Interfaces;
    using NAudio.Wave;
    using System.Collections.Generic;

    public class AudioService : IAudioService
    {
        private readonly AudioHandler handlers;
        private readonly List<WaveOutEvent> activeOutputs = new List<WaveOutEvent>();
        private readonly object lockObject = new object();
        private volatile bool isStopping;

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
            if (isStopping)
            {
                reader.Dispose();
                return;
            }

            var waveOutEvent = new WaveOutEvent();
            
            lock (lockObject)
            {
                if (isStopping)
                {
                    reader.Dispose();
                    waveOutEvent.Dispose();
                    return;
                }

                try 
                {
                    reader.Position = 0;
                    waveOutEvent.Init(reader);
                    activeOutputs.Add(waveOutEvent);
                    waveOutEvent.Play();
                }
                catch
                {
                    reader.Dispose();
                    waveOutEvent.Dispose();
                    throw;
                }
            }
            
            waveOutEvent.PlaybackStopped += (s, e) =>
            {
                lock (lockObject)
                {
                    activeOutputs.Remove(waveOutEvent);
                }
                reader.Dispose();
                waveOutEvent.Dispose();
            };
        }

        public Task PlaySoundAsync(object obj) => Task.Run(() =>
        {
            var reader = this.handlers.HandleRequest(obj);
            this.PlaySound(reader);
        });

        public void StopAllSounds()
        {
            isStopping = true;
            
            lock (lockObject)
            {
                foreach (var output in activeOutputs.ToList())
                {
                    try
                    {
                        output.Stop();
                    }
                    catch
                    {
                        // Ignore any errors during stop
                    }
                }
                activeOutputs.Clear();
            }

            // Add a small delay before allowing new sounds
            Task.Delay(100).ContinueWith(_ => isStopping = false);
        }
    }
}