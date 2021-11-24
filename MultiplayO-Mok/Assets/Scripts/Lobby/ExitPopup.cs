using System.Collections;
using System.Collections.Generic;
using Lobby;
using UnityEngine;

public class ExitPopup : MonoBehaviour
{
    [SerializeField] private GameObject settingCanvas;

    public void ExitButtonOnClick()
    {
        Application.Quit();
    }

    public void GiveUpButtonOnClick()
    {
        RoomManager.Instance.DisconnectRoom();
    }
}
