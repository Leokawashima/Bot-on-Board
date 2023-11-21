using System;
using System.Collections.Generic;
using UnityEngine;
using MyFileSystem;

public class DeckEditingManager : MonoBehaviour
{
    [SerializeField]
    private MapObjectTable_SO m_mapObjectTable;

    private const int PRESET_DECK_SIZE = 10;

    private List<DeckData> m_presetDeckList = new(PRESET_DECK_SIZE);

    [Flags]
    public enum DeckState
    {
        isRarilyLimit = 1 << 0,
        isSameBan = 1 << 1,
    }

    private void OpenDeckList()
    {
        m_presetDeckList = new(PRESET_DECK_SIZE);

        for(int i = 0; i < PRESET_DECK_SIZE; ++i)
        {
            if(LoadDeck(i, out DeckData _deck))
            {
                m_presetDeckList[i] = _deck;
            }
        }
    }

    private void CloseDeckList()
    {
        m_presetDeckList.Clear();
    }

    private void SaveDeck(int index_, DeckData deck_)
    {
        var _str = JsonUtility.ToJson(deck_);

        // 暗号化する処理
        var _data = System.Text.Encoding.UTF8.GetBytes(_str);

        JsonFileSystem.Save(GetDeckFilePath(index_), _str);
    }

    private bool LoadDeck(int index_, out DeckData deck_)
    {
        if (false == JsonFileSystem.Load(GetDeckFilePath(index_), out byte[] _data))
        {
            deck_ = null;
            return false;
        }

        // 暗号化から戻す処理
        var _str = System.Text.Encoding.UTF8.GetString(_data);

        deck_ = JsonUtility.FromJson<DeckData>(_str);
        return true;
    }

    private bool DeleteDeck(int index_)
    {
        return JsonFileSystem.Delete(GetDeckFilePath(index_));
    }

    private string GetDeckFilePath(int index_)
    {
        return Name.FilePath.FilePath_Deck + $"/Deck{index_}.json";
    }
}