using DHU2020.DGS.MiniGame.Setting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DHU2020.DGS.MiniGame.Setting.PlayerInfo;
using static DHU2020.DGS.MiniGame.Yubisumo.YubisumoGameController;

namespace DHU2020.DGS.MiniGame.Yubisumo
{
    public class YubisumoPlayerController : MonoBehaviour
    {
        public PlayerInfo playerInfo;
        public YubisumoGameController yubisumoGameController;

        private int hitCount, playerID;
        private PlayerControllerInput playerInputMethod;
        private KeyCode hitKeyCode;

        // Start is called before the first frame update
        void Start()
        {
            hitCount = 0;
        }

        // Update is called once per frame
        void Update()
        {
            GameState currentGameState = yubisumoGameController.GetCurrentGameState();

            if ((currentGameState != GameState.GameStart)) { return; }

            if (playerInputMethod == PlayerControllerInput.Keyboard)
            {
                if (Input.GetKeyDown(hitKeyCode))
                {
                    IncreaseHitCount();
                }
            }
            else if (playerInputMethod == PlayerControllerInput.Joystick)
            {
                if (Input.GetButtonDown("YubisumoP" + (playerID + 1) + "HitButton"))
                {
                    IncreaseHitCount();
                }
            }
        }

        private void IncreaseHitCount()
        {
            hitCount++;
        }

        public int GetHitCount()
        {
            return hitCount;
        }

        public void SetPlayerID(int playerIndex)
        {
            playerID = playerIndex;
        }

        public void InitializeInputMethod(int playerIndex)
        {
            playerInputMethod = playerInfo.GetPlayerControllerInput(playerIndex);
        }

        public void SetKeyboardInputKeyCode(KeyCode keyboardKeyCode)
        {
            hitKeyCode = keyboardKeyCode;
        }
    }
}