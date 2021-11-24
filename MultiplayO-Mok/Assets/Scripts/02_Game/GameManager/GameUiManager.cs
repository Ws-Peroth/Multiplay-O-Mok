using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class GameUiManager : Singleton<GameUiManager>
{
    public Text resultText;
    public Text winnerNameText;
    public Text blackAreaText;
    public Text whiteAreaText;
    public Text blackPlayerNameText;
    public Text whitePlayerNameText;
    public Text playerNameText;
    public Text otherNameText;
    
    [SerializeField] private Text readyButtonText;
    [SerializeField] private GameObject readyPopup;
    [SerializeField] private GameObject resultPopup;
    [SerializeField] private Toggle playerReadyStatus;
    [SerializeField] private Toggle otherPlayerReadyStatus;

    private void Start()
    {
        SetReadyButtonText();
        playerNameText.text = RoomManager.instance.UserName;
    }

    public void SetReadyButtonText()
    {
        readyButtonText.text = RoomManager.instance.IsRoomMaster ? "Start" : "Ready";
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
}
