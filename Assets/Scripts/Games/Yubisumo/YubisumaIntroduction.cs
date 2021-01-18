using DHU2020.DGS.MiniGame.Map;
using DHU2020.DGS.MiniGame.Setting;
using DHU2020.DGS.MiniGame.System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DHU2020.DGS.MiniGame.Map.MapInfo;

namespace DHU2020.DGS.MiniGame.Yubisumo
{
    public class YubisumaIntroduction : MonoBehaviour
    {
        public MapInfo mapInfo;
        public Localization localeJP, localeEN;
        public float showStartGameTextTime = 3f, fadeStartGameTextTime = 0.5f;
        public Text showStartGameText, howToPlayText;
        public GameObject yubisumoGameCanvas;
        public PVPPlayerInfo pvpPlayerInfo;
        public YubisumoGameController yubisumoGameController;
        public KeyCode[] playerIDInputKeyCodes;

        private int player1ID, player2ID;
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
            gameLanguage = mapInfo.GetGameLanguage();
            if(gameLanguage == Language.Japanese)
            {
                howToPlayText.text = localeJP.GetLabelContent("HowToPlay");
                showStartGameText.text = localeJP.GetLabelContent("PressAnyButton");
            }
            else
            {
                howToPlayText.text = localeEN.GetLabelContent("HowToPlay");
                showStartGameText.text = localeEN.GetLabelContent("PressAnyButton");
            }
            player1ID = pvpPlayerInfo.GetPlayer1ID();
            player2ID = pvpPlayerInfo.GetPlayer2ID();
            showStartGameText.CrossFadeAlpha(0f, 0f, false);
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
                }
                if (timer >= 1)
                {
                    showStartGameText.CrossFadeAlpha(0f, fadeStartGameTextTime, false);
                    timer = 0;
                }

                if (Input.GetKeyDown(playerIDInputKeyCodes[player1ID]) || Input.GetKeyDown(playerIDInputKeyCodes[player2ID]) ||
                    Input.GetButtonDown("YubisumoP" + (player1ID+1)+ "HitButton") || Input.GetButtonDown("YubisumoP" + (player2ID + 1) + "HitButton"))
                {
                    yubisumoGameController.GameStart();
                    yubisumoGameCanvas.SetActive(true);
                    gameObject.SetActive(false);
                }
            }
        }

        private void ReadyToStartGame()
        {
            enterGameFlag = true;
        }
    }
}