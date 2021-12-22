using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

namespace Game.GameManager
{
    public class LogManager : Singleton<LogManager>
    {
        public int turnCount { get; set; }
        public void PrintPositionLog(int x, int y, int color)
        {
            PrintPositionLog(new Vector2(x, y), (StoneTypes) color);
        }
        
        public void PrintPositionLog(Vector2 position, StoneTypes color)
        {
            turnCount++;
            switch (color)
            {
                case StoneTypes.White:
                    // 백돌 영역에 로그 추가
                    GameUiManager.Instance.AddWhiteLog(turnCount, position);
                    break;
                case StoneTypes.Black:
                    // 흑돌 영역에 로그 추가
                    GameUiManager.Instance.AddBlackLog(turnCount, position);
                    break;
            }
        }
    }
}