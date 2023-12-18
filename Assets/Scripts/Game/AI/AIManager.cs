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

        //誰か死んだ時点でtrueを返している
        public bool CheckAIIsDead()
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
        private bool CheckHitAI(AIAgent a_, AIAgent b_)
        {
            if (a_.Position == b_.Position) return true;

            if (a_.Position == b_.PrePosition && a_.PrePosition == b_.Position) return true;

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
                //意思決定後に行動
                ai.Action();
            }

            // AIが二体以上いないとインデックス漏れエラー
            for (int i = 0; i < _list.Count - 1; ++i)
            {
                for (int j = i + 1; j < _list.Count; ++j)
                {
                    if (CheckHitAI(_list[i], _list[i + j]))
                    {
                        _list[i].BackPosition();
                        _list[i].Damage(0.5f);
                        _list[i + j].BackPosition();
                        _list[i + j].Damage(0.5f);
                    }
                }
            }

            foreach (var ai in _list)
            {
                for (int i = 0; i < ai.Move.Path.Count; ++i)
                {
                    MapManager.Singleton.AIRideCheck(ai.Move.Path[i].Position, ai);
                    MapManager.Singleton.AIHitCheck(ai.Move.Path[i].Position, ai);
                }
            }

            var _maxMoveCnt = 0;
            foreach (var ai in _list)
            {
                if (_maxMoveCnt < ai.Move.Count)
                    _maxMoveCnt = ai.Move.Count;
                StartCoroutine(ai.DelayMove());
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