using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace SoundMachine
{
    class WaveOutModEvent : WaveOutEvent
    {
        public bool Stoppable = false;
        public int SoundNumber = -1;
        public int SoundIndex = -1;
    }
}
