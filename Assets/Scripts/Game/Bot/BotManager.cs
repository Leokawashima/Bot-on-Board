using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;
using Player;

namespace Bot
{
    public class BotManager : SingletonMonoBehaviour<BotManager>
    {
        public const int Bot_SIZE = 2;

        [SerializeField] BotAgent m_prefab;
        [SerializeField] BotCameraManager m_cameraManager;

#if UNITY_EDITOR
        [field: Header("Debug"), SerializeField]
#endif
        public List<BotAgent> Bots { get; private set; } = new();

        public static event Action Event_BotsActioned;

        private void OnEnable()
        {
            GUIManager.Event_BotCutInFinish += Action;
        }
        private void OnDisable()
        {
            GUIManager.Event_BotCutInFinish -= Action;
        }

        public void Initialize()
        {
            var _playerList = PlayerManager.Singleton.PlayerList;
            for (int i = 0, cnt = Bot_SIZE; i < cnt; ++i)//人数分処理する　現在は2固定
            {
                var _oerator = _playerList[i];
                var _bot = Instantiate(m_prefab, transform);
                _bot.Spawn(_oerator, new Vector2Int(i * 9, i * 9));// 0,0 9,9に初期化している
                Bots.Add(_bot);

                if (i == 1)
                {
                    _bot.transform.rotation = Quaternion.Euler(0, 180, 0);
                }

                _bot.Health.Event_Damage += (BotAgent bot_, float power_) =>
                {
                    GUIManager.Singleton.DamageEffect(bot_, power_);
                    GUIManager.Singleton.Refresh(bot_);
                };
                _bot.Health.Event_Heal += (BotAgent bot_, float power_) =>
                {
                    GUIManager.Singleton.HealEffect(bot_, power_);
                    GUIManager.Singleton.Refresh(bot_);
                };
                _bot.Brain.Event_IncreaseIntelligent += (BotAgent bot_, int value_) =>
                {
                    GUIManager.Singleton.InteliEffect(bot_, value_);
                    GUIManager.Singleton.Refresh(bot_);
                };
                _bot.Brain.Event_DecreaseIntelligent += (BotAgent bot_, int value_) =>
                {
                    GUIManager.Singleton.InteliEffect(bot_, value_);
                    GUIManager.Singleton.Refresh(bot_);
                };
            }
            GUIManager.Singleton.InitializeInfoPlayerData();
            m_cameraManager.Initialize();
        }

        // 誰か死んだ時点でtrueを返している
        public bool CheckBotDead()
        {
            foreach (var bot in Bots)
            {
                if (bot.Health.State == HealthState.Dead)
                {
                    return true;
                }
            }

            return false;
        }
        private bool CheckBotHit(BotAgent first_, BotAgent second_)
        {
            if (first_.Travel.Position == second_.Travel.Position) return true;

            if (first_.Travel.Position == second_.Travel.PrePosition && first_.Travel.PrePosition == second_.Travel.Position) return true;

            return false;
        }

        public void Action()
        {
            var _bots = Bots;
            foreach (var bot in _bots)
            {
                // 全員現在の状態から意思決定
                bot.Think();
            }

            foreach (var bot in _bots)
            {
                // 意思決定後に行動
                bot.Action();
            }

            // Botが二体以上いないとインデックス漏れエラー
            for (int i = 0; i < _bots.Count - 1; ++i)
            {
                for (int j = i + 1; j < _bots.Count; ++j)
                {
                    if (CheckBotHit(_bots[i], _bots[i + j]))
                    {
                        _bots[i].Travel.BackPosition();
                        _bots[i].Health.Damage(0.5f);
                        _bots[i + j].Travel.BackPosition();
                        _bots[i + j].Health.Damage(0.5f);
                    }
                }
            }

            foreach (var bot in _bots)
            {
                for (int i = 0; i < bot.Travel.Routes.Count; ++i)
                {
                    MapManager.Singleton.AIRideCheck(bot.Travel.Routes[i].Position, bot);
                    MapManager.Singleton.AIHitCheck(bot.Travel.Routes[i].Position, bot);
                }
            }

            var _maxMoveCnt = 0;
            foreach (var bot in _bots)
            {
                if (_maxMoveCnt < bot.Travel.Routes.Count)
                {
                    _maxMoveCnt = bot.Travel.Routes.Count;
                }
                StartCoroutine(bot.Travel.DelayMove());
            }

            StartCoroutine(Co_DelayMove());

            IEnumerator Co_DelayMove()
            {
                yield return new WaitForSeconds(_maxMoveCnt + 1.0f);
                Event_BotsActioned?.Invoke();
            }
        }
    }
}