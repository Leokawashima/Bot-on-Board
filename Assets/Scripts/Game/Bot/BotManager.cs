using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;
using Map.Object.Component;
using Player;

namespace Bot
{
    public class BotManager : SingletonMonoBehaviour<BotManager>
    {
        [SerializeField] BotAgent m_prefab;

#if UNITY_EDITOR
        [field: Header("Debug"), SerializeField]
#endif
        public List<BotAgent> Bots { get; private set; }

        public static event Action Event_BotsActioned;

        public void Initialize()
        {
            Bots = new();
            var _playerList = PlayerManager.Singleton.Players;
            for (int i = 0, cnt = _playerList.Count; i < cnt; ++i)
            {
                var _oerator = _playerList[i];
                var _botSetting = _oerator.Setting.BotSettings;
                for (int j = 0; j < _botSetting.Count; ++j)
                {
                    var _bot = Instantiate(m_prefab, transform);
                    _bot.Initialize(_oerator, _botSetting[j], new Vector2Int(i * 9, i * 9));// 0,0 9,9に初期化している
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
                    _bot.Assault.Event_HoldWeapon += (BotAgent bot_, Weapon weapon_) =>
                    {
                        GUIManager.Singleton.Refresh(bot_);
                    };
                    _bot.Assault.Event_ReleaceWeapon += (BotAgent bot_) =>
                    {
                        GUIManager.Singleton.Refresh(bot_);
                    };
                }
            }
            GUIManager.Singleton.InitializeInfoPlayerData();
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
            }

            StartCoroutine(Co_DelayMove());

            IEnumerator Co_DelayMove()
            {
                for (int i = 0, cnt = _maxMoveCnt; i < cnt; ++i)
                {
                    foreach (var bot in _bots)
                    {
                        if (bot.Travel.Routes.Count > i)
                        {
                            StartCoroutine(bot.Travel.DelayMove(i));
                        }
                    }

                    // Botが二体以上いないとインデックス漏れエラー
                    for (int j = 0; j < _bots.Count - 1; ++j)
                    {
                        for (int k = j + 1; k < _bots.Count; ++k)
                        {
                            if (CheckBotHit(_bots[j], _bots[j + k]))
                            {
                                _bots[j].Travel.BackPosition();
                                _bots[j].Health.Damage(0.5f);
                                _bots[j + k].Travel.BackPosition();
                                _bots[j + k].Health.Damage(0.5f);
                            }
                        }
                    }
                    
                    yield return new WaitForSeconds(1.0f);
                }
                yield return new WaitForSeconds(1.0f);
                Event_BotsActioned?.Invoke();
            }
        }
    }
}