using System;
using UnityEngine;
using UnityEngine.UI;
using Map;
using Map.Chip;

/// <summary>
/// 一人当たりのプレイヤーのUIを管理するクラス
/// </summary>
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] CardManager m_CardManager;
    [SerializeField] OrderManager m_OrderManager;
    [SerializeField] Button m_PlaceButton;
    [SerializeField] Button m_TurnEndButton;

    [SerializeField] int m_MaxOfPlace = 3;
    [SerializeField] int m_NumOfPlace = 0;
    [SerializeField] int m_AddOfPlace = 3;

    public event Action Event_ButtonPlace;
    public event Action Event_ButtonTurnEnd;

    public void Initialize()
    {
        m_NumOfPlace = m_MaxOfPlace;

        m_TurnEndButton.onClick.AddListener(OnButton_TurnEnd);
        m_PlaceButton.onClick.AddListener(OnButton_Place);

        m_CardManager.Initialize();
        m_OrderManager.Initialize();
    }

    void OnButton_Place()
    {
        // カードが選択されていないなら返す
        if (m_CardManager.GetSelectCard == null)
            return;

        // 選択チップ取得
        var _chip = LocalPlayerManager.Singleton.SelectChip;
        // ヌルなら返す　空中に置けるようにしたい場合選択方法から作らねばいけない
        if (_chip == null)
            return;

        // AIと被っていたら置けないとして返す　ものによってはAIに直接置けたらおもしろそう
        for (int i = 0; i < MapManager.Singleton.AIManagerList.Count; ++i)
        {
            if (MapManager.Singleton.AIManagerList[i].Position == _chip.Position)
                return;
        }

        // オブジェクトがあるなら返す　ハカイ爆弾などのためにこれは別途方法を考えないといけない
        if (MapManager.Singleton.MapState.MapObjects[_chip.Position.y][_chip.Position.x] != null)
            return;

        var _mo = MapManager.Singleton.ObjectSpawn(m_CardManager.GetSelectCard.SO, _chip);
        _mo.Initialize(MapManager.Singleton);
        m_CardManager.GetSelectCard.Trash();

        Event_ButtonPlace?.Invoke();

        // 置ける枚数を1減らす　カードによって減る量を変える場合これを変えて残り置ける枚数と必要量を比較しなければいけない
        m_NumOfPlace--;

        if (m_NumOfPlace <= 0 || m_CardManager.HandCard.Count == 0)
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
        m_CardManager.Draw();
    }
}
