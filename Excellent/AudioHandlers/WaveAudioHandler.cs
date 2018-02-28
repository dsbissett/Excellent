namespace Excellent.AudioHandlers
{
    using System.IO;
    using NAudio.Wave;

    public class WaveAudioHandler : AudioHandler
    {
        public override WaveStream HandleRequest(object obj)
        {
            if (obj is Stream stream)
            {
                return new WaveFileReader(stream);
            }
            
            return this.successor?.HandleRequest(obj) ??  EmptyWaveStream.Instance;
        }
    }
}