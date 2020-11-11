using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DHU2020.DGS.MiniGame.Game
{
    [
        CreateAssetMenu(
            fileName = "GameInfo",
            menuName = "dgw_work/Settings/GameInfo"
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
        public Image[] gameImage;

        public string GetGameTitleJapanese(int index)
        {
            return gameTitlesJapanese[index];
        }

        public string GetGameTitleEnglish(int index)
        {
            return gameTitlesEnglish[index];
        }

        public string GetGameSceneNameByJapaneseName(string japaneseName)
        {
            int gameIndex = UnityEditor.ArrayUtility.IndexOf(gameTitlesJapanese, japaneseName);
            return gameTitlesEnglish[gameIndex];
        }

        public int GetTotalGameCounts()
        {
            return gameTitlesJapanese.Length;
        }

        public GameType GetGameType(int index)
        {
            return gameType[index];
        }

        public Image GetGameImage(int index)
        {
            return gameImage[index];
        }
    }
}

