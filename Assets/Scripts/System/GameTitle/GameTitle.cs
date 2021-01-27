using DHU2020.DGS.MiniGame.Map;
using DHU2020.DGS.MiniGame.Setting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

namespace DHU2020.DGS.MiniGame.System
{
    public class GameTitle : MonoBehaviour
    {
        public float showStartGameTextTime = 3f, fadeStartGameTextTime = 0.5f, controlHintsTextPosition = -480.0f;
        public MapInfo mapInfo;
        public PlayerInfo playerInfo;
        public GameObject gameTitleOptionObject, gameTitleCanvas, introductionCanvas, setPlayerNameCanvas;
        public GameObject checkInputCanvas, optionCanvas, creditCanvas;
        public GameObject[] menuItems;
        public GameTitleIntroduction gameTitleIntroduction;
        public Text startGameText, startGameText2, controlHintsText, toGameRuleText, toIntroductionText;
        public KeyCode upKey, downKey, introductionKeyCode, howToPlayKeyCode;

        private bool canControl, enterGameFlag, creditFlag, introductionFlag;
        private int menuItemIndex;
        private float startGameTimer;

        // Start is called before the first frame update
        void Start()
        {
            canControl = false;
            enterGameFlag = false;
            creditFlag = false;
            introductionFlag = false;
            menuItemIndex = 0;
            startGameTimer = 0f;
            gameTitleCanvas.SetActive(true);
            setPlayerNameCanvas.SetActive(false);
            checkInputCanvas.SetActive(false);
            introductionCanvas.SetActive(false);
            optionCanvas.SetActive(false);
            creditCanvas.SetActive(false);
            gameTitleOptionObject.SetActive(false);
            controlHintsText.enabled = false;
            toGameRuleText.enabled = false;
            toIntroductionText.enabled = false;
        }

        public void EnableGameTitlePanel()
        {
            canControl = true;
            gameTitleOptionObject.SetActive(true);
            controlHintsText.enabled = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (canControl && Input.GetKeyDown(upKey))
            {
                menuItemIndex = (menuItemIndex - 1 < 0) ? menuItems.Length - 1 : menuItemIndex - 1;
                SelectMenu(menuItemIndex);
            }
            else if (canControl && Input.GetKeyDown(downKey))
            {
                menuItemIndex = (menuItemIndex + 1 >= menuItems.Length) ? 0 : menuItemIndex + 1;
                SelectMenu(menuItemIndex);
            }
            else if((canControl) && Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                switch (menuItemIndex)
                {
                    case 0:
                        SetPlayerName();
                        break;
                        //ネットワークゲームに遷移します。
                    /*case 1:
                        NetworkGame();
                        break;*/
                    case 1:
                        GameOptions();
                        break;
                    case 2:
                        Credit();
                        break;
                }
            }
            else if (enterGameFlag)
            {
                toGameRuleText.enabled = true;
                toIntroductionText.enabled = true;
                startGameTimer = startGameTimer + Time.deltaTime;
                if (startGameTimer >= 0.5)
                {
                    startGameText.CrossFadeAlpha(1f, fadeStartGameTextTime, false);
                    startGameText2.CrossFadeAlpha(1f, fadeStartGameTextTime, false);
                }
                if (startGameTimer >= 1)
                {
                    startGameText.CrossFadeAlpha(0f, fadeStartGameTextTime, false);
                    startGameText2.CrossFadeAlpha(0f, fadeStartGameTextTime, false);
                    startGameTimer = 0;
                }
                
                if (introductionFlag && Input.GetKeyDown(KeyCode.C))
                {
                    gameTitleIntroduction.ActivateCanvas(GameTitleIntroduction.Canvas.HowToPlay);
                }
                else if (introductionFlag && Input.GetKeyDown(KeyCode.R))
                {
                    gameTitleIntroduction.ActivateCanvas(GameTitleIntroduction.Canvas.Introduction);
                }
                else if (Input.anyKeyDown)
                {
                    NewGame();
                }
            }
            else if (creditFlag)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    SceneManager.LoadScene("GameTitle");
                }
            }
        }

        private void SelectMenu(int selectedMenuItemIndex)
        {
            for(int i = 0; i < menuItems.Length; i++)
            {
                if(i == selectedMenuItemIndex)
                {
                    menuItems[i].GetComponentInChildren<Image>().color = Color.black;
                    menuItems[i].GetComponentInChildren<Text>().color = Color.white;
                }
                else
                {
                    menuItems[i].GetComponentInChildren<Image>().color = Color.white;
                    menuItems[i].GetComponentInChildren<Text>().color = Color.black;
                }
            }
        }

        public void Introduction()
        {
            canControl = false;
            creditFlag = false;
            introductionFlag = true;
            gameTitleCanvas.SetActive(false);
            optionCanvas.SetActive(false);
            creditCanvas.SetActive(false);
            setPlayerNameCanvas.SetActive(false);
            checkInputCanvas.SetActive(false);
            introductionCanvas.SetActive(true);
            startGameText.CrossFadeAlpha(0f, 0f, false);
            startGameText2.CrossFadeAlpha(0f, 0f, false);
            Invoke("ShowStartGameText", showStartGameTextTime);
        }

        private void ShowStartGameText()
        {
            enterGameFlag = true;
            creditFlag = false;
        }

        public void SetPlayerName()
        {
            canControl = false;
            creditFlag = false;
            setPlayerNameCanvas.SetActive(true);
            checkInputCanvas.SetActive(false);
            gameTitleCanvas.SetActive(false);
            introductionCanvas.SetActive(false);
            optionCanvas.SetActive(false);
            creditCanvas.SetActive(false);
        }

        public void CheckPlayerInput()
        {
            canControl = false;
            creditFlag = false;
            checkInputCanvas.SetActive(true);
            setPlayerNameCanvas.SetActive(false);
            gameTitleCanvas.SetActive(false);
            introductionCanvas.SetActive(false);
            optionCanvas.SetActive(false);
            creditCanvas.SetActive(false);
        }

        private void NewGame()
        {
            mapInfo.StartNewGame();
            playerInfo.SetPlayersDefaultLife();
            SceneManager.LoadScene("MainMap");
        }

        private void GameOptions()
        {
            canControl = false;
            creditFlag = false;
            gameTitleCanvas.SetActive(false);
            introductionCanvas.SetActive(false);
            optionCanvas.SetActive(true);
            creditCanvas.SetActive(false);
        }

        private void Credit()
        {
            canControl = false;
            creditFlag = true;
            gameTitleCanvas.SetActive(false);
            introductionCanvas.SetActive(false);
            optionCanvas.SetActive(false);
            creditCanvas.SetActive(true);
        }

        private void NetworkGame()
        {
            canControl = false;
            playerInfo.SetPlayersDefaultLife();
            SceneManager.LoadScene("NetworkGameMainMap");
        }

        public void ReturnToMenu()
        {
            SceneManager.LoadScene("GameTitle");
        }
    }
}