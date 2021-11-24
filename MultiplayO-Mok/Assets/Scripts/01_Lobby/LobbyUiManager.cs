using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUiManager : MonoBehaviour
{
    private bool _isPublic = true;
    private string _roomId = "_";
    [SerializeField] private GameObject CreatRoomPopup;
    [SerializeField] private GameObject JoinRoomIdPopup;
    
    public void UserNameInputFieldOnEndEdit(Text name)
    {
        RoomManager.instance.UserName = name.text;
    }
    
    public void RoomIdInputFieldOnEndEdit(Text roomId)
    {
        _roomId = roomId.text;
    }

    public void RoomOptionToggleOnValueChanged(Toggle option)
    {
        _isPublic = option.isOn;
    }

    public void JoinRandomRoomButtonOnClick()
    {
        RoomManager.instance.EnterPublicRoom();
    }

    public void JoinRoomIdPopupButtonOnClick()
    {
        JoinRoomIdPopup.SetActive(true);
    }

    public void JoinPrivateRoomButtonDown()
    {
        RoomManager.instance.EnterRoomId(_roomId);
    }
    
    public void CreatRoomPopupButtonOnClick()
    {
        CreatRoomPopup.SetActive(true);
    }
    
    public void DoCreatRoomButtonOnClick()
    {
        RoomManager.instance.CreatRoom(_isPublic);
    }
}
