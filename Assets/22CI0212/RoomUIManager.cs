using System;
using System.Net;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Net.Sockets;

/// <summary>
/// RoomのUIを管理するクラス
/// </summary>
public class RoomUIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject selectUI;
    [SerializeField] GameObject makeRoomUI;
    [SerializeField] GameObject listUI;
    [SerializeField] GameObject logUI;
    [SerializeField] GameObject hostUI;
    [SerializeField] GameObject clientUI;
    [SerializeField] GameObject connectUI;
    [Header("アタッチ")]
    [SerializeField] RoomManager roomManager;
    [SerializeField] RoomLogManager roomLog;
    [SerializeField] RoomListManager roomList;
    [SerializeField] RoomConnectManager roomConnect;
    public TMP_InputField nameText;

    public enum UIState { Select, MakeRoom, Host, Client, ConnectSelect, ConnectBack }

    void Start()
    {
        SetUI(UIState.Select);

        roomList.Initialize();
    }

    public void OnClick_Host()
    {
        SetUI(UIState.MakeRoom);
    }
    public void OnClick_HostStart()
    {
        SetUI(UIState.Host);

        roomManager.Host();

        roomLog.LogPush("Host Started");
    }
    public void Onclick_HostBack()
    {
        SetUI(UIState.Select);
    }
    public void OnClick_Client()
    {
        SetUI(UIState.Client);

        roomManager.Client();

        roomLog.LogPush("Client Started");
    }
    public void OnClick_SelectInfo()
    {
        SetUI(UIState.ConnectSelect);

        roomLog.LogPush("Connect Require"); 
    }
    public void OnClick_ConnectBack()
    {
        SetUI(UIState.Client);
    }
    public void OnClick_ConnectStart()
    {
        SetUI(UIState.Client);

        roomManager.clientState = RoomManager.ClientState.ConnectRequest;

        roomLog.LogPush("Client Started");
    }
    public void OnClick_Quit()
    {
        SetUI(UIState.Select);

        roomManager.Close();

        roomLog.LogClear();

        roomList.Clear();
    }
    public void OnClick_GameStart()
    {

    }
    public void OnClick_GameReady()
    {

    }

    public void SetUI(UIState state_)
    {
        switch (state_)
        {
            case UIState.Select:
                selectUI.SetActive(true);
                makeRoomUI.SetActive(false);
                listUI.SetActive(false);
                logUI.SetActive(false);
                hostUI.SetActive(false);
                clientUI.SetActive(false);
                connectUI.SetActive(false);
                break;
            case UIState.MakeRoom:
                selectUI.SetActive(false);
                makeRoomUI.SetActive(true);
                break;
            case UIState.Host:
                makeRoomUI.SetActive(false);
                hostUI.SetActive(true);
                listUI.SetActive(true);
                logUI.SetActive(true);
                break;
            case UIState.Client:
                selectUI.SetActive(false);
                listUI.SetActive(true);
                logUI.SetActive(true);
                clientUI.SetActive(true);
                break;
            case UIState.ConnectSelect:
                connectUI.SetActive(true);
                break;
            case UIState.ConnectBack:
                connectUI.SetActive(false);
                break;
        }
    }
}