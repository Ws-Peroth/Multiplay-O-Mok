using System;
using Game.PhotonStreaming;
using Lobby;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GameManager
{
    public class GameUiManager : Singleton<GameUiManager>
    {
        #region 멤버 변수 & 프로퍼티
        [SerializeField] private Text winMessageText;
        [SerializeField] private Text resultText;
        [SerializeField] private Text winnerNameText;
        [SerializeField] private Text blackAreaText;
        [SerializeField] private Text whiteAreaText;
        [SerializeField] private Text playerNameText;
        [SerializeField] private Text otherNameText;
        [SerializeField] private Text blackPlayerNameText;
        [SerializeField] private Text whitePlayerNameText;
        [SerializeField] private Text readyButtonText;
        [SerializeField] private Toggle playerReadyStatus;
        [SerializeField] private Toggle otherPlayerReadyStatus;
        [SerializeField] private GameObject readyPopup;
        [SerializeField] private GameObject resultPopup;

        public bool IsStart { get; set; }

        public string OtherNameText
        {
            set => otherNameText.text = value;
        }

        #endregion

        private void Start()
        {
            SetReadyButtonText();
            playerNameText.text = RoomManager.Instance.UserName;
        }

        public void SetReadyButtonText()
        {
            readyButtonText.text = PhotonNetwork.IsMasterClient ? "Start" : "Ready";
        }

        public void SetPlayerReadyStatus(bool status)
        {
            playerReadyStatus.isOn = status;
        }

        public void SetOtherPlayerReadyStatus(bool status)
        {
            otherPlayerReadyStatus.isOn = status;
        }

        public void ReadyButtonOnClick()
        {
            TurnManager.Instance.PlayerReady = true;
            EventSender.SendRaiseEvent(EventTypes.RequestReady, true, ReceiverGroup.Others);
            Debug.Log("[Sender] RequestReady");
        }

        public void DisconnectRoomButtonOnClick()
        {
            RoomManager.Instance.DisconnectRoom();
            SceneManagerEx.SceneChange(SceneTypes.Lobby);
        }

        public void StartGame()
        {
            readyPopup.SetActive(false);
            IsStart = true;
        }

        public void CallWinEvent(string winMessage)
        {
            IsStart = false;
            resultPopup.SetActive(true);
            var roomManager = RoomManager.Instance;
            var turnManager = TurnManager.Instance;
            var winnerName = turnManager.IsMyTurn ? roomManager.UserName : roomManager.OtherUserName;
            roomManager.IsFinish = true;
            winnerNameText.text = $"winner : {winnerName}";
            resultText.text = turnManager.IsMyTurn ? "Win!!!" : "Lose...";
            winMessageText.text = winMessage;
            TurnManager.Instance.PlayerReady = false;
        }

        public void SetAreaText(bool isMyTurn)
        {
            blackAreaText.text = isMyTurn ? "흑돌 (선공) - me" : "흑돌 (선공)";
            whiteAreaText.text = !isMyTurn ? "백돌 (후공) - me" : "백돌 (후공)";
        }

        public void SetPlayerText(bool isMyTurn, string userName, string otherName)
        {
            blackPlayerNameText.text = isMyTurn ? userName : otherName;
            whitePlayerNameText.text = !isMyTurn ? userName : otherName;
        }
    }
}