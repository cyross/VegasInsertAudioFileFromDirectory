using System;
using System.IO;
using System.Runtime.InteropServices;
using ScriptPortal.Vegas;

namespace VegasScriptPrescribedPattern
{
    /// <summary>
    /// Vegasオブジェクトを操作するヘルパクラス
    /// 本クラスはSingleton
    /// </summary>
    internal class VegasHelper
    {
        private static VegasHelper _instance = null;

        internal Vegas Vegas { get; set; }

        internal static VegasHelper Instance(Vegas vegas)
        {
            if (_instance == null)
            {
                _instance = new VegasHelper(vegas);
            }
            else
            {
                _instance.Vegas = vegas;
            }
            return _instance;
        }

        private VegasHelper(Vegas vegas)
        {
            Vegas = vegas;
        }

        /// <summary>
        /// 現在VEGASが開いているプロジェクトを取得する
        /// </summary>
        internal Project Project
        {
            get
            {
                return Vegas.Project;
            }
        }

        /// <summary>
        /// プロジェクト内で選択しているトラックがあれば、そのトラックのオブジェクトを返す。
        /// なければnullを返す
        /// </summary>
        /// <returns>選択プロジェクトがあればそのTrackオブジェクト、なければnull</returns>
        internal Track SelectedTrack()
        {
            return SelectedTrack(Vegas.Project);
        }

        /// <summary>
        /// プロジェクト内で選択しているトラックがあれば、そのトラックのオブジェクトを返す。
        /// なければnullを返す
        /// </summary>
        /// <param name="project">VEGASが開いているプロジェクト</param>
        /// <returns>選択プロジェクトがあればそのTrackオブジェクト、なければnull</returns>
        internal Track SelectedTrack(Project project)
        {
            foreach(Track track in project.Tracks)
            {
                if (track.Selected)
                {
                    return track;
                }
            }
            return null;
        }

        /// <summary>
        /// プロジェクト内で選択しているトラックがあれば、そのトラックのオブジェクトを返す。
        /// なければnullを返す
        /// </summary>
        /// <returns>選択プロジェクトがあればそのTrackオブジェクト、なければnull</returns>
        internal VideoTrack SelectedVideoTrack()
        {
            return SelectedVideoTrack(Vegas.Project);
        }

        /// <summary>
        /// プロジェクト内で選択しているトラックがあれば、そのトラックのオブジェクトを返す。
        /// なければnullを返す
        /// </summary>
        /// <param name="project">VEGASが開いているプロジェクト</param>
        /// <returns>選択プロジェクトがあればそのTrackオブジェクト、なければnull</returns>
        internal VideoTrack SelectedVideoTrack(Project project)
        {
            Track track = SelectedTrack();
            if (track is null)
            {
                return null;
            }
            return track.IsVideo() ? (VideoTrack)track : null;
        }

        /// <summary>
        /// プロジェクト内で選択しているトラックがあれば、そのトラックのオブジェクトを返す。
        /// なければnullを返す
        /// </summary>
        /// <returns>選択プロジェクトがあればそのTrackオブジェクト、なければnull</returns>
        internal AudioTrack SelectedAudioTrack()
        {
            return SelectedAudioTrack(Vegas.Project);
        }

        /// <summary>
        /// プロジェクト内で選択しているトラックがあれば、そのトラックのオブジェクトを返す。
        /// なければnullを返す
        /// </summary>
        /// <param name="project">VEGASが開いているプロジェクト</param>
        /// <returns>選択プロジェクトがあればそのTrackオブジェクト、なければnull</returns>
        internal AudioTrack SelectedAudioTrack(Project project)
        {
            Track track = SelectedTrack();
            if (track is null)
            {
                return null;
            }
            return track.IsAudio() ? (AudioTrack)track : null;
        }

        /// <summary>
        /// 引数で指定したトラックがビデオトラックかどうかを調べる
        /// </summary>
        /// <param name="track">対象のトラックオブジェクト</param>
        /// <returns>ビデオトラックの場合はTrue、それ以外のときはFalseを返す</returns>
        internal bool IsVideoTrack(Track track)
        {
            return track.IsVideo();
        }

        /// <summary>
        /// 引数で指定したトラックがオーディオトラックかどうかを調べる
        /// </summary>
        /// <param name="track">対象のトラックオブジェクト</param>
        /// <returns>オーディオトラックの場合はTrue、それ以外のときはFalseを返す</returns>
        internal bool IsAudioTrack(Track track)
        {
            return track.IsAudio();
        }

        internal VideoTrack AddVideoTrack()
        {
            return Vegas.Project.AddVideoTrack();
        }

        internal AudioTrack AddAudioTrack()
        {
            return Vegas.Project.AddAudioTrack();
        }

        /// <summary>
        /// オーディオトラックを作り、指定したディレクトリ内の音楽ファイルをイベントとして挿入する
        /// </summary>
        /// <param name="fileDir">指定したディレクトリ</param>
        /// <param name="fromStart">トラックの最初から挿入するかどうかを示すフラグ。trueのときは最初から、falseのときは現在のカーソル位置から</param>
        internal void InseretAudioInTrack(string fileDir, bool fromStart = false)
        {
            AudioTrack audioTrack = AddAudioTrack();
            audioTrack.Selected = true;

            Timecode currentPosition = fromStart ? new Timecode() : Vegas.Cursor;

            foreach(string filePath in Directory.GetFiles(fileDir))
            {
                if(Path.GetExtension(filePath)　== ".wav")
                {
                    Media audioMedia = new Media(filePath);
                    AudioStream audioStream = audioMedia.GetAudioStreamByIndex(0);

                    AudioEvent audioEvent = audioTrack.AddAudioEvent(currentPosition, audioStream.Length);
                    audioEvent.AddTake(audioStream);

                    currentPosition += audioStream.Length;
                }
            }
        }
    }
}
