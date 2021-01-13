using DHU2020.DGS.MiniGame.Setting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DHU2020.DGS.MiniGame.Darumasan.Darumasan1v3GameController;

namespace DHU2020.DGS.MiniGame.Darumasan
{
    public class DarumasanGhostPlayerController : MonoBehaviour
    {
        public Darumasan1v3GameController darumasanGameController;
        public PlayerInfo playerInfo;
        public Text ghostMessageText;
        public Image ghostFaceToRightImage, ghostFaceToLeftImage;
        public int playerID;
        public string ghostMessageToShow;
        public KeyCode p1HitButton, p2HitButton, p3HitButton, p4HitButton;

        private int characterIndex;
        private string ghostMessage;
        private bool showMessageFlag;

        // Start is called before the first frame update
        void Start()
        {
            ghostFaceToRightImage.enabled = true;
            ghostFaceToLeftImage.enabled = false;
            showMessageFlag = true;
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

            ghostMessageText.text = ghostMessageToShow.Substring(0, characterIndex);

            GetPlayerHitButtonDown(playerID);
        }

        private void GetPlayerHitButtonDown(int playerID)
        {
            if(!showMessageFlag) { return; }
            
            switch (playerID)
            {
                case 0:
                    if(playerInfo.GetPlayerControllerInput(playerID) == PlayerInfo.PlayerControllerInput.Keyboard)
                    {
                        if (Input.GetKeyDown(p1HitButton))
                        {
                            HandleGhostPlayerHitButtonDown();
                        }
                    }
                    else
                    {
                        if (Input.GetButtonDown("DarumasanGhostP"+(playerID+1)+"HitButton"))
                        {
                            HandleGhostPlayerHitButtonDown();
                        }
                    }
                    break;
                case 1:
                    if (playerInfo.GetPlayerControllerInput(playerID) == PlayerInfo.PlayerControllerInput.Keyboard)
                    {
                        if (Input.GetKeyDown(p2HitButton))
                        {
                            HandleGhostPlayerHitButtonDown();
                        }
                    }
                    else
                    {
                        if (Input.GetButtonDown("DarumasanGhostP" + (playerID + 1) + "HitButton"))
                        {
                            HandleGhostPlayerHitButtonDown();
                        }
                    }
                    break;
                case 2:
                    if (playerInfo.GetPlayerControllerInput(playerID) == PlayerInfo.PlayerControllerInput.Keyboard)
                    {
                        if (Input.GetKeyDown(p3HitButton))
                        {
                            HandleGhostPlayerHitButtonDown();
                        }
                    }
                    else
                    {
                        if (Input.GetButtonDown("DarumasanGhostP" + (playerID + 1) + "HitButton"))
                        {
                            HandleGhostPlayerHitButtonDown();
                        }
                    }
                    break;
                case 3:
                    if (playerInfo.GetPlayerControllerInput(playerID) == PlayerInfo.PlayerControllerInput.Keyboard)
                    {
                        if (Input.GetKeyDown(p4HitButton))
                        {
                            HandleGhostPlayerHitButtonDown();
                        }
                    }
                    else
                    {
                        if (Input.GetButtonDown("DarumasanGhostP" + (playerID + 1) + "HitButton"))
                        {
                            HandleGhostPlayerHitButtonDown();
                        }
                    }
                    break;
            }
        }

        private void HandleGhostPlayerHitButtonDown()
        {
            characterIndex++;
            if (characterIndex >= ghostMessage.Length)
            {
                ghostFaceToRightImage.enabled = false;
                ghostFaceToLeftImage.enabled = true;
                showMessageFlag = false;
                darumasanGameController.ShowGhostMessageEnd();
            }
        }

        public void SetOnePlayerSidePlayerID(int playerIndex)
        {
            playerID = playerIndex;
        }

        public void StartToShowGhostMessageText()
        {
            ghostFaceToRightImage.enabled = true;
            ghostFaceToLeftImage.enabled = false;
            ghostMessage = ghostMessageToShow;
            showMessageFlag = true;
        }

        public void EmptyGhostMessageText()
        {
            ghostMessageText.text = "";
            characterIndex = 0;
        }
    }
}