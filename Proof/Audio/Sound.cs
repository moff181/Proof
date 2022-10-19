﻿using NAudio.Wave;
using Proof.Core.Logging;

namespace Proof.Audio
{
    public sealed class Sound : IDisposable
    {
        private readonly ALogger _logger;
        private readonly WaveFileReader _waveReader;
        private readonly WaveOutEvent _wave;

        internal Sound(ALogger logger, string path)
        {
            _logger = logger;

            _logger.LogInfo($"Loading sound from {path}...");

            Path = path;

            _waveReader = new WaveFileReader(path);
            _wave = new WaveOutEvent();
            _wave.Init(_waveReader);

            _logger.LogInfo("Sound loaded.");
        }

        ~Sound()
        {
            Dispose();
        }

        public void Dispose()
        {
            _logger.LogInfo($"Disposing of Sound: {Path}");

            _wave.Dispose();
            _waveReader.Dispose();

            _logger.LogInfo("Sound disposed of successfully.");
        }

        public string Path { get; }

        public void Play()
        {
            _wave.Play();
        }

        public void Stop()
        {
            _wave.Stop();
        }

        public void Pause()
        {
            _wave.Pause();
        }
    }
}
