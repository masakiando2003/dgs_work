﻿using DHU2020.DGS.MiniGame.Map;
using DHU2020.DGS.MiniGame.Setting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace DHU2020.DGS.MiniGame.System
{
    public class GameTitle : MonoBehaviour
    {
        public float showStartGameTextTime = 3f, fadeStartGameTextTime = 0.5f;
        public MapInfo mapInfo;
        public PlayerInfo playerInfo;
        public GameObject gameTitleOptionObject, gameTitleCanvas, introductionCanvas, optionCanvas;
        public GameObject[] menuItems;
        public Text startGameText;
        public KeyCode upKey, downKey;

        private bool canControl, enterGameFlag;
        private int menuItemIndex;
        private float startGameTimer;

        // Start is called before the first frame update
        void Start()
        {
            canControl = false;
            enterGameFlag = false;
            menuItemIndex = 0;
            startGameTimer = 0f;
            gameTitleCanvas.SetActive(true);
            introductionCanvas.SetActive(false);
            optionCanvas.SetActive(false);
            gameTitleOptionObject.SetActive(false);
        }

        public void EnableGameTitlePanel()
        {
            canControl = true;
            gameTitleOptionObject.SetActive(true);
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
            else if((canControl) && Input.GetKeyDown(KeyCode.Return))
            {
                switch (menuItemIndex)
                {
                    case 0:
                        Introduction();
                        break;
                    case 1:
                        GameOptions();
                        break;
                }
            }
            else if (enterGameFlag)
            {
                startGameTimer = startGameTimer + Time.deltaTime;
                if (startGameTimer >= 0.5)
                {
                    startGameText.CrossFadeAlpha(1f, fadeStartGameTextTime, false);
                }
                if (startGameTimer >= 1)
                {
                    startGameText.CrossFadeAlpha(0f, fadeStartGameTextTime, false);
                    startGameTimer = 0;
                }

                if (Input.anyKeyDown)
                {
                    NewGame();
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

        private void Introduction()
        {
            canControl = false;
            gameTitleCanvas.SetActive(false);
            optionCanvas.SetActive(false);
            introductionCanvas.SetActive(true);
            startGameText.CrossFadeAlpha(0f, 0f, false);
            Invoke("ShowStartGameText", showStartGameTextTime);
        }

        private void ShowStartGameText()
        {
            enterGameFlag = true;
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
            gameTitleCanvas.SetActive(false);
            introductionCanvas.SetActive(false);
            optionCanvas.SetActive(true);
        }

        public void ReturnToMenu()
        {
            SceneManager.LoadScene("GameTitle");
        }
    }
}