using System;
using Game.PhotonStreaming;
using Lobby;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Playables;
using Random = UnityEngine.Random;

namespace Game.GameManager
{
    public enum StoneColorTypes
    {
        Blank = 0,
        Black = 1,
        White = 2
    }

    public class TurnManager : PunSingleton<TurnManager>
    {
        private bool _playerReady;

        public bool PlayerReady
        {
            get => _playerReady;
            set
            {
                _playerReady = value;
                GameUiManager.Instance.SetPlayerReadyStatus(value);
            }
        }
        public bool IsMyTurn { get; set; }
        public int MyColor { get; set; }
        public int OtherColor { get; set; }
        
        public static void OtherPlayerReady(bool status)
        {
            GameUiManager.Instance.SetOtherPlayerReadyStatus(status);
        }

        public void GameStart()
        {
            FieldManager.Instance.FieldInitialize();
        }

        public void SetPlayTurn()
        {
            // 선 후공 선택
            IsMyTurn = Random.Range(0, 2) == 1;
            SetPlayerTurnData();
            // 선 후공을 정한 이벤트 수신
            EventSender.SendRaiseEvent(EventTypes.SetTurn, !IsMyTurn, ReceiverGroup.Others);
            Debug.Log($"[Receiver] MyTurn : {IsMyTurn.ToString()}, Color : {MyColor.ToString()}");
        }

        public void SetPlayerTurnData()
        {
            var roomManager = RoomManager.Instance;
            // 선 후공 색 선택
            MyColor = (int) (IsMyTurn ? StoneColorTypes.Black : StoneColorTypes.White);
            OtherColor = (int) (!IsMyTurn ? StoneColorTypes.Black : StoneColorTypes.White);
            GameUiManager.Instance.SetPlayerTurnMark();
            // 플레이어 이름 설정
            GameUiManager.Instance.SetAreaText(IsMyTurn);
            GameUiManager.Instance.SetPlayerText(IsMyTurn, roomManager.UserName, roomManager.OtherUserName);
        }

        public void TurnChange()
        {
            IsMyTurn = !IsMyTurn;
            GameUiManager.Instance.TurnOnStatusReverse();
            Debug.Log($"Change Turn : {IsMyTurn.ToString()}");
        }
    }
}