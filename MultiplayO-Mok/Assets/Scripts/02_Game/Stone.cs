using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public enum StoneTypes
{
    None,
    Black,
    White
}

public class Stone : MonoBehaviour
{
    private Color alphaWhite = new Color(1, 1, 1, 0.5f);
    private Color defaultColor = Color.white;
    [SerializeField] private List<Sprite> placeSprites = new List<Sprite>();
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
        spriteImage.color = defaultColor;
    }

    public void ButtonOnClick()
    {
        if (!TurnManager.instance.IsMyTurn) return;
        if (Status != 0) return;
        Debug.Log("[Stone] ButtonOnClick");
        SetStone(TurnManager.instance.MyColor);
        EventSender.SendRaiseEvent(EventTypes.SetStone, new[] {PositionX, PositionY}, ReceiverGroup.Others);
    }

    public void SetStone(int setColor)
    {
        Debug.Log($"Call Set Stone : color {setColor.ToString()}");
        FieldManager.instance.SetStone(PositionX, PositionY, setColor);
        TurnManager.instance.TurnChange();
    }

    private void OnMouseEnter()
    {
        if(Status != 0) return;
        Debug.Log("[Stone] OnMouseEnter");
        spriteImage.color = alphaWhite;
        SetSprite(TurnManager.instance.MyColor);
    }

    private void OnMouseExit()
    {
        if(Status != 0) return;
        Debug.Log("[Stone] OnMouseExit");
        spriteImage.color = defaultColor;
        SetSprite(StoneTypes.None);
    }
    
}
