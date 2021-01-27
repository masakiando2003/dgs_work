using DHU2020.DGS.MiniGame.Map;
using DHU2020.DGS.MiniGame.Setting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DHU2020.DGS.MiniGame.Map.MapInfo;

namespace DHU2020.DGS.MiniGame.Darumasan
{
    public class DarumasanIntroduction : MonoBehaviour
    {
        public MapInfo mapInfo;
        public PlayerInfo playerInfo;
        public Localization localeJP, localeEN;
        public float showStartGameTextTime = 3f, fadeStartGameTextTime = 0.5f;
        public Text showStartGameText, showStartGameText2, titleText, controlTitleText, descriptionText, controlText, gameRuleText;
        public Text player1NameText, player2NameText, player3NameText, player4NameText;
        public Text player1ControlText, player2ControlText, player3ControlText, player4ControlText;
        public GameObject darumasanGameCanvas, introductionBoard, controlBoard;
        public DarumasanGameController darumasanGameController;
        public KeyCode p1EnterButton, p2EnterButton, p3EnterButton, p4EnterButton;

        private bool enterGameFlag;
        private float timer;
        private Language gameLanguage;

        // Start is called before the first frame update
        void Start()
        {
            Initialization();
        }

        private void Initialization()
        {
            introductionBoard.SetActive(true);
            controlBoard.SetActive(false);
            gameLanguage = mapInfo.GetGameLanguage();
            player1NameText.text = playerInfo.GetPlayerName(0) + ":";
            player2NameText.text = playerInfo.GetPlayerName(1) + ":";
            player3NameText.text = playerInfo.GetPlayerName(2) + ":";
            player4NameText.text = playerInfo.GetPlayerName(3) + ":";
            if (gameLanguage == Language.Japanese)
            {
                titleText.text = localeJP.GetLabelContent("Title");
                controlTitleText.text = localeJP.GetLabelContent("ControlTitle");
                showStartGameText.text = localeJP.GetLabelContent("PressAnyButton");
                showStartGameText2.text = localeJP.GetLabelContent("PressAnyButton");
                descriptionText.text = localeJP.GetLabelContent("Description").Replace("_", Environment.NewLine);
                controlText.text = localeJP.GetLabelContent("Controls");
                gameRuleText.text = localeJP.GetLabelContent("GameRule");
                if (playerInfo.GetPlayerControllerInput(0) == PlayerInfo.PlayerControllerInput.Keyboard)
                {
                    player1ControlText.text = "キーボード: " + p1EnterButton.ToString() + "を連打する = 走る";
                }
                else
                {
                    player1ControlText.text = "ジョイスティック: ○を連打する = 走る";
                }
                if (playerInfo.GetPlayerControllerInput(1) == PlayerInfo.PlayerControllerInput.Keyboard)
                {
                    player2ControlText.text = "キーボード: " + p2EnterButton.ToString() + "を連打する = 走る";
                }
                else
                {
                    player2ControlText.text = "ジョイスティック: ○を連打する = 走る";
                }
                if (playerInfo.GetPlayerControllerInput(2) == PlayerInfo.PlayerControllerInput.Keyboard)
                {
                    player3ControlText.text = "キーボード: " + p3EnterButton.ToString() + "を連打する = 走る";
                }
                else
                {
                    player3ControlText.text = "ジョイスティック: ○を連打する = 走る";
                }
                if (playerInfo.GetPlayerControllerInput(3) == PlayerInfo.PlayerControllerInput.Keyboard)
                {
                    player4ControlText.text = "キーボード: " + p4EnterButton.ToString() + "を連打する = 走る";
                }
                else
                {
                    player4ControlText.text = "ジョイスティック: ○を連打する = 走る";
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
                if (playerInfo.GetPlayerControllerInput(0) == PlayerInfo.PlayerControllerInput.Keyboard)
                {
                    player1ControlText.text = "Keyboard: Press" + p1EnterButton.ToString() + "rapidly = Run";
                }
                else
                {
                    player1ControlText.text = "Joystick: Press ○ rapaidly = Run";
                }
                if (playerInfo.GetPlayerControllerInput(1) == PlayerInfo.PlayerControllerInput.Keyboard)
                {
                    player2ControlText.text = "Keyboard: Press " + p2EnterButton.ToString() + " rapidly = Run";
                }
                else
                {
                    player2ControlText.text = "Joystick: Press ○ rapaidly = Run";
                }
                if (playerInfo.GetPlayerControllerInput(2) == PlayerInfo.PlayerControllerInput.Keyboard)
                {
                    player3ControlText.text = "Keyboard: Press " + p3EnterButton.ToString() + " rapidly = Run";
                }
                else
                {
                    player3ControlText.text = "Joystick: Press ○ rapaidly = Run";
                }
                if (playerInfo.GetPlayerControllerInput(3) == PlayerInfo.PlayerControllerInput.Keyboard)
                {
                    player4ControlText.text = "Keyboard: Press" + p4EnterButton.ToString() + "rapidly = Run";
                }
                else
                {
                    player4ControlText.text = "Joystick: Press ○ rapaidly = Run";
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
                    darumasanGameController.GameStart();
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