using UnityEngine;
using UnityEngine.UI;

namespace Lobby
{
    public class LobbyUiManager : MonoBehaviour
    {
        private bool _isPublic = true;
        private string _roomId = "_";
        [SerializeField] private GameObject creatRoomPopup;
        [SerializeField] private GameObject joinRoomIdPopup;

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
            RoomManager.Instance.EnterPublicRoom();
        }

        public void JoinRoomIdPopupButtonOnClick()
        {
            joinRoomIdPopup.SetActive(true);
        }

        public void JoinPrivateRoomButtonDown()
        {
            RoomManager.Instance.EnterRoomId(_roomId);
        }

        public void CreatRoomPopupButtonOnClick()
        {
            creatRoomPopup.SetActive(true);
        }

        public void DoCreatRoomButtonOnClick()
        {
            RoomManager.Instance.CreatRoom(_isPublic);
        }
    }
}