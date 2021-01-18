using DHU2020.DGS.MiniGame.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DHU2020.DGS.MiniGame.Darumasan.DarumasanGameController;
using static DHU2020.DGS.MiniGame.Map.MapInfo;

namespace DHU2020.DGS.MiniGame.Darumasan
{
    public class DarumasanGhostTextWriter : MonoBehaviour
    {
        public MapInfo mapInfo;
        public DarumasanGameController darumasanGameController;
        public Text ghostMessageText;
        public Image ghostFaceToRightImage, ghostFaceToLeftImage;
        public float ghostMessageRandomMinTimeFactor = 0.1f, ghostMessageRandomMaxTimeFactor = 1f;
        public string ghostMessageToShowJP, ghostMessageToShowEN;

        private int characterIndex;
        private float ghostMessageTimer, ghostMessageTimerPerCharacter;
        private string ghostMessage;
        private bool showMessageFlag;
        private Language gameLanguage;

        private void Start()
        {
            Initialization();
        }

        private void Initialization()
        {
            gameLanguage = mapInfo.GetGameLanguage();
            ghostFaceToRightImage.enabled = true;
            ghostFaceToLeftImage.enabled = false;
            showMessageFlag = false;
            if (gameLanguage == Language.Japanese)
            {
                ghostMessage = ghostMessageToShowJP;
            }
            else
            {
                ghostMessage = ghostMessageToShowEN;
            }
            ghostMessageText.text = "";
            characterIndex = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if (darumasanGameController.GetCurrentGameState() == GameState.Winner || 
                darumasanGameController.GetCurrentGameState() == GameState.GameSet)
            {
                return;
            }

            if (showMessageFlag)
            {
                ghostMessageTimer -= Time.deltaTime;
                if (ghostMessageTimer <= 0f)
                {
                    ghostMessageTimerPerCharacter = Random.Range(ghostMessageRandomMinTimeFactor, ghostMessageRandomMaxTimeFactor);
                    ghostMessageTimer += ghostMessageTimerPerCharacter;
                    characterIndex++;
                    if(characterIndex >= ghostMessage.Length)
                    {
                        ghostFaceToRightImage.enabled = false;
                        ghostFaceToLeftImage.enabled = true;
                        showMessageFlag = false;
                        darumasanGameController.ShowGhostMessageEnd();
                    }
                    else
                    {
                        ghostMessageText.text = ghostMessage.Substring(0, characterIndex);
                    }
                }
            }
        }

        public void ShowGhostMessageText()
        {
            ghostFaceToRightImage.enabled = true;
            ghostFaceToLeftImage.enabled = false;
            if (gameLanguage == Language.Japanese)
            {
                ghostMessage = ghostMessageToShowJP;
            }
            else
            {
                ghostMessage = ghostMessageToShowEN;
            }
            showMessageFlag = true;
        }

        public void EmptyGhostMessageText()
        {
            ghostMessageText.text = "";
            characterIndex = 0;
            ghostMessageTimer = 0f;
        }
    }
}
