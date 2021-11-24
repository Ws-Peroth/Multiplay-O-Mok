using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameUiManager : Singleton<GameUiManager>
{
    #region 멤버 변수 & 프로퍼티
    [SerializeField] private Text resultText;
    [SerializeField] private Text winnerNameText;
    [SerializeField] private Text blackAreaText;
    [SerializeField] private Text whiteAreaText;
    [SerializeField] private Text playerNameText;
    [SerializeField] private Text otherNameText;
    [SerializeField] private Text blackPlayerNameText;
    [SerializeField] private Text whitePlayerNameText;
    [SerializeField] private Text readyButtonText;
    [SerializeField] private Toggle playerReadyStatus;
    [SerializeField] private Toggle otherPlayerReadyStatus;
    [SerializeField] private GameObject readyPopup;
    [SerializeField] private GameObject resultPopup;
    public string BlackAreaText { set => blackAreaText.text = value; }
    public string WhiteAreaText { set => whiteAreaText.text = value; }
    public string BlackPlayerNameText { set => blackPlayerNameText.text = value; }
    public string WhitePlayerNameText { set => whitePlayerNameText.text = value; }
    public string OtherNameText { set => otherNameText.text = value; }
    #endregion
    
    private void Start()
    {
        SetReadyButtonText();
        playerNameText.text = RoomManager.instance.UserName;
    }

    public void SetReadyButtonText()
    {
        readyButtonText.text = PhotonNetwork.IsMasterClient ? "Start" : "Ready";
    }

    public void SetPlayerReadyStatus(bool status)
    {
        playerReadyStatus.isOn = status;
    }
    
    public void SetOtherPlayerReadyStatus(bool status)
    {
        otherPlayerReadyStatus.isOn = status;
    }

    public void ReadyButonOnClick()
    {
        TurnManager.instance.PlayerReady = true;
        EventSender.SendRaiseEvent(EventTypes.RequestReady, true, ReceiverGroup.Others);
        Debug.Log("[Sender] RequestReady");
    }

    public void DisconnectRoomButtonOnClick()
    {
        RoomManager.instance.DisconnectRoom();
        SceneManagerEx.SceneChange(SceneTypes.Lobby);
    }

    public void StartGame()
    {
        readyPopup.SetActive(false);
    }
    
    public void CallWinEvent()
    {
        resultPopup.SetActive(true);
        var roomManager = RoomManager.instance;
        var turnManager = TurnManager.instance;
        var winnerName = turnManager.IsMyTurn ? roomManager.UserName : roomManager.OtherUserName;
        roomManager.isFinish = true;
        winnerNameText.text = $"winner : {winnerName}";
        resultText.text = turnManager.IsMyTurn ? "Win!!!" : "Lose...";
    }
    
    public void SetAreaText(bool isMyTurn)
    {
        BlackAreaText = isMyTurn ? "흑돌 (선공) - me" : "흑돌 (선공)";
        WhiteAreaText = !isMyTurn ? "백돌 (후공) - me" : "백돌 (후공)";
    }

    public void SetPlayerText(bool isMyTurn, string userName, string otherName)
    {
        BlackPlayerNameText = isMyTurn ? userName : otherName;
        WhitePlayerNameText = !isMyTurn ? userName : otherName;
    }
}
