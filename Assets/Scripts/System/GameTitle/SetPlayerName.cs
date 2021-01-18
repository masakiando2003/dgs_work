using DHU2020.DGS.MiniGame.Map;
using DHU2020.DGS.MiniGame.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DHU2020.DGS.MiniGame.Map.MapInfo;

namespace DHU2020.DGS.MiniGame.Setting
{
    public class SetPlayerName : MonoBehaviour
    {
        public MapInfo mapInfo;
        public Localization localeJP, localeEN;
        public GameTitle gameTitleManager;
        public Text[] playerNameLabels;
        public Text inputPlayerNameHintsText, proceedToCheckPlayerInputButtonText, titleButtonText;
        public InputField player1NameInputField, player2NameInputField, player3NameInputField, player4NameInputField;
        public PlayerInfo playerInfo;
        public Button proceedToCheckPlayerInputButton;
        private Language gameLangauge;

        // Start is called before the first frame update
        void Start()
        {
            Initialization();
        }

        private void Initialization()
        {
            gameLangauge = mapInfo.GetGameLanguage();
            if (gameLangauge == Language.Japanese)
            {
                inputPlayerNameHintsText.text = localeJP.GetLabelContent("InputPlayerHints");
                string playerLabel = localeJP.GetLabelContent("PlayerLabel");
                for (int i = 0; i < playerNameLabels.Length; i++)
                {
                    playerNameLabels[i].text = playerLabel + (i + 1);
                }
                proceedToCheckPlayerInputButtonText.text = localeJP.GetLabelContent("Proceed");
                titleButtonText.text = localeJP.GetLabelContent("Title");
            }
            else
            {
                inputPlayerNameHintsText.text = localeEN.GetLabelContent("InputPlayerHints");
                string playerLabel = localeEN.GetLabelContent("PlayerLabel");
                for (int i = 0; i < playerNameLabels.Length; i++)
                {
                    playerNameLabels[i].text = playerLabel + (i + 1);
                }
                proceedToCheckPlayerInputButtonText.text = localeEN.GetLabelContent("Proceed");
                titleButtonText.text = localeEN.GetLabelContent("Title");
            }
            player1NameInputField.text = playerInfo.GetPlayerName(0);
            player2NameInputField.text = playerInfo.GetPlayerName(1);
            player3NameInputField.text = playerInfo.GetPlayerName(2);
            player4NameInputField.text = playerInfo.GetPlayerName(3);
        }

        private void Update()
        {
            if (player1NameInputField.text != "" && player2NameInputField.text != "" && 
                player3NameInputField.text != "" && player4NameInputField.text != "")
            {
                proceedToCheckPlayerInputButton.interactable = true;
            }
            else
            {
                proceedToCheckPlayerInputButton.interactable = false;
            }
        }

        public void SavePlayerNames()
        {
            playerInfo.SetPlayerName(0, player1NameInputField.text);
            playerInfo.SetPlayerName(1, player2NameInputField.text);
            playerInfo.SetPlayerName(2, player3NameInputField.text);
            playerInfo.SetPlayerName(3, player4NameInputField.text);
            gameTitleManager.CheckPlayerInput();
        }

        public void ToTitle()
        {
            gameTitleManager.ReturnToMenu();
        }
    }
}
