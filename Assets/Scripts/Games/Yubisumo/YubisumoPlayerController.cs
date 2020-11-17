using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DHU2020.DGS.MiniGame.Yubisumo.YubisumoGameController;

namespace DHU2020.DGS.MiniGame.Yubisumo
{
    public class YubisumoPlayerController : MonoBehaviour
    {
        public YubisumoGameController yubisumoGameController;
        public KeyCode hitKeyCode;

        private int hitCount;

        // Start is called before the first frame update
        void Start()
        {
            hitCount = 0;
        }

        // Update is called once per frame
        void Update()
        {
            GameState currentGameState = yubisumoGameController.GetCurrentGameState();

            if(currentGameState != GameState.GameStart) { return; }

            if (Input.GetKeyDown(hitKeyCode))
            {
                IncreaseHitCount();
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
    }
}