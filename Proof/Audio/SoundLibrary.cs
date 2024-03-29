﻿using Proof.Core.Logging;

namespace Proof.Audio
{
    public class SoundLibrary
    {
        private readonly ALogger _logger;
        private readonly List<Sound> _sounds;

        public SoundLibrary(ALogger logger)
        {
            _logger = logger;
            _sounds = new List<Sound>();
        }

        public Sound Get(string filePath)
        {
            Sound? item = _sounds.FirstOrDefault(x => x.Path == filePath);
            if(item != null)
            {
                return item;
            }

            return new Sound(_logger, filePath);
        }
    }
}
