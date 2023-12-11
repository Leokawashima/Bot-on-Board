using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

public class AIManager : MonoBehaviour
{
    public static AIManager Singleton { get; private set; }

    [SerializeField] AISystem m_AIPrefab;
#if UNITY_EDITOR
    [Header("Debug"), SerializeField]
#endif
    List<AISystem> m_AIList = new();

    [SerializeField] AICameraTest m_AICameraTest;

    public List<AISystem> AIList => m_AIList;

    public static event Action Event_AiActioned;

    private void OnEnable()
    {
        GUIManager.Event_AICutInFinish += AIAction;
    }
    private void OnDisable()
    {
        GUIManager.Event_AICutInFinish -= AIAction;
    }

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
            if (i == 1)
            {
                _ai.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            _ai.Event_DamageHP += (AISystem ai_, float damage_) =>
            {
                GUIManager.Singleton.DamageEffect(ai_, damage_);
                GUIManager.Singleton.OnSetHPText(ai_.Index, ai_.m_HP);
            };
            _ai.Event_HealHP += (AISystem ai_, float heal) =>
            {
                GUIManager.Singleton.OnSetHPText(ai_.Index, ai_.m_HP);
            };
        }
        GUIManager.Singleton.InitializeAIHPUI();
        m_AICameraTest.Initialize();
    }

    public bool CheckAIIsDead()//誰か死んだ時点でtrueを返している
    {
        foreach(var _ai in m_AIList)
        {
            if(_ai.AIAliveState == AISystem.AliveState.Dead)
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

        foreach(var _ai in m_AIList)//意思決定後に行動
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
            MapManager.Singleton.AIRideChip(_ai.Position, _ai);
            MapManager.Singleton.AIHitObject(_ai.Position, _ai);
        }

        StartCoroutine(Co_DelayMove());
        
        IEnumerator Co_DelayMove()
        {
            for(int i = 1; i <= 10; ++i)
            {
                foreach(var _ai in m_AIList)
                {
                    Vector2 prepos = _ai.PrePosition;
                    Vector2 pos = _ai.Position;
                    Vector2 _offset = (pos - prepos) * i / 10.0f;
                    _ai.transform.localPosition = new Vector3(_ai.PrePosition.x, 1, _ai.PrePosition.y) + new Vector3(_offset.x, 0, _offset.y) + MapManager.Singleton.Offset;
                }
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(0.5f);
            Event_AiActioned?.Invoke();
        }
    }
}