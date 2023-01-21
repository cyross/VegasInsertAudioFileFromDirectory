using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace VegasInsertAudioFileFromDirectory
{
    internal class VegasScriptSettings
    {
        public static float AudioInsertInterval;
        public static string OpenDirectory;
        public static bool IsRecursive;
        public static bool StartFrom;

        public static void Load()
        {
            Properties.Vegas.Default.Upgrade();
            AudioInsertInterval = Properties.Vegas.Default.audioInsertInterval;
            OpenDirectory = Properties.Vegas.Default.openDirectory;
            IsRecursive = Properties.Vegas.Default.isRecursive;
            StartFrom = Properties.Vegas.Default.startFrom;
        }

        public static void Save()
        {
            Properties.Vegas.Default.startFrom = StartFrom;
            Properties.Vegas.Default.isRecursive = IsRecursive;
            Properties.Vegas.Default.audioInsertInterval = AudioInsertInterval;
            Properties.Vegas.Default.openDirectory = OpenDirectory;
            Properties.Vegas.Default.Save();
        }
    }
}
