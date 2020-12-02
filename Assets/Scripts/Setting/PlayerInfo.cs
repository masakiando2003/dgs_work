using DHU2020.DGS.MiniGame.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DHU2020.DGS.MiniGame.Setting
{
    [
        CreateAssetMenu(
            fileName = "PlayerInfo",
            menuName = "dgs_work/Settings/PlayerInfo"
        )
    ]
    public class PlayerInfo : ScriptableObject
    {
        public int defaultLife = 4;
        public List<string> playerNames = new List<string>();
        public List<int> playerLifes = new List<int>();
        public List<int> playerMaxLifes = new List<int>();
        private string[] defaultPlayerName = { "ダイスケ", "サトシ", "カズヤ", "ケイコ", "ユリナ", "アヤ" };

        public void RandomizePlayerName(int index)
        {
            int randomedDefaultPlayerNameID = Random.Range(0, defaultPlayerName.Length);
            
            bool randomizedFlag = false;
            while (!randomizedFlag)
            {
                if (!playerNames.Contains(defaultPlayerName[randomedDefaultPlayerNameID])){
                    playerNames[index] = defaultPlayerName[randomedDefaultPlayerNameID];
                    randomizedFlag = true;
                }
            }
        }

        public bool CheckPlayerNameExist(string playerName)
        {
            return playerNames.Contains(playerName);
        }

        // プレイヤー名入力用
        public void AddPlayerName(string playerName)
        {
            playerNames.Add(playerName);
        }

        public string GetPlayerName(int index)
        {
            return playerNames[index];
        }
        
        public int GetPlayerID(string name)
        {
            return playerNames.FindIndex(x => x.Equals(name));
        }

        public int GetNumOfPlayer()
        {
            return playerNames.Count;
        }

        public void AddPlayerLife(int life)
        {
            playerLifes.Add(life);
        }

        public void SetPlayersDefaultLife()
        {
            playerLifes.Clear();
            for (var i = 0; i < playerNames.Count; i++)
            {
                playerLifes.Add(defaultLife);
                playerMaxLifes.Add(defaultLife);
            }
        }

        public void IncreaseLife(int playerIndex, int life=1)
        {
            playerLifes[playerIndex] = (playerLifes[playerIndex] + life > defaultLife) ? defaultLife : playerLifes[playerIndex] + life;
        }

        public void DecreaseLife(int playerIndex, int life=1)
        {
            playerLifes[playerIndex] = (playerLifes[playerIndex] - life <= 0) ? 0 : playerLifes[playerIndex] - life;
        }

        public void SetCurrentLifeToMaxLife(int playerIndex)
        {
            playerLifes[playerIndex] = playerMaxLifes[playerIndex];
        }

        public int GetMaxLife(int playerIndex)
        {
            return playerMaxLifes[playerIndex];
        }

        public int GetCurrentLife(int playerIndex)
        {
            return playerLifes[playerIndex];
        }

        public int GetPlayersCount()
        {
            return playerNames.Count;
        }

    }
}
