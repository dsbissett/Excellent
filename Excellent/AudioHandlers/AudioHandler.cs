namespace Excellent.AudioHandlers
{
    using NAudio.Wave;

    public abstract class AudioHandler
    {
        protected AudioHandler successor;

        public void SetSuccessor(AudioHandler successor)
        {
            this.successor = successor;
        }

        public abstract WaveStream HandleRequest(object obj);
    }
}