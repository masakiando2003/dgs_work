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
        public KeyCode runKeyCode;
        public float playerStandTimer = 1.5f;
        public int playerID;

        private float playerInputTimer;
        private bool playerIsRunning;
        
        // Start is called before the first frame update
        void Start()
        {
            playerIsRunning = false;
            playerInputTimer = playerStandTimer;
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
            else
            {
                if(playerID == 1)
                {
                    Debug.Log("playerInputTimer: "+ playerInputTimer);
                }
                playerInputTimer -= Time.deltaTime;
                if(playerInputTimer <= 0f)
                {
                    playerInputTimer += playerStandTimer;
                    darumasanGameController.PlayerStand(playerID);
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