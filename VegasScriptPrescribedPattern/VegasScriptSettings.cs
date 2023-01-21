using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace VegasScriptPrescribedPattern
{
    internal class VegasScriptSettings
    {
        public static float AudioInsertInterval;
        public static string OpenDirectory;

        public static void Load()
        {
            Properties.Vegas.Default.Upgrade();
            AudioInsertInterval = Properties.Vegas.Default.audioInsertInterval;
            OpenDirectory = Properties.Vegas.Default.openDirectory;
        }

        public static void Save()
        {
            Properties.Vegas.Default.audioInsertInterval = AudioInsertInterval;
            Properties.Vegas.Default.openDirectory = OpenDirectory;
            Properties.Vegas.Default.Save();
        }
    }
}
