using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FieldManager : Singleton<FieldManager>
{
    private const int _boardPivotX = -8;
    private const int _boardPivotY = 4;
    private const int _winCount = 5;
    private const int _fieldSize = 19;
    private const int fieldLimit = _fieldSize - _winCount;

    private readonly int[,] _omokBoardData = new int[_fieldSize, _fieldSize];
    public readonly Stone[,] OmokStoneGameObjects = new Stone[_fieldSize, _fieldSize];
    [SerializeField] private Transform fieldTransform;
    [SerializeField] private GameObject stonePrefab;
    

    public void FieldInitialize()
    {
        for (var y = 0; y < _fieldSize; y++)
        {
            for (var x = 0; x < _fieldSize; x++)
            {
                if (OmokStoneGameObjects[y, x] is null)
                {
                    var stoneObject = Instantiate(stonePrefab).GetComponent<Stone>();
                    OmokStoneGameObjects[y, x] = InitializeStoneObject(stoneObject, x, y);
                }
                OmokStoneGameObjects[y, x].SetSprite(StoneTypes.None);
                _omokBoardData[y, x] = (int) StoneTypes.None;
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
        _omokBoardData[y, x] = setColor;
        OmokStoneGameObjects[y, x].Status = setColor;
        OmokStoneGameObjects[y, x].SetSprite(setColor);
        FieldCheck(x, y, setColor);
    }

    private void FieldCheck(int x, int y, int setColor)
    {
        // 돌을 둔 위치로부터 탐색하여 승리 유저 체크
        var startX = x < _winCount ? 0 : x - _winCount;
        var endX = x > fieldLimit ? _fieldSize : x + 5;
        
        var startY = y < _winCount ? 0 : y - _winCount;
        var endY = y > fieldLimit ? _fieldSize : y + 5;
        var cnt = 0;
        // 세로 확인
        Debug.Log("[Win Check] Check Y");
        for (var i = startY; i < endY; i++)
        {
            cnt = _omokBoardData[i, x] == setColor ? cnt + 1 : 0;
            if (cnt != _winCount) continue;
            GameUiManager.instance.CallWinEvent();
            return;
        }
        // 가로 확인
        Debug.Log("[Win Check] Check X");
        for (var i = startX; i < endX; i++)
        {
            cnt = _omokBoardData[y, i] == setColor ? cnt + 1 : 0;
            if (cnt != _winCount) continue;
            GameUiManager.instance.CallWinEvent();
            return;
        }
        
        Debug.Log("[Win Check] Check Button Left -> Top Right");
        // Button Left -> Top Right
        startX = x - math.min(GetDecreasValue(x), GetIncreasValue(y));
        startY = y + math.min(GetDecreasValue(x), GetIncreasValue(y));
        if (startY >= _fieldSize)   // y가 19의 값을 가졌을 경우
        {
            var deltaValue = _fieldSize - startY + 1;
            startY -= deltaValue;
            startX += deltaValue;
        }

        endX = x + math.min(GetIncreasValue(x), GetDecreasValue(y));
        cnt = 0;
        Debug.Log($"[Win Check] ({startX}, {startY}) -> ({endX})");
        for (int i = startX, j = startY; i < endX; i++, j--)
        {
            Debug.Log($"[Check] ({i}, {j})");
            cnt = _omokBoardData[j, i] == setColor ? cnt + 1 : 0;
            if (cnt != 5) continue;
            GameUiManager.instance.CallWinEvent();
            return;
        }

        // Top Left -> Bottom Right
        Debug.Log("[Win Check] Check Top Left -> Bottom Right");
        startX = x - math.min(GetDecreasValue(x), GetDecreasValue(y));
        startY = y - math.min(GetDecreasValue(x), GetDecreasValue(y));
        endX = x + math.min(GetIncreasValue(x), GetIncreasValue(y));
        cnt = 0;
        Debug.Log($"[Win Check] ({startX}, {startY}) -> ({endX})");
        for (int i = startX, j = startY; i < endX; i++, j++)
        {
            cnt = _omokBoardData[j, i] == setColor ? cnt + 1 : 0;
            if (cnt != 5) continue;
            GameUiManager.instance.CallWinEvent();
            return;
        }
    }

    private int GetIncreasValue(int position)
    {
        return position > fieldLimit ? _fieldSize - position : _winCount;
    }

    private int GetDecreasValue(int position)
    {
        return position < _winCount ? position : _winCount;
    }
}
