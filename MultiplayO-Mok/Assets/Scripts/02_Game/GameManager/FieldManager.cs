using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class FieldManager : Singleton<FieldManager>
{
    private PhotonView _photonView;
    private const int _winStoneCount = 5;
    private const int _fieldSize = 19;
    private const int _boardPivotX = -8;
    private const int _boardPivotY = 4;

    public int[,] omokBoardData = new int[_fieldSize, _fieldSize];
    public Stone[,] _omokStoneGameObjects = new Stone[_fieldSize, _fieldSize];
    [SerializeField] private Transform fieldTransform;
    [SerializeField] private GameObject stonePrefab;
    

    public void FieldInitialize()
    {
        for (var y = 0; y < _fieldSize; y++)
        {
            for (var x = 0; x < _fieldSize; x++)
            {
                if (_omokStoneGameObjects[y, x] is null)
                {
                    var stoneObject = Instantiate(stonePrefab).GetComponent<Stone>();
                    _omokStoneGameObjects[y, x] = InitializeStoneObject(stoneObject, x, y);
                }
                _omokStoneGameObjects[y, x].SetSprite(StoneTypes.None);
                omokBoardData[y, x] = (int) StoneTypes.None;
            }
        }
    }

    private Stone InitializeStoneObject(Stone stone, int x, int y)
    {
        var stoneGameObject = stone.gameObject;
        stoneGameObject.transform.SetParent(fieldTransform);
        stoneGameObject.transform.localScale = Vector3.one;
        stoneGameObject.transform.localPosition = new Vector3(_boardPivotX + x, _boardPivotY - y, 0);
        stoneGameObject.SetActive(true);
        stone.PositionX = x;
        stone.PositionY = y;
        return stone;
    }

    public void SetStone(int x, int y, int setColor)
    {
        omokBoardData[y, x] = setColor;
        _omokStoneGameObjects[y, x].Status = setColor;
        _omokStoneGameObjects[y, x].SetSprite(setColor);
        FieldCheck(x, y, setColor);
    }
    
    public void FieldCheck(int x, int y, int setColor)
    {
        // 돌을 둔 위치로부터 탐색하여 승리 유저 체크
        var startX = x - 5 < 0 ? 0 : x - 5;
        var startY = y - 5 < 0 ? 0 : y - 5;
        
        var endX = x + 5 > _fieldSize ? _fieldSize : x + 5;
        var endY = y + 5 > _fieldSize ? _fieldSize : y + 5;
        
        var cnt = 0;
        
        // 가로 세로 확인
        for (int i = startY; i < endY; i++)
        {
            cnt = omokBoardData[i, x] == setColor ? cnt + 1 : 0;
            if (cnt == 5)
            {
                CallWinEvent();
                return;
            }
        }
        cnt = 0;
        for (int i = startX; i < endX; i++)
        {
            cnt = omokBoardData[y, i] == setColor ? cnt + 1 : 0;
            if (cnt == 5)
            {
                CallWinEvent();
                return;
            }
        }
        
        // 대각선 확인
        // TODO
    }

    private void CallWinEvent()
    {
        
    }
}
