using DHU2020.DGS.MiniGame.Setting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DHU2020.DGS.MiniGame.Darumasan.DarumasanGameController;
using static DHU2020.DGS.MiniGame.Setting.PlayerInfo;

namespace DHU2020.DGS.MiniGame.Darumasan
{
    public class DarumasanPlayerController : MonoBehaviour
    {
        public DarumasanGameController darumasanGameController;
        public PlayerInfo playerInfo;
        public int playerID;
        public KeyCode runKeyCodeKeyboard;

        private bool playerIsRunning;
        private PlayerControllerInput playerInputMethod;

        // Start is called before the first frame update
        void Start()
        {
            playerIsRunning = false;
            playerInputMethod = playerInfo.GetPlayerControllerInput(playerID);
        }

        // Update is called once per frame
        void Update()
        {
            GameState currentGameState = darumasanGameController.GetCurrentGameState();

            if (currentGameState != GameState.GameStart && currentGameState != GameState.GhostMessageEnded)
            {
                return;
            }

            if (playerInputMethod == PlayerControllerInput.Keyboard)
            {
                if (Input.GetKeyDown(runKeyCodeKeyboard))
                {
                    if (currentGameState == GameState.GameStart)
                    {
                        darumasanGameController.HandlePlayersInputRun(playerID);
                    }
                    else if (currentGameState == GameState.GhostMessageEnded)
                    {
                        playerIsRunning = true;
                    }
                }
            }
            else if(playerInputMethod == PlayerControllerInput.Joystick)
            {
                if (Input.GetButtonDown("DarumasanP"+(playerID+1)+"RunButton"))
                {
                    if (currentGameState == GameState.GameStart)
                    {
                        darumasanGameController.HandlePlayersInputRun(playerID);
                    }
                    else if (currentGameState == GameState.GhostMessageEnded)
                    {
                        playerIsRunning = true;
                    }
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