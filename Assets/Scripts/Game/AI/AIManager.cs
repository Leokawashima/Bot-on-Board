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
            var _playerList = PlayerManager.Singleton.PlayerList;
            for (int i = 0, cnt = AI_SIZE; i < cnt; ++i)//人数分処理する　現在は2固定
            {
                var _oerator = _playerList[i];
                var _ai = Instantiate(m_prefab, transform);
                _ai.Spawn(_oerator, new Vector2Int(i * 9, i * 9));// 0,0 9,9に初期化している
                AIList.Add(_ai);

                if (i == 1)
                {
                    _ai.transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                _ai.Health.Event_Damage += (AIAgent ai_, float power_) =>
                {
                    GUIManager.Singleton.DamageEffect(ai_, power_);
                    GUIManager.Singleton.Refresh(ai_);
                };
                _ai.Health.Event_Heal += (AIAgent ai_, float power_) =>
                {
                    GUIManager.Singleton.HealEffect(ai_, power_);
                    GUIManager.Singleton.Refresh(ai_);
                };
                _ai.Brain.Event_IncreaseIntelligent += (AIAgent ai_, int value_) =>
                {
                    GUIManager.Singleton.InteliEffect(ai_, value_);
                    GUIManager.Singleton.Refresh(ai_);
                };
                _ai.Brain.Event_DecreaseIntelligent += (AIAgent ai_, int value_) =>
                {
                    GUIManager.Singleton.InteliEffect(ai_, value_);
                    GUIManager.Singleton.Refresh(ai_);
                };
            }
            GUIManager.Singleton.InitializeInfoPlayerData();
            m_cameraManager.Initialize();
        }

        // 誰か死んだ時点でtrueを返している
        public bool CheckAIIsDead()
        {
            foreach (var _ai in AIList)
            {
                if (_ai.Health.State == HealthState.Dead)
                {
                    return true;
                }
            }

            return false;
        }
        private bool CheckHitAI(AIAgent a_, AIAgent b_)
        {
            if (a_.Travel.Position == b_.Travel.Position) return true;

            if (a_.Travel.Position == b_.Travel.PrePosition && a_.Travel.PrePosition == b_.Travel.Position) return true;

            return false;
        }

        public void AIAction()
        {
            var _list = AIList;
            foreach (var ai in _list)
            {
                // 全員現在の状態から意思決定
                ai.Think();
            }

            foreach (var ai in _list)
            {
                // 意思決定後に行動
                ai.Action();
            }

            // AIが二体以上いないとインデックス漏れエラー
            for (int i = 0; i < _list.Count - 1; ++i)
            {
                for (int j = i + 1; j < _list.Count; ++j)
                {
                    if (CheckHitAI(_list[i], _list[i + j]))
                    {
                        _list[i].Travel.BackPosition();
                        _list[i].Health.Damage(0.5f);
                        _list[i + j].Travel.BackPosition();
                        _list[i + j].Health.Damage(0.5f);
                    }
                }
            }

            foreach (var ai in _list)
            {
                for (int i = 0; i < ai.Travel.Route.Count; ++i)
                {
                    MapManager.Singleton.AIRideCheck(ai.Travel.Route[i].Position, ai);
                    MapManager.Singleton.AIHitCheck(ai.Travel.Route[i].Position, ai);
                }
            }

            var _maxMoveCnt = 0;
            foreach (var ai in _list)
            {
                if (_maxMoveCnt < ai.Travel.Route.Count)
                    _maxMoveCnt = ai.Travel.Route.Count;
                StartCoroutine(ai.Travel.DelayMove());
            }

            StartCoroutine(Co_DelayMove());

            IEnumerator Co_DelayMove()
            {
                yield return new WaitForSeconds(_maxMoveCnt + 0.5f);
                Event_AiActioned?.Invoke();
            }
        }
    }
}