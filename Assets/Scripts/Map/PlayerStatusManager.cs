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
        public GameObject[] playerCrossImage;
        public GameObject crossImageObject;
        public Text playerNameText, gameOverText;
        public int playerID;
        public KeyCode nextTurnKey;
        private int maxLife;
        private bool isAlive, isPlayingAnimation;

        // Start is called before the first frame update
        void Start()
        {
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
            maxLife = playerInfo.GetMaxLife(playerID);
            int numOfCrosses = maxLife - currentLife;
            for (int i = 0; i < numOfCrosses; i++)
            {
                GameObject crossObj = Instantiate(crossImageObject, playerCrossImage[i].transform);
            }
        }

        void CheckIsGameOver(int currentLife)
        {
            if (currentLife < 1)
            {
                GameOver();
            }
            else
            {
                gameOverText.CrossFadeAlpha(0f, 0f, true);
            }
        }

        public void CheckLife(int playerID)
        {
            int currentLife = playerInfo.GetCurrentLife(playerID);
            EnableCross(currentLife);
            CheckIsGameOver(currentLife);
        }

        private void GameOver()
        {
            gameOverText.CrossFadeAlpha(1f, 0f, true);
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
