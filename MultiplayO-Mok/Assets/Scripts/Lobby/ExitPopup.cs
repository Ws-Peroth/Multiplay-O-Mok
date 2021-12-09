using Game.GameManager;
using UnityEngine;

namespace Lobby
{
    public class ExitPopup : MonoBehaviour
    {
        [SerializeField] private GameObject settingPopup;

        public void ExitButtonOnClick()
        {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        }

        public void GiveUpButtonOnClick()
        {
            settingPopup.SetActive(false);
            TurnManager.Instance.IsMyTurn = false;
            GameUiManager.Instance.CallWinEvent("항복하였습니다.");
            RoomManager.Instance.DisconnectRoom();
        }
    }
}
