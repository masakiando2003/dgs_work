using DHU2020.DGS.MiniGame.Map;
using DHU2020.DGS.MiniGame.Setting;
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
        public Localization localeJP, localeEN;
        public float showStartGameTextTime = 3f, fadeStartGameTextTime = 0.5f;
        public Text showStartGameText, howToPlayText;
        public GameObject darumasanGameCanvas;
        public Darumasan1v3GameController darumasan1v3GameController;
        public KeyCode p1EnterButton, p2EnterButton, p3EnterButton, p4EnterButton;

        private bool enterGameFlag;
        private float timer;
        private Language gameLanguage;

        // Start is called before the first frame update
        void Start()
        {
            gameLanguage = mapInfo.GetGameLanguage();
            if (gameLanguage == Language.Japanese)
            {
                howToPlayText.text = localeJP.GetLabelContent("HowToPlay");
                showStartGameText.text = localeJP.GetLabelContent("PressAnyButton");
            }
            else
            {
                howToPlayText.text = localeEN.GetLabelContent("HowToPlay");
                showStartGameText.text = localeEN.GetLabelContent("PressAnyButton");
            }
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

                if (Input.GetKeyDown(p1EnterButton) || Input.GetKeyDown(p2EnterButton) ||
                    Input.GetKeyDown(p3EnterButton) || Input.GetKeyDown(p4EnterButton) ||
                    Input.GetButtonDown("P1DecideButton") || Input.GetButtonDown("P2DecideButton") ||
                    Input.GetButtonDown("P3DecideButton") || Input.GetButtonDown("P4DecideButton")
                )
                {
                    darumasan1v3GameController.GameStart();
                    darumasanGameCanvas.SetActive(true);
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