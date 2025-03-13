using NAudio.Wave.SampleProviders;

namespace Excellent;

using System.Threading.Tasks;
using Annotations;
using AudioHandlers;
using Interfaces;
using NAudio.Wave;
using NAudio.Extras;
using System.Collections.Generic;
using System;

public class AudioService : IAudioService
{
    private readonly AudioHandler handlers;
    private readonly List<WaveOutEvent> activeOutputs = new List<WaveOutEvent>();
    private readonly object lockObject = new object();
    private volatile bool isStopping;
    private double currentPitch = 1.0;

    public AudioService()
    {
        var waveAudioHandler = new WaveAudioHandler();
        var byteArrayAudioHandler = new ByteArrayAudioHandler();
        waveAudioHandler.SetSuccessor(byteArrayAudioHandler);
        this.handlers = waveAudioHandler;
    }

    private WaveOutEvent ooter;

    public void SetPlaybackPitch(double pitch)
    {
        currentPitch = pitch;
    }

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
                
                // Apply pitch shifting if needed
                if (Math.Abs(currentPitch - 1.0) > 0.01)
                {
                    var pitchStream = new SmbPitchShiftingSampleProvider(reader.ToSampleProvider());
                    pitchStream.PitchFactor = (float)currentPitch;
                    waveOutEvent.Init(pitchStream);
                }
                else
                {
                    waveOutEvent.Init(reader);
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
    }

    public Task PlaySoundAsync(object obj) => Task.Run(() =>
    {
        var reader = this.handlers.HandleRequest(obj);
        this.PlaySound(reader);
    });

    public async Task StopAllSounds()
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

        // Wait for the delay before allowing new sounds
        await Task.Delay(100);
        isStopping = false;
    }
}