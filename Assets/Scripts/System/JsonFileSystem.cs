using UnityEngine;
using System.IO;

namespace MyFileSystem
{
    /// <summary>
    /// Jsonファイル管理用クラス
    /// </summary>
    /// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
    public static class JsonFileSystem
    {
        /// <summary>
        /// Jsonセーブ処理
        /// </summary>
        /// <param name="path_">保存先ファイルパス</param>
        /// <param name="obj_">保存するデータ</param>
        public static void Save(string path_, object obj_)
        {
            Save(path_, JsonUtility.ToJson(obj_));
        }

        /// <summary>
        /// Jsonセーブ処理
        /// </summary>
        /// <param name="path_">保存先ファイルパス</param>
        /// <param name="str_">保存する文字列</param>
        public static void Save(string path_, string str_)
        {
            if(!Directory.Exists($"{path_}/../"))
            {
                Directory.CreateDirectory($"{path_}/../");
            }

            StreamWriter _sw = new StreamWriter(path_, false);
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
            var _flag = Load(path_, out var str);
            data_ = _flag ? JsonUtility.FromJson<T>(str) : default;
            return _flag;
        }

        /// <summary>
        /// Jsonロード処理
        /// </summary>
        /// <param name="path_">読取先パス</param>
        /// <param name="str_">出力される文字列</param>
        /// <returns>パスが存在するか否か</returns>
        public static bool Load(string path_, out string str_)
        {
            if(!Directory.Exists($"{path_}/../"))
            {
                str_ = default;
                return false;
            }

            if (!File.Exists(path_))
            {
                str_ = default;
                return false;
            }

            StreamReader _sr = new StreamReader(path_);
            str_ = _sr.ReadToEnd();
            _sr.Close();

            return true;
        }
    }
}