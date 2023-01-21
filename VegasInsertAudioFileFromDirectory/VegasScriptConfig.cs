using System;
using System.Collections.Generic;

namespace VegasInsertAudioFileFromDirectory
{
    /// <summary>
    /// VEGASのスクリプトで使われる設定を参照するためのオブジェクト
    /// Singleton
    /// </summary>
    internal class VegasScriptConfig
    {
        private static List<string> supportedAudioFile = new List<string>() { ".wav", ".mp3", ".ogg"};

        public static List<string> SupportedAudioFile { get { return supportedAudioFile; } }
    }
}
