using DHU2020.DGS.MiniGame.Map;
using DHU2020.DGS.MiniGame.Setting;
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
        public Localization localeJP, localeEN;
        public float showStartGameTextTime = 3f, fadeStartGameTextTime = 0.5f;
        public Text showStartGameText, howToPlayText;
        public GameObject kenkenpaGameCanvas;
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

                if (Input.GetKeyDown(p1HitButton1) || Input.GetKeyDown(p1HitButton2) || Input.GetKeyDown(p1HitButton3) || Input.GetKeyDown(p1HitButton4) ||
                    Input.GetKeyDown(p2HitButton1) || Input.GetKeyDown(p2HitButton2) || Input.GetKeyDown(p2HitButton3) || Input.GetKeyDown(p2HitButton4) ||
                    Input.GetKeyDown(p3HitButton1) || Input.GetKeyDown(p3HitButton2) || Input.GetKeyDown(p3HitButton3) || Input.GetKeyDown(p3HitButton4) ||
                    Input.GetKeyDown(p4HitButton1) || Input.GetKeyDown(p4HitButton2) || Input.GetKeyDown(p4HitButton3) || Input.GetKeyDown(p4HitButton4) ||
                    Input.GetButtonDown("KenkenpaP1HitButton1") || Input.GetButtonDown("KenkenpaP1HitButton2") ||
                    Input.GetButtonDown("KenkenpaP1HitButton3") || Input.GetButtonDown("KenkenpaP1HitButton4") ||
                    Input.GetButtonDown("KenkenpaP2HitButton1") || Input.GetButtonDown("KenkenpaP2HitButton2") ||
                    Input.GetButtonDown("KenkenpaP2HitButton3") || Input.GetButtonDown("KenkenpaP2HitButton4") ||
                    Input.GetButtonDown("KenkenpaP3HitButton1") || Input.GetButtonDown("KenkenpaP3HitButton2") ||
                    Input.GetButtonDown("KenkenpaP3HitButton3") || Input.GetButtonDown("KenkenpaP3HitButton4") ||
                    Input.GetButtonDown("KenkenpaP4HitButton1") || Input.GetButtonDown("KenkenpaP4HitButton2") ||
                    Input.GetButtonDown("KenkenpaP4HitButton3") || Input.GetButtonDown("KenkenpaP4HitButton4")
                )
                {
                    kenkenpaGameController.GameStart();
                    kenkenpaGameCanvas.SetActive(true);
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