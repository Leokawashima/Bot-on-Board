using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;
using Player;

namespace AI
{
    public class AIManager : SingletonMonoBehaviour<AIManager>
    {
        public const int AI_SIZE = 2;

        [SerializeField] AIAgent m_prefab;
        [SerializeField] AICameraManager m_cameraManager;

#if UNITY_EDITOR
        [field: Header("Debug"), SerializeField]
#endif
        public List<AIAgent> AIList { get; private set; } = new();

        public static event Action Event_AiActioned;

        private void OnEnable()
        {
            GUIManager.Event_AICutInFinish += AIAction;
        }
        private void OnDisable()
        {
            GUIManager.Event_AICutInFinish -= AIAction;
        }

        public void Initialize()
        {
            for (int i = 0; i < AI_SIZE; ++i)//人数分処理する　現在は2固定
            {
                var _oerator = PlayerManager.Singleton.PlayerList[i];
                var _ai = Instantiate(m_prefab, transform);
                _ai.Spawn(_oerator, new Vector2Int(i * 9, i * 9));// 0,0 9,9に初期化している
                AIList.Add(_ai);
                MapManager.Singleton.AIManagerList.Add(_ai);

                if (i == 1)
                {
                    _ai.transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                _ai.Event_DamageHP += (AIAgent ai_, float damage_) =>
                {
                    GUIManager.Singleton.DamageEffect(ai_, damage_);
                    GUIManager.Singleton.OnSetHPText(ai_.Operator.Index, ai_.HP);
                };
                _ai.Event_HealHP += (AIAgent ai_, float heal) =>
                {
                    GUIManager.Singleton.OnSetHPText(ai_.Operator.Index, ai_.HP);
                };
            }
            GUIManager.Singleton.InitializeAIHPUI();
            m_cameraManager.Initialize();
        }

        public bool CheckAIIsDead()//誰か死んだ時点でtrueを返している
        {
            foreach (var _ai in AIList)
            {
                if (_ai.AIAliveState == AliveState.Dead)
                {
                    return true;
                }
            }

            return false;
        }

        public void AIAction()
        {
            foreach (var _ai in AIList)//全員現在の状態から意思決定
                _ai.Think();

            foreach (var _ai in AIList)//意思決定後に行動
                _ai.Action();

            //以下AI2体前提処理　時間が足りないのでこのまま
            var _isHit = false;
            if (AIList[0].Position == AIList[1].Position) _isHit = true;//完全に同一のマスにいる
            if (AIList[0].Position == AIList[1].PrePosition)//[0]が[1]の移動前にいた場所にいる
            {
                if (AIList[0].PrePosition == AIList[1].Position)//[1]が[0]の移動前にいた場所にいる
                    _isHit = true;//つまりすれ違っているのでHit判定
            }

            if (_isHit)
            {
                for (int i = 0; i < AI_SIZE; ++i)
                {
                    AIList[i].BackPosition();
                    AIList[i].Damage(0.5f);
                    //接触ダメージ直入れ　アモアスのように設定できるようにしたい
                }
            }
            //ここまでAI2体前提処理

            foreach (var ai in AIList)
            {
                for (int i = 0; i < ai.Move.Path.Count; ++i)
                {
                    MapManager.Singleton.AIRideCheck(ai.Move.Path[i].Position, ai);
                    MapManager.Singleton.AIHitCheck(ai.Move.Path[i].Position, ai);
                }
            }

            foreach (var ai in AIList)
            {
                StartCoroutine(ai.DelayMove());
            }

            StartCoroutine(Co_DelayMove());

            IEnumerator Co_DelayMove()
            {
                yield return new WaitForSeconds(1.5f);
                Event_AiActioned?.Invoke();
            }
        }
    }
}