using DHU2020.DGS.MiniGame.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DHU2020.DGS.MiniGame.Setting
{
    public class SetPlayerName : MonoBehaviour
    {
        public GameTitle gameTitleManager;
        public InputField player1NameInputField, player2NameInputField, player3NameInputField, player4NameInputField;
        public PlayerInfo playerInfo;

        // Start is called before the first frame update
        void Start()
        {
            player1NameInputField.text = playerInfo.GetPlayerName(0);
            player2NameInputField.text = playerInfo.GetPlayerName(1);
            player3NameInputField.text = playerInfo.GetPlayerName(2);
            player4NameInputField.text = playerInfo.GetPlayerName(3);
        }

        public void SavePlayerNames()
        {
            playerInfo.SetPlayerName(0, player1NameInputField.text);
            playerInfo.SetPlayerName(1, player2NameInputField.text);
            playerInfo.SetPlayerName(2, player3NameInputField.text);
            playerInfo.SetPlayerName(3, player4NameInputField.text);
            gameTitleManager.Introduction();
        }

        public void ToTitle()
        {
            gameTitleManager.ReturnToMenu();
        }
    }
}
