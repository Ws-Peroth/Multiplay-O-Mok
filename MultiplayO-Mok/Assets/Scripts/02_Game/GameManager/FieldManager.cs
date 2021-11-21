using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class FieldManager : Singleton<FieldManager>
{
    private PhotonView _photonView;
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
    }

    public void FieldCheck()
    {
        // 돌을 둔 위치로부터 탐색하여 승리 유저 체크
    }
}
