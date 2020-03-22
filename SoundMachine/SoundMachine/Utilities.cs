using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundMachine
{
    class Utilities
    {
        public static int ConvertKeyCodeToButtonId(int vkCode)
        {
            int id = -1;

            for(int i = 0; i < Config._currentConfig.Bindings.Length; i++)
            {
                if (Config._currentConfig.Bindings[i] == vkCode)
                    id = i;
            }

            return id;
        }
    }
}
