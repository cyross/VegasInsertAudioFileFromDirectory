using ScriptPortal.Vegas;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VegasScriptHelper;

namespace VegasInsertAudioFileFromDirectory
{
    public class EntryPoint
    {
        public void FromVegas(Vegas vegas)
        {
            VegasHelper helper = VegasHelper.Instance(vegas);

            AudioTrack selectedAudioTrack = null;
            try
            {
                selectedAudioTrack = helper.SelectedAudioTrack();
            }
            catch (VegasHelperTrackUnselectedException)
            {
                selectedAudioTrack = null;
            }

            List<AudioTrack> audioTracks = helper.AllAudioTracks.ToList();

            Dictionary<string, AudioTrack> keyValuePairs = new Dictionary<string, AudioTrack>(); ;

            foreach (AudioTrack audioTrack in audioTracks)
            {
                keyValuePairs[helper.GetTrackKey(audioTrack)] = audioTrack;
            }
            List<string> keyList = keyValuePairs.Keys.ToList();

            Setting settingDialog = new Setting()
            {
                AudioFileFolder = VegasScriptSettings.OpenDirectory,
                AudioInterval = VegasScriptSettings.AudioInsertInterval,
                IsRecursive = VegasScriptSettings.IsRecursive,
                StartFrom = VegasScriptSettings.StartFrom,
                MediaBinName = VegasScriptSettings.DefaultBinName["voiroVoice"],
                TrackNameDataSource = keyList,
                TrackName = selectedAudioTrack != null ? helper.GetTrackKey(selectedAudioTrack) : keyList.First()
            };

            if (settingDialog.ShowDialog() == DialogResult.Cancel) { return; }

            string selectedPath = settingDialog.AudioFileFolder;
            float interval = settingDialog.AudioInterval;
            bool isRecursive = settingDialog.IsRecursive;
            bool startFrom = settingDialog.StartFrom;

            AudioTrack targetAudioTrack = null;
            if(keyValuePairs.ContainsKey(settingDialog.TrackName))
            {
                targetAudioTrack = keyValuePairs[settingDialog.TrackName];
            }

            helper.InseretAudioInTrack(
                selectedPath,
                interval,
                startFrom,
                isRecursive,
                settingDialog.UseMediaBin,
                settingDialog.MediaBinName,
                targetAudioTrack,
                settingDialog.TrackName
                );

            VegasScriptSettings.OpenDirectory = selectedPath;
            VegasScriptSettings.AudioInsertInterval = interval;
            VegasScriptSettings.IsRecursive = isRecursive;
            VegasScriptSettings.StartFrom = startFrom;
            VegasScriptSettings.Save();
        }
    }
}
