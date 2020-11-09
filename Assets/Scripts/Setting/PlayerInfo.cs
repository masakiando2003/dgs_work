using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DHU2020.DGS.MiniGame.Setting
{
    [
        CreateAssetMenu(
            fileName = "PlayerInfo",
            menuName = "dgw_work/Settings/PlayerInfo"
        )
    ]
    public class PlayerInfo : ScriptableObject
    {
        public List<string> playerNames = new List<string>();
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

    }
}
