using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DHU2020.DGS.MiniGame.Kenkenpa.KenkenpaGameController;

namespace DHU2020.DGS.MiniGame.Kenkenpa
{
    public class KenkenpaPlayerController : MonoBehaviour
    {
        public KenkenpaGameController kenkenpaGameController;
        public int playerID;
        public KeyCode hitButton1, hitButton2, hitButton3, hitButton4;

        private bool buttonPressed;
        [SerializeField] private List<KeyCode> buttonEntered = new List<KeyCode>();
        private List<KeyCode> playerButtons = new List<KeyCode>();

        // Start is called before the first frame update
        void Start()
        {
            buttonPressed = false;

            buttonEntered.Clear();
            playerButtons.Clear();
            playerButtons.Add(hitButton1);
            playerButtons.Add(hitButton2);
            playerButtons.Add(hitButton3);
            playerButtons.Add(hitButton4);
        }

        // Update is called once per frame
        void Update()
        {
            GameState currentGameState = kenkenpaGameController.GetCurrentGameState();
            if (currentGameState != GameState.GameStart) { return; }

            if (!buttonPressed)
            {
                if (Input.GetKeyDown(hitButton1))
                {
                    buttonEntered.Clear();
                    if (Input.GetKeyDown(hitButton2))
                    {
                        if (!buttonEntered.Contains(hitButton1))
                        {
                            buttonEntered.Add(hitButton1);
                        }
                        if (!buttonEntered.Contains(hitButton2))
                        {
                            buttonEntered.Add(hitButton2);
                        }
                    }
                    else if (Input.GetKeyDown(hitButton3))
                    {
                        if (!buttonEntered.Contains(hitButton1))
                        {
                            buttonEntered.Add(hitButton1);
                        }
                        if (!buttonEntered.Contains(hitButton3))
                        {
                            buttonEntered.Add(hitButton3);
                        }
                    }
                    else if (Input.GetKeyDown(hitButton4))
                    {
                        if (!buttonEntered.Contains(hitButton1))
                        {
                            buttonEntered.Add(hitButton1);
                        }
                        if (!buttonEntered.Contains(hitButton4))
                        {
                            buttonEntered.Add(hitButton4);
                        }
                    }
                    else
                    {
                        if (!buttonEntered.Contains(hitButton1))
                        {
                            buttonEntered.Add(hitButton1);
                        }
                    }
                }
                if (Input.GetKeyDown(hitButton2))
                {
                    buttonEntered.Clear();
                    if (Input.GetKeyDown(hitButton1))
                    {
                        if (!buttonEntered.Contains(hitButton2))
                        {
                            buttonEntered.Add(hitButton2);
                        }
                        if (!buttonEntered.Contains(hitButton1))
                        {
                            buttonEntered.Add(hitButton1);
                        }
                    }
                    else if (Input.GetKeyDown(hitButton3))
                    {
                        if (!buttonEntered.Contains(hitButton2))
                        {
                            buttonEntered.Add(hitButton2);
                        }
                        if (!buttonEntered.Contains(hitButton3))
                        {
                            buttonEntered.Add(hitButton3);
                        }
                    }
                    else if (Input.GetKeyDown(hitButton4))
                    {
                        if (!buttonEntered.Contains(hitButton2))
                        {
                            buttonEntered.Add(hitButton2);
                        }
                        if (!buttonEntered.Contains(hitButton4))
                        {
                            buttonEntered.Add(hitButton4);
                        }
                    }
                    else
                    {
                        if (!buttonEntered.Contains(hitButton2))
                        {
                            buttonEntered.Add(hitButton2);
                        }
                    }
                }
                if (Input.GetKeyDown(hitButton3))
                {
                    buttonEntered.Clear();
                    if (Input.GetKeyDown(hitButton1))
                    {
                        if (!buttonEntered.Contains(hitButton3))
                        {
                            buttonEntered.Add(hitButton3);
                        }
                        if (!buttonEntered.Contains(hitButton1))
                        {
                            buttonEntered.Add(hitButton1);
                        }
                    }
                    else if (Input.GetKeyDown(hitButton2))
                    {
                        if (!buttonEntered.Contains(hitButton3))
                        {
                            buttonEntered.Add(hitButton3);
                        }
                        if (!buttonEntered.Contains(hitButton2))
                        {
                            buttonEntered.Add(hitButton2);
                        }
                    }
                    else if (Input.GetKeyDown(hitButton4))
                    {
                        if (!buttonEntered.Contains(hitButton3))
                        {
                            buttonEntered.Add(hitButton3);
                        }
                        if (!buttonEntered.Contains(hitButton4))
                        {
                            buttonEntered.Add(hitButton4);
                        }
                    }
                    else
                    {
                        if (!buttonEntered.Contains(hitButton3))
                        {
                            buttonEntered.Add(hitButton3);
                        }
                    }
                }
                if (Input.GetKeyDown(hitButton4))
                {
                    buttonEntered.Clear();
                    if (Input.GetKeyDown(hitButton1))
                    {
                        if (!buttonEntered.Contains(hitButton4))
                        {
                            buttonEntered.Add(hitButton4);
                        }
                        if (!buttonEntered.Contains(hitButton1))
                        {
                            buttonEntered.Add(hitButton1);
                        }
                    }
                    else if (Input.GetKeyDown(hitButton2))
                    {
                        if (!buttonEntered.Contains(hitButton4))
                        {
                            buttonEntered.Add(hitButton4);
                        }
                        if (!buttonEntered.Contains(hitButton2))
                        {
                            buttonEntered.Add(hitButton2);
                        }
                    }
                    else if (Input.GetKeyDown(hitButton3))
                    {
                        if (!buttonEntered.Contains(hitButton4))
                        {
                            buttonEntered.Add(hitButton4);
                        }
                        if (!buttonEntered.Contains(hitButton3))
                        {
                            buttonEntered.Add(hitButton3);
                        }
                    }
                    else
                    {
                        if (!buttonEntered.Contains(hitButton4))
                        {
                            buttonEntered.Add(hitButton4);
                        }
                    }
                }
                buttonPressed = Input.GetKeyDown(hitButton1) |
                 Input.GetKeyDown(hitButton2) |
                 Input.GetKeyDown(hitButton3) |
                 Input.GetKeyDown(hitButton4);
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
        }
    }
}