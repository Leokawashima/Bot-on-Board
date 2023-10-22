using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public static AIManager Singleton { get; private set; }

    [SerializeField] AISystem m_AIPrefab;
#if UNITY_EDITOR
    [Header("Debug"), SerializeField]
#endif
    List<AISystem> m_AIList = new();

    void Awake()
    {
        Singleton ??= this;
    }
    void OnDestroy()
    {
        Singleton = null;
    }

    public void Initialize()
    {
        for(int i = 0; i < 2; ++i)//人数分処理する　現在は2固定
        {
            var _ai = Instantiate(m_AIPrefab, transform);
            _ai.Spawn(i, $"AI:{i}", new Vector2Int(i * 9, i * 9));// 0,0 9,9に初期化している
            m_AIList.Add(_ai);
            _ai.Event_DamageHP += (int index_, float hp_) =>
            {
                GUIManager.Singleton.OnSetHPText(index_, hp_);
            };
            _ai.Event_HealHP += (int index_, float hp_) =>
            {
                GUIManager.Singleton.OnSetHPText(index_, hp_);
            };
        }
    }

    public bool CheckAIIsDead()//誰か死んだ時点でtrueを返しているので人数が増えた場合
    {
        foreach(var _ai in m_AIList)
        {
            if(_ai.m_State == AISystem.State.Dead)
            {
                return true;
            }
        }

        return false;
    }

    public void AIAction()
    {
        foreach(var _ai in m_AIList)//全員現在の状態から意思決定
            _ai.Think();

        foreach(var _ai in m_AIList)//前意思決定後に行動
            _ai.Action();

        //以下AI2体前提処理　時間が足りないのでこのまま
        var _isHit = false;
        if(m_AIList[0].Position == m_AIList[1].Position) _isHit = true;//完全に同一のマスにいる
        if(m_AIList[0].Position == m_AIList[1].PrePosition)//[0]が[1]の移動前にいた場所にいる
        {
            if(m_AIList[0].PrePosition == m_AIList[1].Position)//[1]が[0]の移動前にいた場所にいる
                _isHit = true;//つまりすれ違っているのでHit判定
        }

        if(_isHit)
        {
            for(int i = 0; i < 2; ++i)
            {
                m_AIList[i].BackPosition();
                m_AIList[i].DamageHP(0.5f);
                //接触ダメージ直入れ　アモアスのように設定できるようにしたい
            }
        }
        //ここまでAI2体前提処理

        foreach(var _ai in m_AIList)
        {
            MapManager.Singleton.AIHitObject(_ai.Position, _ai);
        }
    }
}