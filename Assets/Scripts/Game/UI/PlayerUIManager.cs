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
            if(MapManager.Singleton.m_ObjStates[_chip.m_Pos.y, _chip.m_Pos.x] == -1)
            {
                var _cost = 0; //仮実装しないと提出できないのでマップのコストをここで設定
                               //本実装では　オブジェクト依存のコスト設定　賢さに応じての倍率補正設定　＆図鑑ナンバーのようなインデックスが必須
                               //ナンバーどうするか...　名前順でもいいしモンストみたいに登録順でもいいし悩み
                switch(m_CardManager.GetSelectCard.m_Index)
                {
                    case 0: _cost = 10000; break;
                    case 1: _cost = 50; break;
                    case 2: _cost = -50; break;
                    case 3: _cost = -100; break;
                    case 4: _cost = 10000; break;
                }
                MapManager.Singleton.SetObjState(_chip.m_Pos, _cost);

                m_CardManager.GetSelectCard.ObjectSpawn(_chip, MapManager.Singleton);
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
