using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DHU2020.DGS.MiniGame.Darumasan
{
    public class Darumasan1v3Introduction : MonoBehaviour
    {
        public float showStartGameTextTime = 3f, fadeStartGameTextTime = 0.5f;
        public Text showStartGameText;
        public GameObject darumasanGameCanvas;
        public Darumasan1v3GameController darumasan1v3GameController;

        private bool enterGameFlag;
        private float timer;
        // Start is called before the first frame update
        void Start()
        {
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

                if (Input.anyKeyDown)
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