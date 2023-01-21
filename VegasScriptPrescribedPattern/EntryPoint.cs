using ScriptPortal.Vegas;
using System;
using System.Windows.Forms;

namespace VegasScriptPrescribedPattern
{
    public class EntryPoint
    {
        public void FromVegas(Vegas vegas)
        {
            VegasScriptSettings.Load();
            VegasHelper helper = VegasHelper.Instance(vegas);
        }

        /// <summary>
        /// フォルダ選択ダイアログを開き、選択したフォルダの全wavファイルを新規オーディオファイルに貼り付ける
        /// 貼り付け開始は現在のタイムルーラーの位置から。
        /// </summary>
        /// <param name="helper">VegasHelperオブジェクト</param>
        private void InsertWaveFileInNewAudioTrack(VegasHelper helper)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.SelectedPath = VegasScriptSettings.OpenDirectory;
            float interval = VegasScriptSettings.AudioInsertInterval;
            if (folderBrowser.ShowDialog() == DialogResult.OK )
            {
                string selectedPath = folderBrowser.SelectedPath;
                helper.InseretAudioInTrack(selectedPath, interval);

                VegasScriptSettings.OpenDirectory = selectedPath;
                VegasScriptSettings.AudioInsertInterval = interval;
                VegasScriptSettings.Save();
            }
        }
    }
}
