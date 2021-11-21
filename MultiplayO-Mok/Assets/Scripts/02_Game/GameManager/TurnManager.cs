using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

public enum StoneColorTypes
{
    Blank = 0,
    Black = 1,
    White = 2
}

public class TurnManager : PunSingleton<TurnManager>
{
    private bool _playerReady;
    private bool _otherPlayerReady;
    public bool PlayerReady
    {
        get => _playerReady;
        set
        {
            _playerReady = value;
            GameUiManager.instance.SetPlayerReadyStatus(value);
        }
    }
    public bool OtherPlayerReady
    {
        get => _otherPlayerReady;
        set
        {
            _otherPlayerReady = value;
            GameUiManager.instance.SetOtherPlayerReadyStatus(value);
        }
    }

    public bool IsMyTurn { get; set; }

    public int MyColor { get; set; }
    public int OtherColor { get; set; }

    public void GameStart()
    {
        FieldManager.instance.FieldInitialize();
    }
    public void UserLeaveGame()
    {
        OtherPlayerReady = false;
    }

    public void GameEnd()
    {
        GameUiManager.instance.DisconnectRoomButtonOnClick();
    }
    
    public void SetPlayTurn()
    {
        // 선 후공 선택
        IsMyTurn = Random.Range(0, 2) == 1;
        MyColor = (int) (IsMyTurn ? StoneColorTypes.Black : StoneColorTypes.White);
        OtherColor = (int) (!IsMyTurn ? StoneColorTypes.Black : StoneColorTypes.White);
        
        EventSender.SendRaiseEvent(EventTypes.SetTurn, !IsMyTurn, ReceiverGroup.Others);
        Debug.Log($"[Receiver] MyTurn : {IsMyTurn.ToString()}, Color : {MyColor.ToString()}");
    }

    public void TurnChange()
    {
        IsMyTurn = !IsMyTurn;
        Debug.Log($"Change Turn : {IsMyTurn.ToString()}");
    }
}