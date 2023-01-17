using ScriptPortal.Vegas;
using System;
using System.Windows.Forms;

namespace VegasScriptPrescribedPattern
{
    public class EntryPoint
    {
        public void FromVegas(Vegas vegas)
        {
            VegasHelper helper = VegasHelper.Instance(vegas);
        }

        /// <summary>
        /// フォルダ選択ダイアログを開き、選択したフォルダの全wavファイルを新規オーディオファイルに貼り付ける
        /// 貼り付け開始は現在のタイムルーラーの位置から。
        /// </summary>
        /// <param name="helper">VegasHelperオブジェクト</param>
        /// <param name="initialFolder">参照開始するフォルダ。初期値はMyDocuments</param>
        private void InsertWaveFileInNewAudioTrack(VegasHelper helper, Environment.SpecialFolder initialFolder = Environment.SpecialFolder.MyDocuments)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.RootFolder = initialFolder;
            if (folderBrowser.ShowDialog() == DialogResult.OK )
            {
                helper.InseretAudioInTrack(folderBrowser.SelectedPath);
            }
        }
    }
}
