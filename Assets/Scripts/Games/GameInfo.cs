using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DHU2020.DGS.MiniGame.Game
{
    [
        CreateAssetMenu(
            fileName = "GameInfo",
            menuName = "dgw_work/Map/GameInfo"
        )
    ]
    public class GameInfo : ScriptableObject
    {
        public string[] gameTitlesEnglish;
        public string[] gameTitlesJapanese;
        public enum GameType {
            PVP,
            ThreePlayers,
            All
        };
        public GameType[] gameType;

        public string GetGameTitleJapanese(int index)
        {
            return gameTitlesJapanese[index];
        }

        public string GetGameTitleEnglish(int index)
        {
            return gameTitlesEnglish[index];
        }

        public int GetTotalGameCounts()
        {
            return gameTitlesJapanese.Length;
        }
    }
}

