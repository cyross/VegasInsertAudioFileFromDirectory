using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ScriptPortal.Vegas;

namespace VegasInsertAudioFileFromDirectory
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

        internal string GetTrackTitle(Track track)
        {
            return track.Name;
        }

        internal string GetVideoTrackTitle()
        {
            VideoTrack track = SelectedVideoTrack();
            if (track is null) { return null; }
            return GetTrackTitle(track);
        }

        internal string GetAudioTrackTitle()
        {
            AudioTrack track = SelectedAudioTrack();
            if (track is null) { return null; }
            return GetTrackTitle(track);
        }

        internal void SetTrackTitle(Track track, string title)
        {
            track.Name = title;
        }

        internal void SetVideoTrackTitle(string title)
        {
            VideoTrack track = SelectedVideoTrack();
            if (track is null) { return; }
            SetTrackTitle(track, title);
        }

        internal void SetAudioTrackTitle(string title)
        {
            AudioTrack track = SelectedAudioTrack();
            if (track is null) { return; }
            SetTrackTitle(track, title);
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
        /// オーディオトラックを作り、指定したディレクトリ内のwavファイルをイベントとして挿入する
        /// オーディオファイルの検知は拡張子のみで、ファイルの中身はチェックしない
        /// 対応するファイルはVegasScriptConfig.SupportedAudioFileで指定されたもの
        /// </summary>
        /// <param name="fileDir">指定したディレクトリ名</param>
        /// <param name="interval">挿入するイベント間の間隔　単位はミリ秒　標準は0.0</param>
        /// <param name="fromStart">トラックの最初から挿入するかどうかを示すフラグ　trueのときは最初から、falseのときは現在のカーソル位置から</param>
        /// <param name="recursive">子ディレクトリのを再帰的にトラックの最初から挿入するかどうかを示すフラグ　trueのときは最初から、falseのときは現在のカーソル位置から</param>
        internal void InseretAudioInTrack(string fileDir, float interval = 0.0f, bool fromStart = false, bool recursive = true)
        {
            AudioTrack audioTrack = AddAudioTrack();
            SetTrackTitle(audioTrack, "Subtitles");
            audioTrack.Selected = true;

            Timecode currentPosition = fromStart ? new Timecode() : Vegas.Cursor;
            Timecode intervalTimecode = new Timecode(interval);

            _InsertAudio(currentPosition, intervalTimecode, fileDir, audioTrack, recursive);
        }

        private Timecode _InsertAudio(Timecode current, Timecode interval, string fileDir, AudioTrack audioTrack, bool recursive)
        {
            if (recursive)
            {
                foreach (string childDir in Directory.GetDirectories(fileDir))
                {
                    current = _InsertAudio(current, interval, childDir, audioTrack, recursive);
                }
            }
            foreach (string filePath in Directory.GetFiles(fileDir))
            {
                if (VegasScriptConfig.SupportedAudioFile.Contains(Path.GetExtension(filePath)))
                {
                    Media audioMedia = new Media(filePath);
                    AudioStream audioStream = audioMedia.GetAudioStreamByIndex(0);

                    AudioEvent audioEvent = audioTrack.AddAudioEvent(current, audioStream.Length);
                    audioEvent.AddTake(audioStream);

                    current += audioStream.Length + interval;
                }
            }
            return current;
        }
    }
}
