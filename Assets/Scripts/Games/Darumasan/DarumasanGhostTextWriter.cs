using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DHU2020.DGS.MiniGame.Darumasan.DarumasanGameController;

namespace DHU2020.DGS.MiniGame.Darumasan
{
    public class DarumasanGhostTextWriter : MonoBehaviour
    {
        public DarumasanGameController darumasanGameController;
        public Text ghostMessageText;
        public float ghostMessageRandomMinTimeFactor = 0.1f, ghostMessageRandomMaxTimeFactor = 1f;
        public string ghostMessageToShow;

        private int characterIndex;
        private float ghostMessageTimer, ghostMessageTimerPerCharacter;
        private string ghostMessage;
        private bool showMessageFlag;

        private void Start()
        {
            showMessageFlag = false;
            ghostMessage = ghostMessageToShow;
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
            ghostMessage = ghostMessageToShow;
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
