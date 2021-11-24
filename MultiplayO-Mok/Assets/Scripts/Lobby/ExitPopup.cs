using Game.GameManager;
using UnityEngine;

namespace Lobby
{
    public class ExitPopup : MonoBehaviour
    {
        [SerializeField] private GameObject settingCanvas;

        public void ExitButtonOnClick()
        {
            Application.Quit();
        }

        public void GiveUpButtonOnClick()
        {
            settingCanvas.SetActive(false);
            TurnManager.Instance.IsMyTurn = false;
            GameUiManager.Instance.CallWinEvent("항복하였습니다.");
            RoomManager.Instance.DisconnectRoom();
        }
    }
}
