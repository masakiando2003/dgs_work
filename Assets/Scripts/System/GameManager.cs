using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DHU2020.DGS.MiniGame.Map;

namespace DHU2020.DGS.MiniGame.System
{
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// GameManagerのインスタンス
        /// </summary>
        public static GameManager Instance
        {
            get; private set;
        }

        public GameObject[] players;
        public Text[] playerNames;
        public int numOfWinningPlayers = 1;
  
        [Range(1, 99)]
        public int maxTurns;
        private int remainingPlayers, currentTurn;

        public Text currentTurnText, maxTurnsText, turnCanvasTurnText, selectGamePlayerText, winnerText;

        public GameObject turnCanvas, selectGameCanvas, winnerCanvas;
        public CanvasGroup turnCanvasGroup, selectGameCanvasGroup, winnerCanvasGroup;
        public float showCanvasTime = 1f, canvasFadeInSpeed = 3f, canvasFadeOutSpeed = 3f;

        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            currentTurn = 0;
            currentTurnText.text = currentTurn.ToString();
            maxTurnsText.text = maxTurns.ToString();
            turnCanvas.SetActive(false);
            selectGameCanvas.SetActive(false);
            winnerCanvas.SetActive(false);
            ProceedNextTurn();
        }

        // Update is called once per frame
        void Update()
        {

        }

        int GetRemainingPlayers()
        {
            remainingPlayers = 0;// 一旦リセットします
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].GetComponent<PlayerStatusManager>().IsAlive())
                {
                    remainingPlayers++;
                }
            }

            return remainingPlayers;
        }

        void Winner()
        {
            /*
            List<int> winnerPlayerIDs = new List<int>();
            List<string> winnerPlayerNames = new List<string>();
            */
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].GetComponentInChildren<PlayerStatusManager>().IsAlive())
                {
                    /*
                    winnerPlayerIDs.Add(players[i].GetComponentInChildren<PlayerStatusManager>().PlayerID);
                    winnerPlayerNames.Add(players[i].GetComponentInChildren<PlayerStatusManager>().PlayerName);
                    */
                    StartCoroutine(ShowWinner(i));
                    break;
                }
            }
        }

        IEnumerator ShowWinner(int playerIndex)
        {
            winnerCanvas.SetActive(true);
            winnerText.text = playerNames[playerIndex].GetComponent<Text>().text;
            yield return StartCoroutine(CanvasFadeEffect.FadeCanvas(turnCanvasGroup, 1f, 0f, canvasFadeOutSpeed));

        }

        public void ProceedNextTurn()
        {
            selectGameCanvas.SetActive(false);
            if(GetRemainingPlayers() < 2)
            {
                Invoke("Winner", showCanvasTime);
            }
            else
            {
                currentTurn++;
                turnCanvasTurnText.text = currentTurn.ToString();
                for(int i = 0; i < players.Length; i++)
                {
                    players[i].GetComponent<PlayerStatusManager>().SetPlayingAnimation(true);
                }
                //StartCoroutine(ShowTurnCanvas());
                Invoke("ShowTurnCanvas", showCanvasTime);
            }
        }

        bool checkTurnsIsReachedMax()
        {
            return currentTurn >= maxTurns;
        }

        void ShowTurnCanvas()
        {
            turnCanvas.SetActive(true);
            currentTurnText.text = currentTurn.ToString();
            Invoke("HideTurnCanvas", showCanvasTime);
        }

        /*
        IEnumerator ShowTurnCanvas()
        {
            turnCanvas.SetActive(true);
            yield return CanvasFadeEffect.FadeCanvas(turnCanvasGroup, 0f, 1f, canvasFadeInSpeed);
            StartCoroutine(HideTurnCanvas());
        }*/

            /*
        IEnumerator HideTurnCanvas()
        {
            yield return StartCoroutine(CanvasFadeEffect.FadeCanvas(turnCanvasGroup, 1f, 0f, canvasFadeOutSpeed));
            turnCanvas.SetActive(false);
            currentTurnText.text = currentTurn.ToString();
            //FindObjectOfType<PlayerStatusManager>().SetPlayingAnimation(false);
            //StartCoroutine(ShowSelectGameCanvas());
            Invoke("ShowSelectGameCanvas", 2f);
        }*/

        void HideTurnCanvas()
        {
            turnCanvas.SetActive(false);
            Invoke("ShowSelectGameCanvas", showCanvasTime);
        }

        void ShowSelectGameCanvas()
        {
            selectGameCanvas.SetActive(true);
            selectGameCanvas.GetComponent<GameSelector>().RandomizeGames();
            for (int i = 0; i < players.Length; i++)
            {
                players[i].GetComponent<PlayerStatusManager>().SetPlayingAnimation(false);
            }
            if (players[(currentTurn - 1) % players.Length].GetComponent<PlayerStatusManager>().IsAlive())
            {
                selectGamePlayerText.text = playerNames[(currentTurn - 1) % players.Length].GetComponent<Text>().text;
            }
            else
            {
                // 生存しているプレイヤーを探す
                // 次のプレイヤーが優先なので逆順で探す
                for(int selectPlayerID = players.Length - 1; selectPlayerID > 0; selectPlayerID--)
                {
                    if(players[selectPlayerID].GetComponent<PlayerStatusManager>().IsAlive())
                    {
                        selectGamePlayerText.text = playerNames[selectPlayerID].GetComponent<Text>().text;
                        break;
                    }
                }
            }
        }

        /*
        IEnumerator ShowSelectGameCanvas()
        {
            selectGameCanvas.SetActive(true);
            yield return CanvasFadeEffect.FadeCanvas(selectGameCanvasGroup, 0f, 1f, canvasFadeInSpeed);
            selectGamePlayerText.text = playerNames[currentTurn % players.Length].GetComponent<Text>().text;
        }
        */
    }
}