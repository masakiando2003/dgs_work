using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DHU2020.DGS.MiniGame.Darumasan.DarumasanGameController;

namespace DHU2020.DGS.MiniGame.Darumasan
{
    public class DarumasanPlayerController : MonoBehaviour
    {
        public DarumasanGameController darumasanGameController;
        public int playerID;
        public KeyCode runKeyCode;

        private bool playerIsRunning;
        
        // Start is called before the first frame update
        void Start()
        {
            playerIsRunning = false;
        }

        // Update is called once per frame
        void Update()
        {
            GameState currentGameState = darumasanGameController.GetCurrentGameState();

            if (currentGameState != GameState.GameStart && currentGameState != GameState.GhostMessageEnded)
            {
                return;
            }

            if (Input.GetKeyDown(runKeyCode))
            {
                if(currentGameState == GameState.GameStart)
                {
                    darumasanGameController.HandlePlayersInputRun(playerID);
                }
                else if (currentGameState == GameState.GhostMessageEnded)
                {
                    playerIsRunning = true;
                }
            }
        }

        public bool GetPlayerIsInRunningState()
        {
            return playerIsRunning;
        }
        
        public void ResetPlayerToNonRunningState()
        {
            playerIsRunning = false;
        }
    }
}