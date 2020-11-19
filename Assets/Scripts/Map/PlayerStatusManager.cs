using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DHU2020.DGS.MiniGame.Setting;
using DHU2020.DGS.MiniGame.Game;

namespace DHU2020.DGS.MiniGame.Map
{
    public class PlayerStatusManager : MonoBehaviour
    {
        public PlayerInfo playerInfo;
        public GameInfo gameInfo;
        public GameObject[] PlayerCrossesObjects;
        public GameObject GameOverObject;
        public Text playerNameText;
        public int playerID;
        public KeyCode nextTurnKey;
        private int maxLife;
        private bool isAlive, isPlayingAnimation;

        // Start is called before the first frame update
        void Start()
        {
            maxLife = playerInfo.GetMaxLife(playerID);
            for(int i=0; i < PlayerCrossesObjects.Length; i++)
            {
                PlayerCrossesObjects[i].SetActive(false);
            }
            GameOverObject.SetActive(false);
            isAlive = true;
            isPlayingAnimation = false;
        }

        // Update is called once per frame
        void Update()
        {
            // 開発用
            if (isAlive)
            {
                if (Input.GetKeyDown(nextTurnKey))
                {
                    //gameInfo.SetMiniGameWinner("Yubisuma", 1);
                }
            }
        }

        void EnableCross(int currentLife)
        {
            // 一旦全部クロスを消す
            for (int i = 0; i < PlayerCrossesObjects.Length; i++)
            {
                PlayerCrossesObjects[i].SetActive(false);
            }

            int numOfCrosses = maxLife - currentLife;
            for(int i = 0; i < numOfCrosses; i++)
            {
                PlayerCrossesObjects[i].SetActive(true);
            }
        }

        void CheckIsGameOver()
        {
            if(playerInfo.GetCurrentLife(playerID) < 1)
            {
                GameOver();
            }
        }

        public void CheckLife(int playerID)
        {
            int currentLife = playerInfo.GetCurrentLife(playerID);
            EnableCross(currentLife);
            CheckIsGameOver();
        }

        private void GameOver()
        {
            GameOverObject.SetActive(true);
            isAlive = false;
        }

        public bool IsAlive()
        {
            return isAlive;
        }

        public void SetPlayingAnimation(bool isPlaying)
        {
            isPlayingAnimation = isPlaying;
        }
    }
}
