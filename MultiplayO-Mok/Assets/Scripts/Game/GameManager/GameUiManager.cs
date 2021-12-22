using System;
using Game.PhotonStreaming;
using Lobby;
using Photon.Pun;
using Photon.Realtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GameManager
{
    public class GameUiManager : Singleton<GameUiManager>
    {
        #region 멤버 변수 & 프로퍼티
        
        [SerializeField] private Text roomIdText;
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
        [SerializeField] private GameObject whiteTurnImage;
        [SerializeField] private GameObject blackTurnImage;
        [SerializeField] private GameObject stoneMarkImage;
        [SerializeField] private Transform blackLogArea;
        [SerializeField] private Transform whiteLogArea;
        [SerializeField] private LogBlock logPrefab;

        
        public bool IsStart { get; set; }

        public string OtherNameText
        {
            set => otherNameText.text = value;
        }

        #endregion

        private void Start()
        {
            SetReadyButtonText();
            roomIdText.text = $"Room ID : {RoomManager.Instance.RoomName}";
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

        public void SetPlayerTurnMark()
        {
            whiteTurnImage.SetActive(false);
            blackTurnImage.SetActive(true);
        }

        public void TurnOnStatusReverse()
        {
            whiteTurnImage.SetActive(!whiteTurnImage.activeSelf);
            blackTurnImage.SetActive(!blackTurnImage.activeSelf);
        }

        public void AddWhiteLog(int turn, Vector2 position)
        {
            var logBlock = Instantiate(logPrefab, whiteLogArea);
            InitializeLogBlock(logBlock, turn, position);
        }

        public void AddBlackLog(int turn, Vector2 position)
         {
            var logBlock = Instantiate(logPrefab, blackLogArea);
            InitializeLogBlock(logBlock, turn, position);
        }

        private void InitializeLogBlock(LogBlock logBlock, int turn, Vector2 StonePosition)
        {
            if (!stoneMarkImage.activeSelf)
            {
                stoneMarkImage.SetActive(true);
            }
            
            logBlock.positionX = ((char) ('A' + StonePosition.x)).ToString();
            logBlock.positionY = (int) StonePosition.y + 1;
            var markPositionX = FieldManager.BoardPivotX + StonePosition.x;
            var markPositionY = FieldManager.BoardPivotY - StonePosition.y;
            stoneMarkImage.transform.localPosition = new Vector3(markPositionX, markPositionY, 0);
        }
    }
}