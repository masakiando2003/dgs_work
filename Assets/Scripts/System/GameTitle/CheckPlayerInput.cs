using DHU2020.DGS.MiniGame.Map;
using DHU2020.DGS.MiniGame.Setting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DHU2020.DGS.MiniGame.Map.MapInfo;
using static DHU2020.DGS.MiniGame.Setting.PlayerInfo;

namespace DHU2020.DGS.MiniGame.System
{
    public class CheckPlayerInput : MonoBehaviour
    {
        public MapInfo mapInfo;
        public Localization localeJP, localeEN;
        public GameTitle gameTitle;
        public PlayerInfo playerInfo;
        public Button[] playerConfirmInputButtons;
        public Button proceedButton;
        public KeyCode[] player1TestButtons, player2TestButtons, player3TestButtons, player4TestButtons;
        public Text[] playerNameTexts, playerInputHintsTexts, playerInputTestAreaTexts, playerInputKeybordTexts, playerInputJoystickTexts;
        public Text[] playerLabelTexts, confirmedButtonText;
        public Text checkPlayerInputHintsText, checkInputTestControlHintsTitle, checkInputTestAreaTitle, playerInputTypeText;
        public Text settingHintsText, startButtonText, backButtonText, testInputHintsText;
        public int numOfTestJoystickButtons;
        public string[] playerInputKeyboardHints, playerInputButtonsSymbols;
        public string playerInputJoystickHints;

        public string chosenPlayerLabel, testInputHints, settingHints;
        private int numOfPlayers, selectedPlayerID;
        private bool[] playerInputConfirmFlags;
        private bool isSelectingPlayers, playerInputTestFlag;
        private Language gameLanguage;

        // Start is called before the first frame update
        void Start()
        {
            Initialization();
        }

        private void Initialization()
        {
            numOfPlayers = playerInfo.GetPlayersCount();
            playerInputConfirmFlags = new bool[numOfPlayers];
            selectedPlayerID = 1;
            GameObject.Find("Player" + selectedPlayerID + "NameBackground").GetComponent<Image>().color = Color.black;
            GameObject.Find("Player" + selectedPlayerID + "NameText").GetComponent<Text>().color = Color.white;
            gameLanguage = mapInfo.GetGameLanguage();
            if (gameLanguage == Language.Japanese)
            {
                checkPlayerInputHintsText.text = localeJP.GetLabelContent("CheckPlayerInputHints");
                checkInputTestControlHintsTitle.text = localeJP.GetLabelContent("InputButtonsTitle");
                checkInputTestAreaTitle.text = localeJP.GetLabelContent("InputTestTitle");
                testInputHints = localeJP.GetLabelContent("TestInputHint");
                chosenPlayerLabel = localeJP.GetLabelContent("ChosenPlayerHint");
                startButtonText.text = localeJP.GetLabelContent("Start");
                backButtonText.text = localeJP.GetLabelContent("Back");
                settingHints = localeJP.GetLabelContent("SettingtHint");
                startButtonText.text = localeJP.GetLabelContent("Start");
                playerInputTypeText.text = localeJP.GetLabelContent("PlayerInputType");
                for (int playerIndex = 0; playerIndex < numOfPlayers; playerIndex++)
                {
                    playerLabelTexts[playerIndex].text = localeJP.GetLabelContent("Player") + (playerIndex + 1);
                    playerInputKeybordTexts[playerIndex].text = localeJP.GetLabelContent("Keyboard");
                    playerInputJoystickTexts[playerIndex].text = localeJP.GetLabelContent("Joystick");
                    confirmedButtonText[playerIndex].text = localeJP.GetLabelContent("Confirmed");
                }
            }
            else
            {
                checkPlayerInputHintsText.text = localeEN.GetLabelContent("CheckPlayerInputHints");
                checkInputTestControlHintsTitle.text = localeEN.GetLabelContent("InputButtonsTitle");
                checkInputTestAreaTitle.text = localeEN.GetLabelContent("InputTestTitle");
                testInputHints = localeEN.GetLabelContent("TestInputHint");
                chosenPlayerLabel = localeEN.GetLabelContent("ChosenPlayerHint");
                startButtonText.text = localeEN.GetLabelContent("Start");
                backButtonText.text = localeEN.GetLabelContent("Back");
                settingHints = localeEN.GetLabelContent("SettingtHint");
                startButtonText.text = localeEN.GetLabelContent("Start");
                playerInputTypeText.text = localeEN.GetLabelContent("PlayerInputType");
                for (int playerIndex = 0; playerIndex < numOfPlayers; playerIndex++)
                {
                    playerLabelTexts[playerIndex].text = localeEN.GetLabelContent("Player") + (playerIndex + 1);
                    playerInputKeybordTexts[playerIndex].text = localeEN.GetLabelContent("Keyboard");
                    playerInputJoystickTexts[playerIndex].text = localeEN.GetLabelContent("Joystick");
                    confirmedButtonText[playerIndex].text = localeEN.GetLabelContent("Confirmed");
                }
            }
            for (int playerIndex = 0; playerIndex < numOfPlayers; playerIndex++)
            {
                int playerID = playerIndex + 1;
                PlayerControllerInput playerControllerInput = playerInfo.GetPlayerControllerInput(playerIndex);
                if(playerControllerInput == PlayerControllerInput.Keyboard)
                {
                    GameObject.Find("Player" + playerID + "ControllerKeyboardBackground").GetComponent<Image>().color = Color.black;
                    GameObject.Find("Player" + playerID + "ControllerKeyboardText").GetComponent<Text>().color = Color.white;
                    GameObject.Find("Player" + playerID + "ControllerJoystickBackground").GetComponent<Image>().color = Color.white;
                    GameObject.Find("Player" + playerID + "ControllerJoystickText").GetComponent<Text>().color = Color.black;
                    playerInputHintsTexts[playerIndex].text = playerInputKeyboardHints[playerIndex];
                }
                else
                {
                    GameObject.Find("Player" + playerID + "ControllerJoystickBackground").GetComponent<Image>().color = Color.black;
                    GameObject.Find("Player" + playerID + "ControllerJoystickText").GetComponent<Text>().color = Color.white;
                    GameObject.Find("Player" + playerID + "ControllerKeyboardBackground").GetComponent<Image>().color = Color.white;
                    GameObject.Find("Player" + playerID + "ControllerKeyboardText").GetComponent<Text>().color = Color.black;
                    playerInputHintsTexts[playerIndex].text = playerInputJoystickHints;
                }
                playerInputConfirmFlags[playerIndex] = false;
                playerInputTestAreaTexts[playerIndex].text = "";
                playerNameTexts[playerIndex].text = playerInfo.GetPlayerName(playerIndex);
            }
            isSelectingPlayers = true;
            playerInputTestFlag = false;
            settingHintsText.text = settingHints;
            testInputHintsText.text = "";
            proceedButton.interactable = false;
            startButtonText.color = Color.gray;

        }

        // Update is called once per frame
        void Update()
        {
            for(int playerIndex = 0; playerIndex < playerNameTexts.Length; playerIndex++)
            {
                playerNameTexts[playerIndex].text = playerInfo.GetPlayerName(playerIndex);
            }

            if (isSelectingPlayers == true)
            {
                SelectPlayer();
            }
            else if (playerInputTestFlag == true)
            {
                SelectAndTestPlayerInput(selectedPlayerID);
            }
            CheckAllPlayersConfirmInput();
        }

        private void SelectPlayer()
        {
            settingHintsText.text = settingHints;

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                GameObject.Find("Player" + selectedPlayerID + "NameBackground").GetComponent<Image>().color = Color.white;
                GameObject.Find("Player" + selectedPlayerID + "NameText").GetComponent<Text>().color = Color.black;

                selectedPlayerID = ((selectedPlayerID - 1) <= 0) ? numOfPlayers : selectedPlayerID - 1;

                GameObject.Find("Player" + selectedPlayerID + "NameBackground").GetComponent<Image>().color = Color.black;
                GameObject.Find("Player" + selectedPlayerID + "NameText").GetComponent<Text>().color = Color.white;
            }
            if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                GameObject.Find("Player" + selectedPlayerID + "NameBackground").GetComponent<Image>().color = Color.white;
                GameObject.Find("Player" + selectedPlayerID + "NameText").GetComponent<Text>().color = Color.black;

                selectedPlayerID = ((selectedPlayerID + 1) > numOfPlayers) ? 1 : selectedPlayerID + 1;

                GameObject.Find("Player" + selectedPlayerID + "NameBackground").GetComponent<Image>().color = Color.black;
                GameObject.Find("Player" + selectedPlayerID + "NameText").GetComponent<Text>().color = Color.white;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                SavePlayerInputControllerType(selectedPlayerID);
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                playerInputTestFlag = true;
                isSelectingPlayers = false;
                SetNotConfirmedInput(selectedPlayerID-1);
                settingHintsText.text = "";
            }
        }

        private void SelectAndTestPlayerInput(int selectedPlayerID)
        {
            int playerID = selectedPlayerID - 1;

            if(gameLanguage == Language.Japanese)
            {
                testInputHintsText.text = chosenPlayerLabel + " " + playerInfo.GetPlayerName(playerID) +
                                            "。操作をテストしてください。" + testInputHints;
            }
            else
            {
                testInputHintsText.text = chosenPlayerLabel + " " + playerInfo.GetPlayerName(playerID) +
                                            "。Please test the input。" + testInputHints;
            }

            if (playerInfo.GetPlayerControllerInput(playerID) == PlayerControllerInput.Keyboard)
            {
                TestKeyboardInput(playerID);
            }
            else
            {
                TestJoystickInput(playerID);
            }
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                playerInputTestFlag = false;
                isSelectingPlayers = true;
                playerInputTestAreaTexts[playerID].text = "";
                testInputHintsText.text = "";
            }
        }

        private void TestKeyboardInput(int playerID)
        {
            switch (playerID)
            {
                case 0:
                    for (int i = 0; i < player1TestButtons.Length; i++)
                    {
                        if (Input.GetKeyDown(player1TestButtons[i]))
                        {
                            playerInputTestAreaTexts[playerID].text = player1TestButtons[i].ToString();
                        }
                    }
                    break;
                case 1:
                    for (int i = 0; i < player2TestButtons.Length; i++)
                    {
                        if (Input.GetKeyDown(player2TestButtons[i]))
                        {
                            playerInputTestAreaTexts[playerID].text = player2TestButtons[i].ToString();
                        }
                    }
                    break;
                case 2:
                    for (int i = 0; i < player3TestButtons.Length; i++)
                    {
                        if (Input.GetKeyDown(player3TestButtons[i]))
                        {
                            playerInputTestAreaTexts[playerID].text = player3TestButtons[i].ToString();
                        }
                    }
                    break;
                case 3:
                    for (int i = 0; i < player4TestButtons.Length; i++)
                    {
                        if (Input.GetKeyDown(player4TestButtons[i]))
                        {
                            playerInputTestAreaTexts[playerID].text = player4TestButtons[i].ToString();
                        }
                    }
                    break;
            }
        }

        private void TestJoystickInput(int playerID)
        {
            int testJoytickPlayerID = playerID + 1;
            for(int buttonIndex = 1; buttonIndex <= numOfTestJoystickButtons; buttonIndex++)
            {
                if (Input.GetButtonDown("P"+ testJoytickPlayerID + "TestInputButton" + buttonIndex))
                {
                    playerInputTestAreaTexts[playerID].text = playerInputButtonsSymbols[buttonIndex - 1];
                }
            }
        }

        private void SavePlayerInputControllerType(int playerIndex)
        {
            int playerID = playerIndex;
            PlayerControllerInput currentPlayerControllerInput = playerInfo.GetPlayerControllerInput(playerIndex - 1);
            // 現在選択している入力操作の逆に選択する
            if (currentPlayerControllerInput == PlayerControllerInput.Keyboard)
            {
                GameObject.Find("Player" + playerID + "ControllerKeyboardBackground").GetComponent<Image>().color = Color.white;
                GameObject.Find("Player" + playerID + "ControllerKeyboardText").GetComponent<Text>().color = Color.black;
                GameObject.Find("Player" + playerID + "ControllerJoystickBackground").GetComponent<Image>().color = Color.black;
                GameObject.Find("Player" + playerID + "ControllerJoystickText").GetComponent<Text>().color = Color.white;
                playerInfo.SetPlayerControllerInput(playerIndex - 1, PlayerControllerInput.Joystick);
                playerInputHintsTexts[playerIndex - 1].text = playerInputJoystickHints;
            }
            else
            {
                GameObject.Find("Player" + playerID + "ControllerJoystickBackground").GetComponent<Image>().color = Color.white;
                GameObject.Find("Player" + playerID + "ControllerJoystickText").GetComponent<Text>().color = Color.black;
                GameObject.Find("Player" + playerID + "ControllerKeyboardBackground").GetComponent<Image>().color = Color.black;
                GameObject.Find("Player" + playerID + "ControllerKeyboardText").GetComponent<Text>().color = Color.white;
                playerInfo.SetPlayerControllerInput(playerIndex - 1, PlayerControllerInput.Keyboard);
                playerInputHintsTexts[playerIndex - 1].text = playerInputKeyboardHints[playerIndex - 1];
            }
        }

        private void CheckAllPlayersConfirmInput()
        {
            int playerReadyCounts = 0;
            for (int playerIndex = 0; playerIndex < numOfPlayers; playerIndex++)
            {
                if (playerInputConfirmFlags[playerIndex])
                {
                    playerReadyCounts++;
                }
            }
            if(playerReadyCounts == numOfPlayers)
            {
                proceedButton.interactable = true;
                startButtonText.color = Color.black;
                proceedButton.enabled = true;
            }
            else
            {
                proceedButton.interactable = false;
                startButtonText.color = Color.gray;
                proceedButton.enabled = false;
            }
        }

        public void ConfirmedInput(int playerIndex)
        {
            playerInputConfirmFlags[playerIndex] = true;
            var playerConfirmInputButtonColors = playerConfirmInputButtons[playerIndex].colors;
            playerConfirmInputButtonColors.normalColor = playerConfirmInputButtons[playerIndex].colors.disabledColor;
            playerConfirmInputButtons[playerIndex].colors = playerConfirmInputButtonColors;
            playerConfirmInputButtons[playerIndex].interactable = false;
            playerInputTestFlag = false;
            isSelectingPlayers = true;
            playerInputTestAreaTexts[playerIndex].text = "";
            testInputHintsText.text = "";
        }

        private void SetNotConfirmedInput(int playerIndex)
        {
            playerInputConfirmFlags[playerIndex] = false;
            var playerConfirmInputButtonColors = playerConfirmInputButtons[playerIndex].colors;
            playerConfirmInputButtonColors.normalColor = Color.white;
            playerConfirmInputButtons[playerIndex].colors = playerConfirmInputButtonColors;
            playerConfirmInputButtons[playerIndex].interactable = true;
        }

        public void GoToIntroduction()
        {
            gameTitle.Introduction();
        }

        public void BackToSetPlayerName()
        {
            gameTitle.SetPlayerName();
        }
    }
}
