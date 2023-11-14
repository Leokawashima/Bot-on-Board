using System.Collections.Generic;
using UnityEngine;
using MyFileSystem;

public class DeckEditingManager : MonoBehaviour
{
    [SerializeField]
    private MapObjectTable_SO m_mapObjectTable;

    private List<Deck> m_presetDeckList;

    private Deck m_deckEdit;

    private readonly string m_FILEPATH = Name.Setting.FilePath_Deck + "/Deck.json";

    private void OpenDeck()
    {
        m_presetDeckList = new();

        // ロード処理
    }

    private void CloseDeck()
    {

    }

    private void SaveDeck()
    {
        // 文字列を暗号化する処理～～
        string _data = "a";
        JsonFileSystem.Save(m_FILEPATH, _data);
    }

    private void LoadDeck()
    {
        if (false == JsonFileSystem.Load(m_FILEPATH, out string _data))
        {
            return;
        }
        // 文字列を暗号化から戻す処理～～
    }
}