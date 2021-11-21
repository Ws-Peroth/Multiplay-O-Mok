using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class GameUiManager : Singleton<GameUiManager>
{
    [SerializeField] private Text readyButtonText;
    [SerializeField] private GameObject readyPopup;
    [SerializeField] private Toggle playerReadyStatus;
    [SerializeField] private Toggle otherPlayerReadyStatus;

    private void Start()
    {
        SetReadyButtonText();
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
    
    public void ReadyGame()
    {
        readyPopup.SetActive(true);
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
}
