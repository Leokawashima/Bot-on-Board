using UnityEngine;

namespace Game
{
    public class GameModeMulti : GameMode_Template
    {
        public override void Initialize()
        {
            #region MultiItit
            /*
            //接続するまで待機をする間初期化をしておく
            //一定時間で戻りますか？のUIを出す

            //プレイヤー接続完了時のコールバック
            m_NetConnectManager.Event_PlayersConnected += () =>
            {
                m_RollButton.interactable = true;
                m_DiceSystem.gameObject.SetActive(true);
            };
            //プレイヤーが一人でも切断された時のコールバック
            m_NetConnectManager.Event_PlayersDisconnected += () =>
            {
                m_NetCautionUI.gameObject.SetActive(true);
            };

            m_RollButton.interactable = false;
            m_DiceSystem.gameObject.SetActive(false);

            m_HostButton.onClick.AddListener(() =>
            {
                m_NetConnectManager.Host();

                m_GameState = GameState.DecidedTheOrder;

                m_HostButton.interactable = false;
                m_ClientButton.interactable = false;
            });
            m_ClientButton.onClick.AddListener(() =>
            {
                m_NetConnectManager.Client();

                m_GameState = GameState.DecidedTheOrder;

                m_HostButton.interactable = false;
                m_ClientButton.interactable = false;
            });
            m_RollButton.onClick.AddListener(async () =>
            {
                m_RollButton.interactable = false;
                int _result = await m_DiceSystem.GetResult();
                _result = 2;
                m_Text.text = "Your Dice :" + _result;
                NetworkPlayerManager.m_Local.Roll.Value = _result;

                await Task.Delay(1000);
                m_DiceSystem.gameObject.SetActive(false);

                //ダイスを全員振るまで
                while(true)
                {
                    var _endFlag = true;
                    foreach(NetworkPlayerManager p in NetworkPlayerManager.m_List)
                    {
                        if(p.Roll.Value == 0) _endFlag = false;
                    }
                    if(_endFlag)
                    {
                        break;
                    }
                    await Task.Delay(100);
                }

                //ダイス目がかぶっていないかで分岐
                //人数が増えたら被ってない人と被った数値を比べて先に順番を決め、そのあと被りだけ振り直しのような処理にする
                if(NetworkPlayerManager.m_List[0].Roll.Value != NetworkPlayerManager.m_List[1].Roll.Value)
                {
                    //ダイス目のみのリストを作成、降順ソートを行う(昇順ソート＆反転)
                    var _orders = new List<int>();
                    foreach(NetworkPlayerManager p in NetworkPlayerManager.m_List)
                        _orders.Add(p.Roll.Value);
                    _orders.Sort();
                    _orders.Reverse();

                    //ダイス目とそのPlayerManagerのインスタンスを入れておくDictionaryを作成
                    //Keyにすることでこの後にMpalyerManager.m_Localで値をとれるのですっきり収まって個人的にすごい気に入っている
                    var _resultDictionary = new Dictionary<NetworkPlayerManager, int>();
                    foreach(NetworkPlayerManager p in NetworkPlayerManager.m_List)
                        _resultDictionary.Add(p, p.Roll.Value);

                    //自身のインスタンスだけデータにセット
                    // (Hostがデータ権限を持つとHostが過負荷になる＆設計的に書き込み権を各々のローカルにしか許容してない)
                    //Dictionaryからローカルインスタンスをキーに値を取り出し、降順ソートしたリストからその値でインデックスを確保
                    //...つまり大きい番号順に先攻にしている　わかりづらいよね
                    NetworkPlayerManager.m_Local.OrderIndex.Value = _orders.IndexOf(_resultDictionary[NetworkPlayerManager.m_Local]);

                    GameInitialize();
                }
                else
                {
                    NetworkPlayerManager.m_Local.ResetDice();
                    m_Text.text = "Reset";
                    m_DiceSystem.ResetDice();
                    m_RollButton.interactable = true;
                    m_DiceSystem.gameObject.SetActive(true);
                }
            PlayerManager.Singleton.Initialize();
            });
            */
            #endregion
        }
    }
}