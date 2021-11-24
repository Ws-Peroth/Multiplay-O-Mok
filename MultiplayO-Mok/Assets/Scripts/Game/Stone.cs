using System.Collections.Generic;
using Game.GameManager;
using Game.PhotonStreaming;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public enum StoneTypes
    {
        None,
        Black,
        White
    }

    public class Stone : MonoBehaviour
    {
        [SerializeField] private List<Sprite> placeSprites = new List<Sprite>();
        private readonly Color _alphaWhite = new Color(1, 1, 1, 0.5f);
        private readonly Color _defaultColor = Color.white;
        public Image spriteImage;
        public int Status { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }

        public void SetSprite(StoneTypes type)
        {
            SetSprite((int) type);
        }

        public void SetSprite(int type)
        {
            spriteImage.sprite = placeSprites[type];
            spriteImage.color = _defaultColor;
        }

        public void ButtonOnClick()
        {
            if (!TurnManager.Instance.IsMyTurn) return;
            if (Status != 0) return;
            Debug.Log("[Stone] ButtonOnClick");
            SetStone(TurnManager.Instance.MyColor);
            EventSender.SendRaiseEvent(EventTypes.SetStone, new[] {PositionX, PositionY}, ReceiverGroup.Others);
        }

        public void SetStone(int setColor)
        {
            Debug.Log($"Call Set Stone : color {setColor.ToString()}");
            FieldManager.Instance.SetStone(PositionX, PositionY, setColor);
            TurnManager.Instance.TurnChange();
        }

        private void OnMouseEnter()
        {
            // 작동 안함
            if (Status != 0) return;
            Debug.Log("[Stone] OnMouseEnter");
            spriteImage.color = _alphaWhite;
            SetSprite(TurnManager.Instance.MyColor);
        }

        private void OnMouseExit()
        {
            // 작동 안함
            if (Status != 0) return;
            Debug.Log("[Stone] OnMouseExit");
            spriteImage.color = _defaultColor;
            SetSprite(StoneTypes.None);
        }
    }
}