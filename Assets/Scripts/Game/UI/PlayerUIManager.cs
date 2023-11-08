using System;
using UnityEngine;
using UnityEngine.UI;

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
        //カードが選択されていないなら返す
        if(m_CardManager.GetSelectCard == null) return;

        //ローカルインスタンスに選択チップを取得しに行く　マルチでも生成されるのは一つのインスタンスなので破綻しない
        var _chip = LocalPlayerManager.Singleton.m_SelectChip;
        if(_chip != null)
        {
            if(MapManager.Singleton.MapState.MapObjectState[_chip.m_position.y, _chip.m_position.x] == -1)
            {
                for (int i = 0; i < MapManager.Singleton.m_AIManagerList.Count; ++i)
                {
                    if (MapManager.Singleton.m_AIManagerList[i].Position == _chip.m_position)
                        return;
                }
                var _mo = m_CardManager.GetSelectCard.ObjectSpawn(_chip, MapManager.Singleton);
                _mo.Initialize(MapManager.Singleton);
                m_CardManager.GetSelectCard.Trash();

                Event_ButtonPlace?.Invoke();

                if(--m_NumOfPlace == 0 || m_CardManager.m_HandCardList.Count == 0)
                    OnButton_TurnEnd();
            }
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
