using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby
{
    public class LobbyUiManager : MonoBehaviour
    {
        [SerializeField] private GameObject connectPopup;
        [SerializeField] private Text connectStatusText;
        private bool _isPublic = true;
        private string _roomId = "_";
        [SerializeField] private GameObject creatRoomPopup;
        [SerializeField] private GameObject joinRoomIdPopup;

        private void Update()
        {
            connectStatusText.text = RoomManager.Instance.ConnectStatus;
        }

        public void UserNameInputFieldOnEndEdit(Text userName)
        {
            RoomManager.Instance.UserName = userName.text;
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
            RoomConnecting();
            RoomManager.Instance.EnterPublicRoom();
        }

        public void JoinRoomIdPopupButtonOnClick()
        {
            joinRoomIdPopup.SetActive(true);
        }

        public void JoinPrivateRoomButtonDown()
        {
            RoomConnecting();
            RoomManager.Instance.EnterRoomId(_roomId);
        }

        public void CreatRoomPopupButtonOnClick()
        {
            creatRoomPopup.SetActive(true);
        }

        public void DoCreatRoomButtonOnClick()
        {
            RoomConnecting();
            RoomManager.Instance.CreatRoom(_isPublic);
        }

        private void RoomConnecting()
        {
            connectPopup.SetActive(true);
        }

        private void RoomConnectEnd()
        {
            connectPopup.SetActive(false);
        }

        public void DisconnectButtonOnClick()
        {
            RoomConnectEnd();
            RoomManager.Instance.DisconnectRoom();
        }
    }
}