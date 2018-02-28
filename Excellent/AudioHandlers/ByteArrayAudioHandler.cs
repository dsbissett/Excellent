namespace Excellent.AudioHandlers
{
    using System.IO;
    using NAudio.Wave;

    public class ByteArrayAudioHandler : AudioHandler
    {
        public override WaveStream HandleRequest(object obj)
        {
            if (obj is byte[] bytes)
            {
                var memoryStream = new MemoryStream(bytes);

                return new Mp3FileReader(memoryStream);
            }

            return this.successor?.HandleRequest(obj) ?? EmptyWaveStream.Instance;
        }
    }
}