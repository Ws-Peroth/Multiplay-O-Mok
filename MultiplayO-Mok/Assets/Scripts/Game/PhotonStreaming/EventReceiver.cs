using ExitGames.Client.Photon;
using Game.GameManager;
using Lobby;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Game.PhotonStreaming
{
    public enum EventTypes
    {
        Default,
        SetStone,
        RequestReady,
        AnswerReady,
        ReadyOver,
        GameStart,
        SetTurn,
        SetPlayerName
    }

    public class EventReceiver : PunSingleton<EventReceiver>, IOnEventCallback
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
            if (RoomManager.Instance.IsPlayerLeave) return;

            var data = photonEvent.CustomData;
            switch ((EventTypes) photonEvent.Code)
            {
                // When Received Raise Event
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
                case EventTypes.SetPlayerName:
                    SetPlayerName(data);
                    break;
            }
        }

        private static void SetStoneEvent(object data)
        {
            // 돌을 두었다는 이벤트 알림
            var position = (int[]) data;
            var x = position[0];
            var y = position[1];
            FieldManager.Instance.OMokStoneGameObjects[y, x].SetStone(TurnManager.Instance.OtherColor);
            Debug.Log($"[Receiver] Receive Position : ({x.ToString()}, {y.ToString()})");
        }

        private static void RequestReadyEvent(object data)
        {
            var answer = TurnManager.Instance.PlayerReady;
            TurnManager.OtherPlayerReady((bool) data);
            EventSender.SendRaiseEvent(EventTypes.AnswerReady, answer, ReceiverGroup.Others);
            Debug.Log("[Sender] Answer Ready");
        }

        private static void AnswerReadyEvent(object data)
        {
            var otherReady = (bool) data;
            TurnManager.OtherPlayerReady(otherReady);
            var playerReady = TurnManager.Instance.PlayerReady;

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
            TurnManager.Instance.SetPlayTurn();
            Debug.Log("[Sender] GameStart");
        }

        private static void GameStartEvent()
        {
            TurnManager.Instance.GameStart();
            GameUiManager.Instance.StartGame();
        }

        private static void SetPlayTurn(object data)
        {
            var turn = (bool) data;
            TurnManager.Instance.IsMyTurn = turn;
            TurnManager.Instance.SetPlayerTurnData();
            Debug.Log($"[Receiver] MyTurn : {turn.ToString()}, Color : {TurnManager.Instance.MyColor.ToString()}");
        }

        private static void SetPlayerName(object data)
        {
            var otherName = (string) data;
            RoomManager.Instance.OtherUserName = otherName;
            GameUiManager.Instance.OtherNameText = otherName;
        }
    }
}