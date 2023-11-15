using UnityEngine;
using System.IO;

namespace MyFileSystem
{
    /// <summary>
    /// Jsonファイル管理用クラス
    /// </summary>
    public static class JsonFileSystem
    {
        /// <summary>
        /// Jsonセーブ処理
        /// </summary>
        /// <param name="path_">保存先ファイルパス</param>
        /// <param name="obj_">保存するデータ</param>
        public static void SaveToJson(string path_, object obj_)
        {
            // Json文字列化してセーブする
            Save(path_, JsonUtility.ToJson(obj_));
        }

        /// <summary>
        /// Jsonセーブ処理
        /// </summary>
        /// <param name="path_">保存先ファイルパス</param>
        /// <param name="str_">保存する文字列</param>
        public static void Save(string path_, string str_)
        {
            // セーブするディレクトリを探す
            if (!Directory.Exists($"{path_}/../"))
            {
                // ないなら作る
                Directory.CreateDirectory($"{path_}/../");
            }
            // 書き込むためのインスタンス作成
            StreamWriter _sw = new StreamWriter(path_, false);
            // 書き込み、反映、終了
            _sw.Write(str_);
            _sw.Flush();
            _sw.Close();

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }

        /// <summary>
        /// Jsonロード処理
        /// </summary>
        /// <typeparam name="T">読み取る型</typeparam>
        /// <param name="path_">読取先パス</param>
        /// <param name="data_">出力されるデータ</param>
        /// <returns>パスが存在するか否か</returns>
        public static bool Load<T>(string path_, out T data_)
        {
            // 文字列をロードする
            var _isLoaded = Load(path_, out var str);
            // ロードできていたらオブジェクトに直す
            data_ = _isLoaded ? JsonUtility.FromJson<T>(str) : default;

            return _isLoaded;
        }

        /// <summary>
        /// Jsonロード処理
        /// </summary>
        /// <param name="path_">読取先パス</param>
        /// <param name="str_">出力される文字列</param>
        /// <returns>パスが存在するか否か</returns>
        public static bool Load(string path_, out string str_)
        {
            // ディレクトリを探してないなら返す
            if (!Directory.Exists($"{path_}/../"))
            {
                str_ = default;
                return false;
            }

            // ファイルを探してないなら返す
            if (!File.Exists(path_))
            {
                str_ = default;
                return false;
            }

            // 読み込むためのインスタンス作成
            StreamReader _sr = new StreamReader(path_);
            // 読み込み、終了
            str_ = _sr.ReadToEnd();
            _sr.Close();

            return true;
        }

        /// <summary>
        /// Jsonデリート処理
        /// </summary>
        /// <param name="path_">消去先ファイルパス</param>
        /// /// <returns>パスが存在するか否か</returns>
        public static bool Delete(string path_)
        {
            // ディレクトリを探してないなら返す
            if (!Directory.Exists($"{path_}/../"))
            {
                return false;
            }

            // ファイルを探してないなら返す
            if (!File.Exists(path_))
            {
                return false;
            }

            // ファイル削除
            File.Delete(path_);

#if UNITY_EDITOR
            // metaファイルも削除
            File.Delete(path_ + ".meta");
            UnityEditor.AssetDatabase.Refresh();
#endif

            return true;
        }
    }
}