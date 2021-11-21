using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public enum EventTypes
{
    Default,
    SetStone,
    RequestReady,
    AnswerReady,
    ReadyOver,
    GameStart,
    SetTurn
}

public class EventReceiver : PunSingleton<EventReceiver>,IOnEventCallback
{
    protected override void Awake()
    {
        dontDestroyOnLoad = true;
        base.Awake();
    }

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (RoomManager.instance.IsPlayerLeave) return;
        
        var data = photonEvent.CustomData;
        // When Received Raise Event
        switch ((EventTypes) photonEvent.Code)
        {
            case EventTypes.SetStone:
                Debug.Log("[Receiver] SetStone");
                SetStoneEvent(data);
                break;

            case EventTypes.RequestReady:
                Debug.Log("[Receiver] RequestReady");
                RequestReadyEvent(data);
                break;

            case EventTypes.AnswerReady:
                Debug.Log("[Receiver] AnswerReady");
                AnswerReadyEvent(data);
                break;

            case EventTypes.ReadyOver:
                Debug.Log("[Receiver] ReadyOver");
                ReadyOverEvent();
                break;

            case EventTypes.GameStart:
                Debug.Log("[Receiver] GameStart");
                GameStartEvent();
                break;
            
            case EventTypes.SetTurn:
                SetPlayTurn(data);
                break;
        }
    }

    private void SetStoneEvent(object data)
    {
        // 돌을 두었다는 이벤트 알림
        var position = (int[]) data;
        var x = position[0];
        var y = position[1];
        FieldManager.instance._omokStoneGameObjects[y, x].SetStone(TurnManager.instance.OtherColor);
        Debug.Log($"[Receiver] Receive Position : ({x.ToString()}, {y.ToString()})");
    }

    private static void RequestReadyEvent(object data)
    {
        var answer = TurnManager.instance.PlayerReady;
        TurnManager.instance.OtherPlayerReady = (bool) data;
        EventSender.SendRaiseEvent(EventTypes.AnswerReady, answer, ReceiverGroup.Others);
        Debug.Log("[Sender] Answer Ready");
    }

    private static void AnswerReadyEvent(object data)
    {
        var otherReady = (bool) data;
        TurnManager.instance.OtherPlayerReady = otherReady;
        var playerReady = TurnManager.instance.PlayerReady;
                
        if (otherReady && playerReady)
        {
            EventSender.SendRaiseEvent(EventTypes.ReadyOver, null, ReceiverGroup.MasterClient);
            Debug.Log("[Sender] Ready Over");
            return;
        }

        Debug.Log($"playerReady = {playerReady.ToString()}, otherPlayerReady = {otherReady.ToString()}");
    }

    private static void ReadyOverEvent()
    {
        EventSender.SendRaiseEvent(EventTypes.GameStart, null, ReceiverGroup.All);
        TurnManager.instance.SetPlayTurn();
        Debug.Log("[Sender] GameStart");
    }

    private static void GameStartEvent()
    {
        TurnManager.instance.GameStart();
        GameUiManager.instance.StartGame();
    }

    private void SetPlayTurn(object data)
    {
        var turn = (bool) data;
        TurnManager.instance.IsMyTurn = turn;
        TurnManager.instance.MyColor = (int) (turn ? StoneColorTypes.Black : StoneColorTypes.White);
        TurnManager.instance.OtherColor = (int) (!turn ? StoneColorTypes.Black : StoneColorTypes.White);
        
        Debug.Log($"[Receiver] MyTurn : {turn.ToString()}, Color : {TurnManager.instance.MyColor.ToString()}");
    }
}
