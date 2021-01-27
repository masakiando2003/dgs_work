using DHU2020.DGS.MiniGame.Map;
using DHU2020.DGS.MiniGame.Setting;
using DHU2020.DGS.MiniGame.System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DHU2020.DGS.MiniGame.Map.MapInfo;

namespace DHU2020.DGS.MiniGame.Darumasan
{
    public class Darumasan1v3Introduction : MonoBehaviour
    {
        public MapInfo mapInfo;
        public PlayerInfo playerInfo;
        public OneVSThreePlayerInfo oneVSThreePlayerInfo;
        public Localization localeJP, localeEN;
        public float showStartGameTextTime = 3f, fadeStartGameTextTime = 0.5f;
        public Text showStartGameText, showStartGameText2, titleText, descriptionText, controlText, controlTitleText, gameRuleText;
        public Text playersText, ghostPlayerText, player1NameText, player2NameText, player3NameText, ghostPlayerNameText;
        public Text player1ControlText, player2ControlText, player3ControlText, ghostPlayerControlText;
        public GameObject darumasanGameCanvas, introductionBoard, controlBoard;
        public Darumasan1v3GameController darumasan1v3GameController;
        public KeyCode p1EnterButton, p2EnterButton, p3EnterButton, p4EnterButton;

        private int player1ID, player2ID, player3ID, ghostPlayerID;
        private KeyCode player1RunButton, player2RunButton, player3RunButton, ghostPlayerHitButton;
        private bool enterGameFlag;
        private float timer;
        private Language gameLanguage;

        // Start is called before the first frame update
        void Start()
        {
            introductionBoard.SetActive(true);
            controlBoard.SetActive(false);
            gameLanguage = mapInfo.GetGameLanguage();
            player1ID = oneVSThreePlayerInfo.GetThreePlayerSidePlayerID(0);
            switch (player1ID)
            {
                case 0:
                    player1RunButton = p1EnterButton;
                    break;
                case 1:
                    player1RunButton = p2EnterButton;
                    break;
                case 2:
                    player1RunButton = p3EnterButton;
                    break;
                case 3:
                    player1RunButton = p4EnterButton;
                    break;
            }
            player2ID = oneVSThreePlayerInfo.GetThreePlayerSidePlayerID(1);
            switch (player2ID)
            {
                case 0:
                    player2RunButton = p1EnterButton;
                    break;
                case 1:
                    player2RunButton = p2EnterButton;
                    break;
                case 2:
                    player2RunButton = p3EnterButton;
                    break;
                case 3:
                    player2RunButton = p4EnterButton;
                    break;
            }
            player3ID = oneVSThreePlayerInfo.GetThreePlayerSidePlayerID(2);
            switch (player3ID)
            {
                case 0:
                    player3RunButton = p1EnterButton;
                    break;
                case 1:
                    player3RunButton = p2EnterButton;
                    break;
                case 2:
                    player3RunButton = p3EnterButton;
                    break;
                case 3:
                    player3RunButton = p4EnterButton;
                    break;
            }
            ghostPlayerID = oneVSThreePlayerInfo.GetOnePlayerSidePlayerID();
            switch (ghostPlayerID)
            {
                case 0:
                    ghostPlayerHitButton = p1EnterButton;
                    break;
                case 1:
                    ghostPlayerHitButton = p2EnterButton;
                    break;
                case 2:
                    ghostPlayerHitButton = p3EnterButton;
                    break;
                case 3:
                    ghostPlayerHitButton = p4EnterButton;
                    break;
            }
            player1NameText.text = playerInfo.GetPlayerName(player1ID) + ":";
            player2NameText.text = playerInfo.GetPlayerName(player2ID) + ":";
            player3NameText.text = playerInfo.GetPlayerName(player3ID) + ":";
            ghostPlayerNameText.text = playerInfo.GetPlayerName(ghostPlayerID) + ":";
            if (gameLanguage == Language.Japanese)
            {
                titleText.text = localeJP.GetLabelContent("Title");
                controlTitleText.text = localeJP.GetLabelContent("ControlTitle");
                showStartGameText.text = localeJP.GetLabelContent("PressAnyButton");
                showStartGameText2.text = localeJP.GetLabelContent("PressAnyButton");
                descriptionText.text = localeJP.GetLabelContent("Description").Replace("_", Environment.NewLine);
                controlText.text = localeJP.GetLabelContent("Controls");
                gameRuleText.text = localeJP.GetLabelContent("GameRule");
                if (playerInfo.GetPlayerControllerInput(player1ID) == PlayerInfo.PlayerControllerInput.Keyboard)
                {
                    player1ControlText.text = "キーボード: " + player1RunButton.ToString() + "を連打する = 走る";
                }
                else
                {
                    player1ControlText.text = "ジョイスティック: □、X、△、○";
                }
                if (playerInfo.GetPlayerControllerInput(player2ID) == PlayerInfo.PlayerControllerInput.Keyboard)
                {
                    player2ControlText.text = "キーボード: " + player2RunButton.ToString() + "を連打する = 走る";
                }
                else
                {
                    player2ControlText.text = "ジョイスティック: ○を連打する = 走る";
                }
                if (playerInfo.GetPlayerControllerInput(player3ID) == PlayerInfo.PlayerControllerInput.Keyboard)
                {
                    player3ControlText.text = "キーボード: " + player3RunButton.ToString() + "を連打する = 走る";
                }
                else
                {
                    player3ControlText.text = "ジョイスティック: ○を連打する = 走る";
                }
                if (playerInfo.GetPlayerControllerInput(ghostPlayerID) == PlayerInfo.PlayerControllerInput.Keyboard)
                {
                    ghostPlayerControlText.text = "キーボード: " + ghostPlayerHitButton.ToString() + "を押す = 画面上に一つ文字を表示する";
                }
                else
                {
                    ghostPlayerControlText.text = "ジョイスティック: ○を押す = 画面上に一つ文字を表示する";
                }
            }
            else
            {
                titleText.text = localeEN.GetLabelContent("Title");
                controlTitleText.text = localeEN.GetLabelContent("ControlTitle");
                showStartGameText.text = localeEN.GetLabelContent("PressAnyButton");
                showStartGameText2.text = localeEN.GetLabelContent("PressAnyButton");
                descriptionText.text = localeEN.GetLabelContent("Description").Replace("_", Environment.NewLine);
                controlText.text = localeEN.GetLabelContent("Controls");
                gameRuleText.text = localeEN.GetLabelContent("GameRule");
                if (playerInfo.GetPlayerControllerInput(player1ID) == PlayerInfo.PlayerControllerInput.Keyboard)
                {
                    player1ControlText.text = "Keyboard: Press " + player1RunButton.ToString() + " rapidly = Run";
                }
                else
                {
                    player1ControlText.text = "Joystick: Press ○ rapaidly = Run";
                }
                if (playerInfo.GetPlayerControllerInput(player2ID) == PlayerInfo.PlayerControllerInput.Keyboard)
                {
                    player2ControlText.text = "Keyboard: Press " + player2RunButton.ToString() + " rapidly = Run";
                }
                else
                {
                    player2ControlText.text = "Joystick: Press ○ rapaidly = Run";
                }
                if (playerInfo.GetPlayerControllerInput(player3ID) == PlayerInfo.PlayerControllerInput.Keyboard)
                {
                    player3ControlText.text = "Keyboard: Press " + player3RunButton.ToString() + " rapidly = Run";
                }
                else
                {
                    player3ControlText.text = "Joystick: Press ○ rapaidly = Run";
                }
                if (playerInfo.GetPlayerControllerInput(ghostPlayerID) == PlayerInfo.PlayerControllerInput.Keyboard)
                {
                    ghostPlayerControlText.text = "Keyboard: Press " + ghostPlayerHitButton.ToString() + " = Show letter once per time on the screen";
                }
                else
                {
                    ghostPlayerControlText.text = "Joystick: Press ○ = Show letter once per time on the screen";
                }
            }
            showStartGameText.CrossFadeAlpha(0f, 0f, false);
            showStartGameText2.CrossFadeAlpha(0f, 0f, false);
            controlText.enabled = false;
            gameRuleText.enabled = false;
            enterGameFlag = false;
            timer = 0f;
            Invoke("ReadyToStartGame", showStartGameTextTime);
        }

        // Update is called once per frame
        void Update()
        {
            if (!enterGameFlag)
            {
                return;
            }
            else
            {
                timer = timer + Time.deltaTime;
                if (timer >= 0.5)
                {
                    showStartGameText.CrossFadeAlpha(1f, fadeStartGameTextTime, false);
                    showStartGameText2.CrossFadeAlpha(1f, fadeStartGameTextTime, false);
                }
                if (timer >= 1)
                {
                    showStartGameText.CrossFadeAlpha(0f, fadeStartGameTextTime, false);
                    showStartGameText2.CrossFadeAlpha(0f, fadeStartGameTextTime, false);
                    timer = 0;
                }

                if (
                    (playerInfo.GetPlayerControllerInput(0) == PlayerInfo.PlayerControllerInput.Keyboard && Input.GetKeyDown(p1EnterButton)) ||
                    (playerInfo.GetPlayerControllerInput(1) == PlayerInfo.PlayerControllerInput.Keyboard && Input.GetKeyDown(p2EnterButton)) ||
                    (playerInfo.GetPlayerControllerInput(2) == PlayerInfo.PlayerControllerInput.Keyboard && Input.GetKeyDown(p3EnterButton)) ||
                    (playerInfo.GetPlayerControllerInput(3) == PlayerInfo.PlayerControllerInput.Keyboard && Input.GetKeyDown(p4EnterButton)) ||
                    (playerInfo.GetPlayerControllerInput(0) == PlayerInfo.PlayerControllerInput.Joystick && Input.GetButtonDown("P1DecideButton")) ||
                    (playerInfo.GetPlayerControllerInput(1) == PlayerInfo.PlayerControllerInput.Joystick && Input.GetButtonDown("P2DecideButton")) ||
                    (playerInfo.GetPlayerControllerInput(2) == PlayerInfo.PlayerControllerInput.Joystick && Input.GetButtonDown("P3DecideButton")) ||
                    (playerInfo.GetPlayerControllerInput(3) == PlayerInfo.PlayerControllerInput.Joystick && Input.GetButtonDown("P4DecideButton"))
                )
                {
                    darumasan1v3GameController.GameStart();
                    darumasanGameCanvas.SetActive(true);
                    gameObject.SetActive(false);
                }

                if (Input.GetKeyDown(KeyCode.C))
                {
                    introductionBoard.SetActive(false);
                    controlBoard.SetActive(true);
                }

                if (Input.GetKeyDown(KeyCode.R))
                {
                    introductionBoard.SetActive(true);
                    controlBoard.SetActive(false);
                }
            }
        }

        private void ReadyToStartGame()
        {
            enterGameFlag = true;
            controlText.enabled = true;
            gameRuleText.enabled = true;
        }
    }
}