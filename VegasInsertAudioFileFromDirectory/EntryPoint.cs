using ScriptPortal.Vegas;
using System;
using System.Windows.Forms;

namespace VegasInsertAudioFileFromDirectory
{
    public class EntryPoint
    {
        public void FromVegas(Vegas vegas)
        {
            VegasScriptSettings.Load();
            VegasHelper helper = VegasHelper.Instance(vegas);

            Setting settingDialog = new Setting();
            settingDialog.AudioFileFolder = VegasScriptSettings.OpenDirectory;
            settingDialog.AudioInterval = VegasScriptSettings.AudioInsertInterval;
            settingDialog.IsRecursive = VegasScriptSettings.IsRecursive;
            settingDialog.StartFrom = VegasScriptSettings.StartFrom;

            if (settingDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedPath = settingDialog.AudioFileFolder;
                float interval = settingDialog.AudioInterval;
                bool isRecursive = settingDialog.IsRecursive;
                bool startFrom = settingDialog.StartFrom;
                helper.InseretAudioInTrack(selectedPath, interval, startFrom, isRecursive);

                VegasScriptSettings.OpenDirectory = selectedPath;
                VegasScriptSettings.AudioInsertInterval = interval;
                VegasScriptSettings.IsRecursive = isRecursive;
                VegasScriptSettings.StartFrom = startFrom;
                VegasScriptSettings.Save();

            }
        }
    }
}
