using System;
using UnityEngine;
using UnityEngine.UI;
using MyFileSystem;

public class DeckListManager : MonoBehaviour
{
    private const int PRESET_DECK_SIZE = 10;

    [SerializeField] DeckManager m_deckManager;

    public InfoDeckData SelectInfo { get; private set; }

    [SerializeField] private RectTransform m_content;
    [SerializeField] private InfoDeckData m_prefab;
    [SerializeField] private float m_position = -85.0f;
    [SerializeField] private float m_offset = -160.0f;

    private const string DECK_NAME = "Deck";

    // デフォルトのデータをいれるためだけのSOをいれている
    [SerializeField] private DeckData_SO m_deckData;

    [SerializeField] DeckListInfo m_deckListInfo;

    [SerializeField] private Button m_editButton;
    [SerializeField] private Button m_deleteButton;

    public static event Action<InfoDeckData> Event_EditOpen;

    public void Enable()
    {
        gameObject.SetActive(true);
    }
    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void Initialize()
    {
        Enable();

        m_editButton.onClick.AddListener(() =>
        {
            if (SelectInfo != null)
            {
                Disable();
                m_deckManager.DeckEdit.Enable();
                Event_EditOpen?.Invoke(SelectInfo);
            }
        });

        m_deleteButton.onClick.AddListener(() =>
        {
            if (SelectInfo != null)
            {
                DeleteDeck(SelectInfo.Index);
                if (SelectInfo.Index == 0)
                {
                    SelectInfo.Initialize(0, m_deckData.Deck.DeepCopyInstance(), OnClickInfo);
                }
                else
                {
                    SelectInfo.Initialize(SelectInfo.Index, new()
                    {
                        Name = DECK_NAME + (SelectInfo.Index + 1)
                    }, OnClickInfo);
                }
                m_deckListInfo.SetInfo(SelectInfo);
            }
        });

        OpenDeckList();
    }

    private void OpenDeckList()
    {
        // 最初のデッキデータは空になってほしくない為にデータがない場合既存のデータから読み取る
        {
            if(OpenDeck(0, out var _deck, out var _info))
            {
                _info.Initialize(0, _deck, OnClickInfo);
            }
            else
            {
                _info.Initialize(0, m_deckData.Deck.DeepCopyInstance(), OnClickInfo);
            }
            m_deckListInfo.SetInfo(_info);
        }

        // 2～10は空でも良いため空のデータにする
        for(int i = 1; i < PRESET_DECK_SIZE; ++i)
        {
            if (OpenDeck(i, out var _deck, out var _info))
            {
                _info.Initialize(i, _deck, OnClickInfo);
            }
            else
            {
                _info.Initialize(i, new()
                {
                    Name = DECK_NAME + (i + 1)
                }, OnClickInfo);
            }
        }
    }

    private void OnClickInfo(InfoDeckData info_)
    {
        SelectInfo = info_;
        m_deckListInfo.SetInfo(info_);
    }

    private bool OpenDeck(int index_, out DeckData deck_, out InfoDeckData info_)
    {
        info_ = Instantiate(m_prefab, m_content);
        var _rect = info_.transform as RectTransform;
        _rect.anchoredPosition = new(_rect.anchoredPosition.x, m_position + index_ * m_offset);

        return LoadDeck(index_, out deck_);
    }

    public void Save(int index_, DeckData deck_)
    {
        SaveDeck(index_, deck_);
    }

    private void SaveDeck(int index_, DeckData deck_)
    {
        var _str = JsonUtility.ToJson(deck_);

        // 暗号化する処理

        JsonFileSystem.Save(GetDeckFilePath(index_), _str);
    }

    private bool LoadDeck(int index_, out DeckData deck_)
    {
        if(false == JsonFileSystem.Load(GetDeckFilePath(index_), out string _str))
        {
            deck_ = null;
            return false;
        }

        // 暗号化から戻す処理

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
