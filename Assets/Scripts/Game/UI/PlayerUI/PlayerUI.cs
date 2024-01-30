using System;
using UnityEngine;
using UnityEngine.UI;
using Map;
using Map.Object.Component;
using Bot;
using Player;
using Deck;

/// <summary>
/// 一人当たりのプレイヤーのUIを管理するクラス
/// </summary>
public class PlayerUI : MonoBehaviour
{
    private PlayerAgent m_operator;

    [SerializeField] Canvas m_canvas;

    [SerializeField] private CardManager m_cardManager;
    [SerializeField] private InfoBotOrderManager m_orderManager;
    [SerializeField] private Button m_placeButton;
    [SerializeField] private Button m_turnEndButton;

    [SerializeField] private int m_MaxOfPlace = 3;
    [SerializeField] private int m_NumOfPlace = 0;
    [SerializeField] private int m_AddOfPlace = 3;

    public event Action
        Event_ButtonPlace,
        Event_ButtonTurnEnd;

    [SerializeField] DeckData_SO m_deck;

    public void Enable() => m_canvas.enabled = true;
    public void Disable() => m_canvas.enabled = false;

    public void Initialize(PlayerAgent operator_, DeckData deck_)
    {
        m_operator = operator_;
        name = $"PlayerUI_{m_operator.Index}";

        m_NumOfPlace = m_MaxOfPlace;

        m_turnEndButton.onClick.AddListener(OnButton_TurnEnd);
        m_placeButton.onClick.AddListener(OnButton_Place);

        if (deck_ != null && deck_.Cards != null && deck_.Cards.Count >= 6)
        {
            m_cardManager.Initialize(deck_);
        }
        else
        {
            m_cardManager.Initialize(m_deck.Deck);
        }
        m_orderManager.Initialize();
    }

    void OnButton_Place()
    {
        // カードが選択されていないなら返す
        if (m_cardManager.SelectCard == null)
        {
            return;
        }

        // 選択チップ取得
        var _chip = LocalPlayerManager.Singleton.SelectChip;
        // ヌルなら返す　空中に置けるようにしたい場合選択方法から作らねばいけない
        if (_chip == null)
        {
            return;
        }

        // AIと被っていたら置けないとして返す　ものによってはAIに直接置けたらおもしろそう
        foreach (var bot in BotManager.Singleton.Bots)
        {
            if (bot.Travel.Position == _chip.Position)
            {
                return;
            }
        }

        // オブジェクトがあるなら返す　ハカイ爆弾などのためにこれは別途方法を考えないといけない
        var _obj = MapManager.Singleton.Stage.Object[_chip.Position.y][_chip.Position.x];
        if (_obj != null)
        {
            if (false == m_cardManager.SelectCard.SO.CanOverrideSpawn)
            {
                return;
            }
        }

        var _mo = MapManager.Singleton.ObjectSpawn(m_cardManager.SelectCard.SO, _chip);
        m_cardManager.SelectCard.Trash();

        Event_ButtonPlace?.Invoke();

        // 置ける枚数を1減らす　カードによって減る量を変える場合これを変えて残り置ける枚数と必要量を比較しなければいけない
        m_NumOfPlace--;

        if (m_NumOfPlace <= 0 || m_cardManager.HandCards.Count == 0)
        {
            OnButton_TurnEnd();
        }
    }

    void OnButton_TurnEnd()
    {
        Disable();

        Event_ButtonTurnEnd?.Invoke();
    }

    public void TurnInitialize()
    {
        m_NumOfPlace = Mathf.Min(m_NumOfPlace + m_AddOfPlace, m_MaxOfPlace);
        m_cardManager.Replenish();
    }
}
