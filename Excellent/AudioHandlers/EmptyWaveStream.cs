namespace Excellent.AudioHandlers
{
    using System;
    using NAudio.Wave;

    public sealed class EmptyWaveStream : WaveStream
    {
        private static readonly Lazy<EmptyWaveStream> lazy = new Lazy<EmptyWaveStream>(() => new EmptyWaveStream());

        private EmptyWaveStream()
        {
        }

        public static EmptyWaveStream Instance => lazy.Value;

        public override int Read(byte[] buffer, int offset, int count) => 0;

        public override WaveFormat WaveFormat => null;

        public override long Length => 0;

        public override long Position { get { return 0; } set { } }
    }
}