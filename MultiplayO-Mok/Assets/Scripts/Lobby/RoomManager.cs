using System;
using System.Collections.Generic;
using Game.GameManager;
using Game.PhotonStreaming;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Lobby
{
    public class RoomManager : PunSingleton<RoomManager>
    {
        private const int MAXRoomId = 1000000;
        private const int MINRoomId = 100000;
        private const int MAXUserId = 100000;
        private const int MINUserId = 10000;
        private bool _isConnecting;
        private bool _isCreateRoom;
        private bool _isPublicRoom;
        public string RoomName { get; set; }
        public string ConnectStatus { get; set; } = "Room Connecting";
        public bool IsFinish { get; set; }
        public bool IsPlayerLeave { get; set; }
        public string UserName { get; set; } = "";
        public string OtherUserName { get; set; }

        protected override void Awake()
        {
            dontDestroyOnLoad = true;
            base.Awake();
        }

        private void InitializedMatchingData(bool isPublicRoom, bool isConnecting, bool isCreateRoom)
        {
            _isPublicRoom = isPublicRoom;
            _isConnecting = isConnecting;
            _isCreateRoom = isCreateRoom;
        }

        private static string GetRandomRoomCode()
        {
            return Random.Range(MINRoomId, MAXRoomId).ToString();
        }

        public void CreatRoom(bool isPublicRoom)
        {
            if (_isConnecting) return;
            InitializedMatchingData(isPublicRoom, true, true);

            Debug.Log("[Creat Room] Enter Random Room");
            PhotonNetwork.ConnectUsingSettings();
        }

        public void EnterPublicRoom()
        {
            if (_isConnecting) return;
            InitializedMatchingData(true, true, false);

            Debug.Log("[Enter Room] Enter Public Room");
            PhotonNetwork.ConnectUsingSettings();
        }

        public void EnterRoomId(string roomId)
        {
            if (_isConnecting) return;
            RoomName = roomId;
            InitializedMatchingData(false, true, false);

            Debug.Log("::Private Mode:: Enter Private Room");
            PhotonNetwork.ConnectUsingSettings();
        }

        public void DisconnectRoom()
        {
            Debug.Log("[Disconnect Room]");
            PhotonNetwork.Disconnect();
            ConnectStatus = "Room Connecting";
        }

        public override void OnConnectedToMaster()
        {
            ConnectStatus = "Room Connecting";
            if (UserName is null || UserName == String.Empty)
            {
                UserName = $"User{Random.Range(MINUserId, MAXUserId).ToString()}";
            }

            PhotonNetwork.LocalPlayer.NickName = UserName;

            if (_isCreateRoom)
            {
                // 방 생성시 수행할 코드
                RoomName = GetRandomRoomCode();

                var roomOptions = new RoomOptions
                {
                    IsVisible = _isPublicRoom,
                    MaxPlayers = 2
                };

                PhotonNetwork.CreateRoom(RoomName, roomOptions);
            }
            else
            {
                // 방 입장시 수행할 코드
                if (_isPublicRoom)
                {
                    // 공개방 입장
                    PhotonNetwork.JoinRandomRoom(null, 2);
                }
                else
                {
                    // 비공개방 입장
                    PhotonNetwork.JoinRoom(RoomName);
                }
            }
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            // 방 생성 실패시 callback
            _isConnecting = false;
            // DisconnectRoom();
            ConnectStatus = $"Room Creat Failed : {message} ({returnCode.ToString()})";
            Debug.Log($"{returnCode.ToString()} : {message}");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            // 랜덤 방 입장 실패시 callback
            _isConnecting = false;
            // DisconnectRoom();
            ConnectStatus = $"Room Join Failed : {message} ({returnCode.ToString()})";
            Debug.Log($"{returnCode.ToString()} : {message}");
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            // 방 입장 실패시 callback
            _isConnecting = false;
            // DisconnectRoom();
            ConnectStatus = $"Room Join Failed : {message} ({returnCode.ToString()})";
            Debug.Log($"{returnCode.ToString()} : {message}");
        }

        public override void OnJoinedRoom()
        {
            // 방 입장 성공시 callback
            _isConnecting = false;
            IsFinish = false;
            Debug.Log("[OnJoinedRoom] : Join Success");
            SceneManagerEx.SceneChange(SceneTypes.Game);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            // 연결 종료시 callback
            InitializedMatchingData(false, false, false);
            DisconnectRoom();
            Debug.Log("[OnDisconnected] : Disconnect Success");
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (GameUiManager.Instance.IsStart)
            {
                TurnManager.Instance.IsMyTurn = true;
                GameUiManager.Instance.CallWinEvent("상대가 게임을 나갔습니다.");
                return;
            }
            if (IsFinish) return;
            GameUiManager.Instance.OtherNameText = "";
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            IsPlayerLeave = false;
            OtherUserName = newPlayer.NickName;
            GameUiManager.Instance.OtherNameText = OtherUserName;
            EventSender.SendRaiseEvent(EventTypes.SetPlayerName, PhotonNetwork.LocalPlayer.NickName,
                ReceiverGroup.Others);
            Debug.Log($"{OtherUserName} joined the room");
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            GameUiManager.Instance.SetReadyButtonText();
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            // 방 리스트 변경 시 callback
        }
    }
}