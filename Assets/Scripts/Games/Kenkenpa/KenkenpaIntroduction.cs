using DHU2020.DGS.MiniGame.Map;
using DHU2020.DGS.MiniGame.Setting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DHU2020.DGS.MiniGame.Map.MapInfo;

namespace DHU2020.DGS.MiniGame.Kenkenpa
{
    public class KenkenpaIntroduction : MonoBehaviour
    {
        public MapInfo mapInfo;
        public PlayerInfo playerInfo;
        public Localization localeJP, localeEN;
        public float showStartGameTextTime = 3f, fadeStartGameTextTime = 0.5f;
        public Text showStartGameText, showStartGameText2, titleText, controlTitleText, descriptionText, controlText, gameRuleText;
        public Text player1NameText, player2NameText, player3NameText, player4NameText;
        public Text player1ControlText, player2ControlText, player3ControlText, player4ControlText;
        public GameObject kenkenpaGameCanvas, introductionBoard, controlBoard;
        public KenkenpaGameController kenkenpaGameController;
        public KeyCode p1HitButton1, p1HitButton2, p1HitButton3, p1HitButton4;
        public KeyCode p2HitButton1, p2HitButton2, p2HitButton3, p2HitButton4;
        public KeyCode p3HitButton1, p3HitButton2, p3HitButton3, p3HitButton4;
        public KeyCode p4HitButton1, p4HitButton2, p4HitButton3, p4HitButton4;

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
                    player1ControlText.text = "キーボード: "+p1HitButton1.ToString()+"、"+ p1HitButton2.ToString() + "、"+
                                              p1HitButton3.ToString() + "、" + p1HitButton4.ToString();
                }
                else
                {
                    player1ControlText.text = "ジョイスティック: □、X、△、○";
                }
                if (playerInfo.GetPlayerControllerInput(1) == PlayerInfo.PlayerControllerInput.Keyboard)
                {
                    player2ControlText.text = "キーボード: " + p2HitButton1.ToString() + "、" + p2HitButton2.ToString() + "、" +
                                              p2HitButton3.ToString() + "、" + p2HitButton4.ToString();
                }
                else
                {
                    player2ControlText.text = "ジョイスティック: □、X、△、○";
                }
                if (playerInfo.GetPlayerControllerInput(2) == PlayerInfo.PlayerControllerInput.Keyboard)
                {
                    player3ControlText.text = "キーボード: " + p3HitButton1.ToString() + "、" + p3HitButton2.ToString() + "、" +
                                              p3HitButton3.ToString() + "、" + p3HitButton4.ToString();
                }
                else
                {
                    player3ControlText.text = "ジョイスティック: □、X、△、○";
                }
                if (playerInfo.GetPlayerControllerInput(3) == PlayerInfo.PlayerControllerInput.Keyboard)
                {
                    player4ControlText.text = "キーボード: " + p4HitButton1.ToString() + "、" + p4HitButton2.ToString() + "、" +
                                              p4HitButton3.ToString() + "、" + p4HitButton4.ToString();
                }
                else
                {
                    player4ControlText.text = "ジョイスティック: □、X、△、○";
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
                    player1ControlText.text = "Keyboard: " + p1HitButton1.ToString() + ", " + p1HitButton2.ToString() + ", " +
                                              p1HitButton3.ToString() + ", " + p1HitButton4.ToString();
                }
                else
                {
                    player1ControlText.text = "Joystick: □、X、△、○";
                }
                if (playerInfo.GetPlayerControllerInput(1) == PlayerInfo.PlayerControllerInput.Keyboard)
                {
                    player2ControlText.text = "Keyboard: " + p2HitButton1.ToString() + ", " + p2HitButton2.ToString() + ", " +
                                              p2HitButton3.ToString() + ", " + p2HitButton4.ToString();
                }
                else
                {
                    player2ControlText.text = "Joystick: □、X、△、○";
                }
                if (playerInfo.GetPlayerControllerInput(2) == PlayerInfo.PlayerControllerInput.Keyboard)
                {
                    player3ControlText.text = "Keyboard: " + p3HitButton1.ToString() + ", " + p3HitButton2.ToString() + ", " +
                                              p3HitButton3.ToString() + ", " + p3HitButton4.ToString();
                }
                else
                {
                    player3ControlText.text = "Joystick: □、X、△、○";
                }
                if (playerInfo.GetPlayerControllerInput(3) == PlayerInfo.PlayerControllerInput.Keyboard)
                {
                    player4ControlText.text = "Keyboard: " + p4HitButton1.ToString() + ", " + p4HitButton2.ToString() + ", " +
                                              p4HitButton3.ToString() + ", " + p4HitButton4.ToString();
                }
                else
                {
                    player4ControlText.text = "Joystick: □、X、△、○";
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
                    (playerInfo.GetPlayerControllerInput(0) == PlayerInfo.PlayerControllerInput.Keyboard && 
                     (Input.GetKeyDown(p1HitButton1) || Input.GetKeyDown(p1HitButton2) || Input.GetKeyDown(p1HitButton3) || Input.GetKeyDown(p1HitButton4))
                    ) ||
                    
                    (playerInfo.GetPlayerControllerInput(1) == PlayerInfo.PlayerControllerInput.Keyboard &&
                     (Input.GetKeyDown(p2HitButton1) || Input.GetKeyDown(p2HitButton2) || Input.GetKeyDown(p2HitButton3) || Input.GetKeyDown(p2HitButton4))
                    ) ||

                    (playerInfo.GetPlayerControllerInput(1) == PlayerInfo.PlayerControllerInput.Keyboard &&
                     (Input.GetKeyDown(p3HitButton1) || Input.GetKeyDown(p3HitButton2) || Input.GetKeyDown(p3HitButton3) || Input.GetKeyDown(p3HitButton4))
                    ) ||

                    (playerInfo.GetPlayerControllerInput(1) == PlayerInfo.PlayerControllerInput.Keyboard &&
                     (Input.GetKeyDown(p4HitButton1) || Input.GetKeyDown(p4HitButton2) || Input.GetKeyDown(p4HitButton3) || Input.GetKeyDown(p4HitButton4))
                    ) ||

                    (playerInfo.GetPlayerControllerInput(0) == PlayerInfo.PlayerControllerInput.Joystick &&
                     (Input.GetButtonDown("KenkenpaP1HitButton1") || Input.GetButtonDown("KenkenpaP1HitButton2") ||
                      Input.GetButtonDown("KenkenpaP1HitButton3") || Input.GetButtonDown("KenkenpaP1HitButton4"))
                    ) ||

                    (playerInfo.GetPlayerControllerInput(1) == PlayerInfo.PlayerControllerInput.Joystick &&
                     (Input.GetButtonDown("KenkenpaP2HitButton1") || Input.GetButtonDown("KenkenpaP2HitButton2") ||
                      Input.GetButtonDown("KenkenpaP2HitButton3") || Input.GetButtonDown("KenkenpaP2HitButton4"))
                    ) ||

                    (playerInfo.GetPlayerControllerInput(2) == PlayerInfo.PlayerControllerInput.Joystick &&
                     (Input.GetButtonDown("KenkenpaP3HitButton1") || Input.GetButtonDown("KenkenpaP3HitButton2") ||
                      Input.GetButtonDown("KenkenpaP3HitButton3") || Input.GetButtonDown("KenkenpaP3HitButton4"))
                    ) ||

                    (playerInfo.GetPlayerControllerInput(3) == PlayerInfo.PlayerControllerInput.Joystick &&
                     (Input.GetButtonDown("KenkenpaP4HitButton1") || Input.GetButtonDown("KenkenpaP4HitButton2") ||
                      Input.GetButtonDown("KenkenpaP4HitButton3") || Input.GetButtonDown("KenkenpaP4HitButton4"))
                    )
                )
                {
                    kenkenpaGameController.GameStart();
                    kenkenpaGameCanvas.SetActive(true);
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