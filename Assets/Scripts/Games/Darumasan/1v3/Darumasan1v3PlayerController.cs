using DHU2020.DGS.MiniGame.Setting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DHU2020.DGS.MiniGame.Darumasan.Darumasan1v3GameController;
using static DHU2020.DGS.MiniGame.Setting.PlayerInfo;

namespace DHU2020.DGS.MiniGame.Darumasan
{
    public class Darumasan1v3PlayerController : MonoBehaviour
    {
        public Darumasan1v3GameController darumasan1v3GameController;
        public PlayerInfo playerInfo;
        public float playerStandTimer = 0.9f;
        public int playerID;

        private bool playerIsRunning;
        [SerializeField] private KeyCode runKeyCodeKeyboard;
        private PlayerControllerInput playerInputMethod;
        private float playerInputTimer;
        private int playerLife;

        // Start is called before the first frame update
        void Start()
        {
            Initialization();
        }

        private void Initialization()
        {
            playerIsRunning = false;
            playerInputMethod = playerInfo.GetPlayerControllerInput(playerID);
            playerInputTimer = playerStandTimer;
        }

        // Update is called once per frame
        void Update()
        {
            GameState currentGameState = darumasan1v3GameController.GetCurrentGameState();

            if (currentGameState != GameState.GameStart && currentGameState != GameState.GhostMessageEnded)
            {
                return;
            }

            playerLife = playerInfo.GetCurrentLife(playerID);
            if (playerLife <= 0) { return; }

            if (playerInputMethod == PlayerControllerInput.Keyboard)
            {
                if (Input.GetKeyDown(runKeyCodeKeyboard))
                {
                    if (currentGameState == GameState.GameStart)
                    {
                        darumasan1v3GameController.HandlePlayersInputRun(playerID);
                    }
                    else if (currentGameState == GameState.GhostMessageEnded)
                    {
                        playerIsRunning = true;
                    }
                }
                else
                {
                    playerInputTimer += Time.deltaTime;
                    if (playerInputTimer >= playerStandTimer)
                    {
                        playerInputTimer = 0;
                        darumasan1v3GameController.PlayerStand(playerID);
                    }
                }
            }
            else if (playerInputMethod == PlayerControllerInput.Joystick)
            {
                if (Input.GetButtonDown("DarumasanP" + (playerID + 1) + "RunButton"))
                {
                    if (currentGameState == GameState.GameStart)
                    {
                        darumasan1v3GameController.HandlePlayersInputRun(playerID);
                    }
                    else if (currentGameState == GameState.GhostMessageEnded)
                    {
                        playerIsRunning = true;
                    }
                }
                else
                {
                    playerInputTimer -= Time.deltaTime;
                    if (playerInputTimer <= 0f)
                    {
                        playerInputTimer += playerStandTimer;
                        darumasan1v3GameController.PlayerStand(playerID);
                    }
                }
            }

        }

        public void SetThreePlayerSidePlayerID(int playerIndex)
        {
            playerID = playerIndex;
        }

        public void SetPlayerRunKeyCode(int playerIndex)
        {
            switch (playerIndex)
            {
                case 0:
                    runKeyCodeKeyboard = KeyCode.A;
                    break;
                case 1:
                    runKeyCodeKeyboard = KeyCode.G;
                    break;
                case 2:
                    runKeyCodeKeyboard = KeyCode.L;
                    break;
                case 3:
                    runKeyCodeKeyboard = KeyCode.Keypad4;
                    break;
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