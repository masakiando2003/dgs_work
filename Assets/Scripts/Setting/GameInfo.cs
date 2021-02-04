using DHU2020.DGS.MiniGame.Map;
using DHU2020.DGS.MiniGame.Setting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DHU2020.DGS.MiniGame.Game
{
    [
        CreateAssetMenu(
            fileName = "GameInfo",
            menuName = "dgs_work/Settings/GameInfo"
        )
    ]
    public class GameInfo : ScriptableObject
    {
        public PlayerInfo playerInfo;
        public MapInfo mapInfo;
        public string[] gameTitlesEnglish;
        public string[] gameTitlesJapanese;
        public enum GameType {
            PVP,
            ThreePlayers,
            All,
            MultipleType
        };
        public GameType[] gameType;
        public Sprite[] gameImage;
        public int kenkenpaTime, yubisumoTime, darumasanTime, darumasanDistance;

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
            int gameIndex = Array.IndexOf(gameTitlesJapanese, japaneseName);
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

        public Sprite GetGameImage(int index)
        {
            return gameImage[index];
        }

        public GameType GetGameTypeByGameName(string gameName)
        {
            int gameID = GetGameIDByGameName(gameName);
            return gameType[gameID];
        }

        private int GetGameIDByGameName(string gameName)
        {
            return Array.IndexOf(gameTitlesEnglish, gameName);
        }

        public void SetMiniGameWinner(string miniGame, int playerID, int[] rivalIDs = null, List<int> winnerPlayerIDs = null)
        {
            if (playerID != -1) // -1 = 引き分け
            {
                GameType gameType = GetGameTypeByGameName(miniGame);
                int numOfPlayers = playerInfo.GetPlayersCount();
                switch (gameType)
                {
                    case GameType.All:
                    case GameType.MultipleType:
                        if (winnerPlayerIDs != null && winnerPlayerIDs.Count > 0)
                        {
                            for (var playerIndex = 0; playerIndex < numOfPlayers; playerIndex++)
                            {
                                if (winnerPlayerIDs.Contains(playerIndex))
                                {
                                    playerInfo.IncreaseLife(playerID);
                                }
                            }
                            for (var playerIndex = 0; playerIndex < rivalIDs.Length; playerIndex++)
                            {
                                if (!winnerPlayerIDs.Contains(rivalIDs[playerIndex]))
                                {
                                    playerInfo.DecreaseLife(rivalIDs[playerIndex]);
                                }
                            }
                        }
                        else
                        {
                            for (var playerIndex = 0; playerIndex < numOfPlayers; playerIndex++)
                            {
                                if (playerIndex == playerID)
                                {
                                    playerInfo.IncreaseLife(playerID);
                                }
                                else
                                {
                                    playerInfo.DecreaseLife(playerIndex);
                                }
                            }
                        }
                        break;
                    case GameType.PVP:
                        playerInfo.IncreaseLife(playerID);
                        playerInfo.DecreaseLife(rivalIDs[0]);
                        break;
                    case GameType.ThreePlayers:
                        playerInfo.IncreaseLife(playerID);
                        playerInfo.DecreaseLife(rivalIDs[0]);
                        playerInfo.DecreaseLife(rivalIDs[1]);
                        break;

                }
            }
            mapInfo.ProceedNextTurn();
        }

        public void SetKenkenpaTime(int seconds)
        {
            kenkenpaTime = seconds;
            PlayerPrefs.SetInt("KenkenpaTime", kenkenpaTime);
        }

        public void SetYubisumoTime(int seconds)
        {
            yubisumoTime = seconds;
            PlayerPrefs.SetInt("YubisumoTime", kenkenpaTime);
        }

        public void SetDarumasanTime(int seconds)
        {
            darumasanTime = seconds;
            PlayerPrefs.SetInt("DarumasanTime", kenkenpaTime);
        }

        public void SetDarumasanDistance(int distance)
        {
            darumasanDistance = distance;
            PlayerPrefs.SetInt("DarumasanDisntance", darumasanDistance);
        }

        public int GetKenkenpaTime()
        {
            return kenkenpaTime;
        }

        public int GetYubisumoTime()
        {
            return yubisumoTime;
        }

        public int GetDarumasanTime()
        {
            return darumasanTime;
        }

        public int GetDarumasanDistance()
        {
            return darumasanDistance;
        }
    }
}

