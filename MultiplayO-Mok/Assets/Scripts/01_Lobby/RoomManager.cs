using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RoomManager : PunSingleton<RoomManager>
{
    public bool isFinish;
    public string UserName { get; set; }
    public string OtherUserName { get; set; }
    
    [SerializeField] private string _currentRoom;
    
    private const int _maxRoomId = 1000000;
    private const int _minRoomId = 100000;
    private const int _maxUserId = 100000;
    private const int _minUserId = 10000;
    private bool _isConnecting;
    [SerializeField] private bool _isShowRoomList;

    public bool IsPlayerLeave { get; set; }
    public bool IsRoomMaster { get; set; }
    public bool IsCreateRoom { get; set; }
    public bool IsPublicRoom { get; set; }
    public string RoomName { get; set; }
    
    protected override void Awake()
    {
        dontDestroyOnLoad = true;
        base.Awake();
    }

    private void Update()
    {
        _currentRoom = PhotonNetwork.IsConnected switch
        {
            true when PhotonNetwork.CurrentRoom != null => $"ID : {PhotonNetwork.CurrentRoom.Name}",
            true => "Connecting",
            _ => "Disconnect"
        };
    }

    private void InitializedMatchingData(bool isPublicRoom, bool isConnecting, bool isCreateRoom)
    {
        IsPublicRoom = isPublicRoom;
        _isConnecting = isConnecting;
        IsCreateRoom = isCreateRoom;
    }

    public void ShowPublicRoomList()
    {
        Debug.Log("Show Public Room List");
        _isShowRoomList = true;
        // 기능 구현 필요
        _isShowRoomList = false;
    }

    private string GetRandomRoomCode()
    {
        return Random.Range(_minRoomId, _maxRoomId).ToString();
    }

    public void CreatRoom(bool isPublicRoom)
    {
        if(_isConnecting) return;
        InitializedMatchingData(isPublicRoom, true, true);
        
        Debug.Log("[Creat Room] Enter Random Room");
        PhotonNetwork.ConnectUsingSettings();
    }       

    public void EnterPublicRoom()
    {
        if(_isConnecting) return;
        InitializedMatchingData(true, true, false);

        Debug.Log("[Enter Room] Enter Public Room");
        PhotonNetwork.ConnectUsingSettings();
    }

    public void EnterRoomId(string roomId)
    {
        if(_isConnecting) return;
        RoomName = roomId;
        InitializedMatchingData(false, true, false);

        Debug.Log("::Private Mode:: Enter Private Room");
        PhotonNetwork.ConnectUsingSettings();
    }
    
    public void DisconnectRoom()
    {
        Debug.Log("[Disconnect Room]");
        PhotonNetwork.Disconnect();
    }

    public override void OnConnectedToMaster()
    {
        
        if (UserName is null || UserName == String.Empty)
        {
            UserName = $"User{Random.Range(_minUserId, _maxUserId).ToString()}";
        }
        PhotonNetwork.LocalPlayer.NickName = UserName;
        
        if (IsCreateRoom)
        {
            // 방 생성시 수행할 코드
            IsRoomMaster = true;
            RoomName = GetRandomRoomCode();

            var roomOptions = new RoomOptions();
            roomOptions.IsVisible = IsPublicRoom;
            roomOptions.MaxPlayers = 2;

            PhotonNetwork.CreateRoom(RoomName, roomOptions);
        }
        else
        {
            // 방 입장시 수행할 코드
            if (IsPublicRoom)
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
        DisconnectRoom();
        Debug.Log($"{returnCode.ToString()} : {message}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // 랜덤 방 입장 실패시 callback
        _isConnecting = false;
        DisconnectRoom();
        Debug.Log($"{returnCode.ToString()} : {message}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // 방 입장 실패시 callback
        _isConnecting = false;
        DisconnectRoom();
        Debug.Log($"{returnCode.ToString()} : {message}");
    }
    
    public override void OnJoinedRoom()
    {
        // 방 입장 성공시 callback
        _isConnecting = false;
        isFinish = false;
        Debug.Log("[OnJoinedRoom] : Join Success");
        SceneManagerEx.SceneChange(SceneTypes.Game);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // 연결 종료시 callback
        IsRoomMaster = false;
        InitializedMatchingData(false, false, false);
        DisconnectRoom();
        Debug.Log("[OnDisconnected] : Disconnect Success");
    }

    private void UpdateRoomList(List<RoomInfo> roomList)
    {
        Debug.Log("Update Room List");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (isFinish) return;
        GameUiManager.instance.DisconnectRoomButtonOnClick();
        GameUiManager.instance.otherNameText.text = "";
        /*
        IsPlayerLeave = true;
        Debug.Log($"{otherPlayer.NickName} left the room");
        OtherUserName = "User Leave The Room";  
        TurnManager.instance.UserLeaveGame();
        */
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        IsPlayerLeave = false;
        OtherUserName = newPlayer.NickName;
        GameUiManager.instance.otherNameText.text = OtherUserName;
        EventSender.SendRaiseEvent(EventTypes.SetPlayerName, PhotonNetwork.LocalPlayer.NickName, ReceiverGroup.Others);
        Debug.Log($"{OtherUserName} joined the room");
    }
    
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("[Receive Callback] On Room List Update");
        // 미구현
        // 방 리스트 변경 시 callback
        if (_isShowRoomList) return;
        
        UpdateRoomList(roomList);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        IsRoomMaster = true;
        GameUiManager.instance.SetReadyButtonText();
    }
}