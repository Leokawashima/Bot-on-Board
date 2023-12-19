using System;
using UnityEngine;
using UnityEngine.UI;
using Map;
using AI;

/// <summary>
/// 一人当たりのプレイヤーのUIを管理するクラス
/// </summary>
public class PlayerUI : MonoBehaviour
{
    [SerializeField] CardManager m_cardManager;
    [SerializeField] InfoAIOrderManager m_orderManager;
    [SerializeField] Button m_placeButton;
    [SerializeField] Button m_turnEndButton;

    [SerializeField] int m_MaxOfPlace = 3;
    [SerializeField] int m_NumOfPlace = 0;
    [SerializeField] int m_AddOfPlace = 3;

    public event Action Event_ButtonPlace;
    public event Action Event_ButtonTurnEnd;

    [SerializeField] DeckData_SO m_deck;

    public void Initialize()
    {
        m_NumOfPlace = m_MaxOfPlace;

        m_turnEndButton.onClick.AddListener(OnButton_TurnEnd);
        m_placeButton.onClick.AddListener(OnButton_Place);

        m_cardManager.Initialize(m_deck.Deck);
        m_orderManager.Initialize();
    }

    void OnButton_Place()
    {
        // カードが選択されていないなら返す
        if (m_cardManager.GetSelectCard == null)
            return;

        // 選択チップ取得
        var _chip = LocalPlayerManager.Singleton.SelectChip;
        // ヌルなら返す　空中に置けるようにしたい場合選択方法から作らねばいけない
        if (_chip == null)
            return;

        // AIと被っていたら置けないとして返す　ものによってはAIに直接置けたらおもしろそう
        foreach (var ai in AIManager.Singleton.AIList)
        {
            if (ai.Position == _chip.Position)
                return;
        }

        // オブジェクトがあるなら返す　ハカイ爆弾などのためにこれは別途方法を考えないといけない
        if (MapManager.Singleton.Stage.Object[_chip.Position.y][_chip.Position.x] != null)
            return;

        var _mo = MapManager.Singleton.ObjectSpawn(m_cardManager.GetSelectCard.SO, _chip);
        m_cardManager.GetSelectCard.Trash();

        Event_ButtonPlace?.Invoke();

        // 置ける枚数を1減らす　カードによって減る量を変える場合これを変えて残り置ける枚数と必要量を比較しなければいけない
        m_NumOfPlace--;

        if (m_NumOfPlace <= 0 || m_cardManager.HandCardList.Count == 0)
        {
            OnButton_TurnEnd();
        }
    }

    void OnButton_TurnEnd()
    {
        gameObject.SetActive(false);

        Event_ButtonTurnEnd?.Invoke();
    }

    public void TurnInitialize()
    {
        m_NumOfPlace = Mathf.Min(m_NumOfPlace + m_AddOfPlace, m_MaxOfPlace);
        m_cardManager.Draw();
    }
}
