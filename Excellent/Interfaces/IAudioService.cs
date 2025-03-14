﻿namespace Excellent.Interfaces
{
    using System.Threading.Tasks;

    public interface IAudioService
    {
        Task PlaySoundAsync(object obj);
        Task StopAllSounds();
        void SetPlaybackPitch(double pitch);
    }
}