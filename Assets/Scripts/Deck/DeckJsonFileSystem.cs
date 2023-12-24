using UnityEngine;
using MyFileSystem;

namespace Deck
{
    public static class DeckJsonFileSystem
    {
        #region Json
        public static void SaveJson(int index_, DeckData deck_)
        {
            var _str = JsonUtility.ToJson(deck_);

            // 暗号化する処理

            JsonFileSystem.Save(GetDeckFilePath(index_), _str);
        }

        public static bool LoadJson(int index_, out DeckData deck_)
        {
            if (false == JsonFileSystem.Load(GetDeckFilePath(index_), out string _str))
            {
                deck_ = null;
                return false;
            }

            // 暗号化から戻す処理

            deck_ = JsonUtility.FromJson<DeckData>(_str);
            return true;
        }

        public static bool DeleteJson(int index_)
        {
            return JsonFileSystem.Delete(GetDeckFilePath(index_));
        }

        public static string GetDeckFilePath(int index_)
        {
            return $"{Name.FilePath.FilePath_Deck}/Deck{index_}.json";
        }
        #endregion Json
    }
}