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
        public enum PlayerControllerInput
        {
            Keyboard,
            Joystick
        }
        public List<PlayerControllerInput> playerControllerInputs = new List<PlayerControllerInput>();

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

        public void AddPlayerName(string playerName)
        {
            playerNames.Add(playerName);
        }

        // プレイヤー名入力用
        public void SetPlayerName(int index, string playerName)
        {
            playerNames[index] = playerName;
            PlayerPrefs.SetString("Player" + (index + 1) + "Name", playerName);
        }

        public string GetPlayerName(int index)
        {
            string playerName = PlayerPrefs.GetString("Player" + (index + 1) + "Name");
            return playerName;
            //return playerNames[index];
        }
        
        public int GetPlayerID(string name)
        {
            return playerNames.FindIndex(x => x.Equals(name));
        }

        public void AddPlayerLife(int life)
        {
            playerLifes.Add(life);
        }

        public void SetPlayersDefaultLife()
        {
            playerLifes.Clear();
            playerMaxLifes.Clear();
            for (var i = 0; i < playerNames.Count; i++)
            {
                playerLifes.Add(defaultLife);
                playerMaxLifes.Add(defaultLife);
                PlayerPrefs.SetInt("Player" + (i + 1) + "Life", defaultLife);
            }
        }

        public void IncreaseLife(int playerIndex, int life=1)
        {
            playerLifes[playerIndex] = (playerLifes[playerIndex] + life > defaultLife) ? defaultLife : playerLifes[playerIndex] + life;
            PlayerPrefs.SetInt("Player" + (playerIndex + 1) + "Life", playerLifes[playerIndex]);
        }

        public void DecreaseLife(int playerIndex, int life=1)
        {
            playerLifes[playerIndex] = (playerLifes[playerIndex] - life <= 0) ? 0 : playerLifes[playerIndex] - life;
            PlayerPrefs.SetInt("Player" + (playerIndex + 1) + "Life", playerLifes[playerIndex]);
        }

        public void SetCurrentLifeToMaxLife(int playerIndex)
        {
            playerLifes[playerIndex] = playerMaxLifes[playerIndex];
            PlayerPrefs.SetInt("Player" + (playerIndex + 1) + "Life", playerLifes[playerIndex]);
        }

        public int GetMaxLife(int playerIndex)
        {
            return playerMaxLifes[playerIndex];
        }

        public int GetCurrentLife(int playerIndex)
        {
            return PlayerPrefs.GetInt("Player" + (playerIndex + 1) + "Life");
            //return playerLifes[playerIndex];
        }

        public void SetPlayerControllerInput(int index, PlayerControllerInput input)
        {
            playerControllerInputs[index] = input;
            PlayerPrefs.SetString("Player" + (index + 1) + "ControllerInput", input.ToString());
        }

        public PlayerControllerInput GetPlayerControllerInput(int index)
        {
            string controllerInput = PlayerPrefs.GetString("Player" + (index + 1) + "ControllerInput");
            if(controllerInput == "Keyboard")
            {
                return PlayerControllerInput.Keyboard;
            }
            else if (controllerInput == "Joystick")
            {
                return PlayerControllerInput.Joystick;
            }
            else
            {
                return PlayerControllerInput.Keyboard;
            }
            //return playerControllerInputs[index];
        }

        public int GetPlayersCount()
        {
            return playerNames.Count;
        }

    }
}
