﻿using Proof.Core.Logging;
using System.Media;
using System.Runtime.Versioning;

namespace Proof.Audio
{
    [SupportedOSPlatform("windows")]
    public class Sound
    {
        private readonly SoundPlayer _soundPlayer;

        internal Sound(ALogger logger, string path)
        {
            logger.LogInfo($"Loading sound from {path}");
            Path = path;
            _soundPlayer = new SoundPlayer(path);
            logger.LogInfo("Sound loaded.");
        }

        public string Path { get; }

        public void Play()
        {
            _soundPlayer.Play();
        }

        public void Loop()
        {
            _soundPlayer.PlayLooping();
        }

        public void Stop()
        {
            _soundPlayer.Stop();
        }
    }
}