using System;
using System.Collections.Generic;
using UnityEngine;
using MyFileSystem;
using TMPro;

public class DeckEditingManager : MonoBehaviour
{
    [SerializeField]
    private MapObjectTable_SO m_mapObjectTable;

    private const int PRESET_DECK_SIZE = 10;

    private List<Deck> m_presetDeckList = new(PRESET_DECK_SIZE);

    private Deck m_deckEdit;

    // Flags属性は例えばToStringしたときに数値出力せずに各々の状態に当てはまるか列挙してくれる
    [Flags]
    private enum State
    {
        Non = 0,
        isRarilyLimit = 1 << 0,
        isSameBan = 1 << 1,
    }
    private int GetState(State state_) { return (int)state_; }

    [ContextMenu("aaa")]
    private void aaa()
    {
        var test = new Deck()
        {
            DeckName = "test",
            State = GetState(State.isRarilyLimit),
            MaxSize = 10,
        };
        test.CardIndexList.AddRange(new int[10] {0,0,0,0,0,0,0,0,0,0});

        SaveDeck(0, test);
    }

    [ContextMenu("bbb")]
    private void bbb()
    {
        LoadDeck(0, out Deck _deck);
        Debug.Log(_deck);
    }

    private void OpenDeckList()
    {
        m_presetDeckList = new(PRESET_DECK_SIZE);

        for(int i = 0; i < PRESET_DECK_SIZE; ++i)
        {
            if(LoadDeck(i, out Deck _deck))
            {
                m_presetDeckList[i] = _deck;
            }
        }
    }

    private void CloseDeckList()
    {
        m_presetDeckList.Clear();
    }

    private void SaveDeck(int index_, Deck deck_)
    {
        var _str = JsonUtility.ToJson(deck_);

        // 暗号化する処理
        var _data = System.Text.Encoding.UTF8.GetBytes(_str);

        JsonFileSystem.Save(GetDeckFilePath(index_), _str);
    }

    private bool LoadDeck(int index_, out Deck deck_)
    {
        if (false == JsonFileSystem.Load(GetDeckFilePath(index_), out byte[] _data))
        {
            deck_ = null;
            return false;
        }

        // 暗号化から戻す処理
        var _str = System.Text.Encoding.UTF8.GetString(_data);

        deck_ = JsonUtility.FromJson<Deck>(_str);
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