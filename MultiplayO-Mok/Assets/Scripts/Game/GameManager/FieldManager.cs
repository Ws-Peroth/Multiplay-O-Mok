using Unity.Mathematics;
using UnityEngine;

namespace Game.GameManager
{
    public class FieldManager : Singleton<FieldManager>
    {
        public const int BoardPivotX = -8;
        public const int BoardPivotY = 4;
        private const int WinCount = 5;
        private const int FieldSize = 19;
        private const int FieldLimit = FieldSize - WinCount;

        private readonly int[,] _oMokBoardData = new int[FieldSize, FieldSize];
        public readonly Stone[,] OMokStoneGameObjects = new Stone[FieldSize, FieldSize];
        [SerializeField] private Transform fieldTransform;
        [SerializeField] private GameObject stonePrefab;


        public void FieldInitialize()
        {
            for (var y = 0; y < FieldSize; y++)
            {
                for (var x = 0; x < FieldSize; x++)
                {
                    if (OMokStoneGameObjects[y, x] is null)
                    {
                        var stoneObject = Instantiate(stonePrefab).GetComponent<Stone>();
                        OMokStoneGameObjects[y, x] = InitializeStoneObject(stoneObject, x, y);
                    }

                    OMokStoneGameObjects[y, x].SetSprite(StoneTypes.None);
                    _oMokBoardData[y, x] = (int) StoneTypes.None;
                }
            }
        }

        private Stone InitializeStoneObject(Stone stone, int x, int y)
        {
            var stoneGameObject = stone.gameObject;
            stoneGameObject.transform.SetParent(fieldTransform);
            stoneGameObject.transform.localScale = Vector3.one;
            stoneGameObject.transform.localPosition = new Vector3(BoardPivotX + x, BoardPivotY - y, 0);
            stoneGameObject.SetActive(true);
            stone.PositionX = x;
            stone.PositionY = y;
            return stone;
        }

        public void SetStone(int x, int y, int setColor)
        {
            _oMokBoardData[y, x] = setColor;
            OMokStoneGameObjects[y, x].Status = setColor;
            OMokStoneGameObjects[y, x].SetSprite(setColor);
            OMokCheck(x, y, setColor);
            
        }

        private void OMokCheck(int x, int y, int setColor)
        {
            var startX = x < WinCount ? 0 : x - WinCount;
            var endX = x > FieldLimit ? FieldSize : x + 5;
            var startY = y < WinCount ? 0 : y - WinCount;
            var endY = y > FieldLimit ? FieldSize : y + 5;

            var cnt = 0;
            // 세로 확인
            for (var i = startY; i < endY; i++)
            {
                cnt = _oMokBoardData[i, x] == setColor ? cnt + 1 : 0;
                if (cnt != WinCount) continue;
                GameUiManager.Instance.CallWinEvent("오목을 먼저 완성하였습니다.");
                return;
            }

            // 가로 확인
            for (var i = startX; i < endX; i++)
            {
                cnt = _oMokBoardData[y, i] == setColor ? cnt + 1 : 0;
                if (cnt != WinCount) continue;
                GameUiManager.Instance.CallWinEvent("오목을 먼저 완성하였습니다.");
                return;
            }

            startX = x - math.min(GetDecreaseValue(x), GetIncreaseValue(y));
            startY = y + math.min(GetDecreaseValue(x), GetIncreaseValue(y));
            if (startY >= FieldSize) // y가 19의 값을 가졌을 경우
            {
                var deltaValue = FieldSize - startY + 1;
                startY -= deltaValue;
                startX += deltaValue;
            }

            endX = x + math.min(GetIncreaseValue(x), GetDecreaseValue(y));
            cnt = 0;
            for (int i = startX, j = startY; i < endX; i++, j--)
            {
                cnt = _oMokBoardData[j, i] == setColor ? cnt + 1 : 0;
                if (cnt != 5) continue;
                GameUiManager.Instance.CallWinEvent("오목을 먼저 완성하였습니다.");
                return;
            }

            startX = x - math.min(GetDecreaseValue(x), GetDecreaseValue(y));
            startY = y - math.min(GetDecreaseValue(x), GetDecreaseValue(y));
            endX = x + math.min(GetIncreaseValue(x), GetIncreaseValue(y));
            cnt = 0;
            for (int i = startX, j = startY; i < endX; i++, j++)
            {
                cnt = _oMokBoardData[j, i] == setColor ? cnt + 1 : 0;
                if (cnt != 5) continue;
                GameUiManager.Instance.CallWinEvent("오목을 먼저 완성하였습니다.");
                return;
            }
        }

        private void FullFieldCheck()
        {
            
        }
        
        private static int GetIncreaseValue(int position)
        {
            return position > FieldLimit ? FieldSize - position : WinCount;
        }

        private static int GetDecreaseValue(int position)
        {
            return position < WinCount ? position : WinCount;
        }
    }
}