using DHU2020.DGS.MiniGame.Setting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DHU2020.DGS.MiniGame.Kenkenpa.KenkenpaGameController;
using static DHU2020.DGS.MiniGame.Setting.PlayerInfo;

namespace DHU2020.DGS.MiniGame.Kenkenpa
{
    public class KenkenpaPlayerController : MonoBehaviour
    {
        public KenkenpaGameController kenkenpaGameController;
        public PlayerInfo playerInfo;
        public float handlePlayerInputTime = 0.5f;
        public int playerID;
        public KeyCode hitButton1, hitButton2, hitButton3, hitButton4;

        private int playerLife;
        private bool buttonPressed;
        private float currenyPlayerInputTime;
        [SerializeField] private List<KeyCode> buttonEntered = new List<KeyCode>();
        private List<KeyCode> playerButtons = new List<KeyCode>();
        private PlayerControllerInput playerInputMethod;

        // Start is called before the first frame update
        void Start()
        {
            buttonPressed = false;
            currenyPlayerInputTime = 0f;

            buttonEntered.Clear();
            playerButtons.Clear();
            playerButtons.Add(hitButton1);
            playerButtons.Add(hitButton2);
            playerButtons.Add(hitButton3);
            playerButtons.Add(hitButton4);
            playerInputMethod = playerInfo.GetPlayerControllerInput(playerID);

            playerLife = playerInfo.GetCurrentLife(playerID);
        }

        // Update is called once per frame
        void Update()
        {
            GameState currentGameState = kenkenpaGameController.GetCurrentGameState();
            if (currentGameState != GameState.GameStart) { return; }
            if (playerLife <= 0) { return; }

            if (!buttonPressed)
            {
                currenyPlayerInputTime += Time.deltaTime;

                if(playerInputMethod == PlayerControllerInput.Keyboard)
                {
                    if (Input.GetKey(hitButton1))
                    {
                        if (!buttonEntered.Contains(hitButton1))
                        {
                            buttonEntered.Add(hitButton1);
                        }
                    }
                    if (Input.GetKey(hitButton2))
                    {
                        if (!buttonEntered.Contains(hitButton2))
                        {
                            buttonEntered.Add(hitButton2);
                        }
                    }
                    if (Input.GetKey(hitButton3))
                    {
                        if (!buttonEntered.Contains(hitButton3))
                        {
                            buttonEntered.Add(hitButton3);
                        }
                    }
                    if (Input.GetKey(hitButton4))
                    {
                        if (!buttonEntered.Contains(hitButton4))
                        {
                            buttonEntered.Add(hitButton4);
                        }
                    }
                }
                else if (playerInputMethod == PlayerControllerInput.Joystick)
                {
                    if (Input.GetButton("KenkenpaP" + (playerID + 1) +"HitButton1"))
                    {
                        if (!buttonEntered.Contains(hitButton1))
                        {
                            buttonEntered.Add(hitButton1);
                        }
                    }
                    if (Input.GetButton("KenkenpaP" + (playerID + 1) + "HitButton2"))
                    {
                        if (!buttonEntered.Contains(hitButton2))
                        {
                            buttonEntered.Add(hitButton2);
                        }
                    }
                    if (Input.GetButton("KenkenpaP" + (playerID + 1) + "HitButton3"))
                    {
                        if (!buttonEntered.Contains(hitButton3))
                        {
                            buttonEntered.Add(hitButton3);
                        }
                    }
                    if (Input.GetButton("KenkenpaP" + (playerID + 1) + "HitButton4"))
                    {
                        if (!buttonEntered.Contains(hitButton4))
                        {
                            buttonEntered.Add(hitButton4);
                        }
                    }
                }
                
                if (currenyPlayerInputTime >= handlePlayerInputTime)
                {
                    buttonPressed = true;
                }

            }
            else
            {
                kenkenpaGameController.PlayerButtonPressed(playerID);
            }
        }

        public List<KeyCode> GetPlayerButtons()
        {
            return playerButtons;
        }

        public List<KeyCode> GetEnteredButtons()
        {
            return buttonEntered;
        }

        public void SetButtonNotPressed()
        {
            buttonPressed = false;
            currenyPlayerInputTime = 0f;
            buttonEntered.Clear();
        }
    }
}