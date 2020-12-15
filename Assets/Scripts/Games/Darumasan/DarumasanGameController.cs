using DHU2020.DGS.MiniGame.Game;
using DHU2020.DGS.MiniGame.Setting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DHU2020.DGS.MiniGame.Darumasan
{
    public class DarumasanGameController : MonoBehaviour
    {
        public static DarumasanGameController Instance
        {
            get; private set;
        }

        public int goalDistance = 300, runFactor = 2;
        public int iconStartPosX = -600, iconGoalPosX = 600;
        public GameObject[] playerIcon;
        public GameObject darumansanIntroductionCanvas, darumansanGameCavnas;
        public DarumasanPlayerController[] darumasanPlayerControllers;
        public DarumasanGhostTextWriter darumasanGhostTextWriter;
        public Text[] playerNameText, playerIconNameText, playerRemainingDistanceText;
        public Text countDownTimeText, resultTitleText, resultText;
        public GameInfo gameInfo;
        public PlayerInfo playerInfo;
        public float startCountDownTime = 3.9f, hideCountDownTime = 1f, remainingTime = 20.9f, gameSetTime = 2f, ghostWatchPlayerTime = 3f;
        public string startGameText = "GO!";

        private int winnerPlayerID;
        private float countDownTimer, ghostWatchPlayerTimer;
        private int[] playerRemainingDistance, playerInputCount;
        private static GameState currentGameState;

        public enum GameState
        {
            Prepare,
            NotReady,
            WaitForStart,
            GameStart,
            GhostMessageEnded,  // 鬼が"だるまさんがころんだ"を言った直後
            Winner,
            GameSet
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            Initialization();
        }

        private void Initialization()
        {
            playerRemainingDistance = new int[playerInfo.GetPlayersCount()];
            playerInputCount = new int[playerInfo.GetPlayersCount()];
            for (int playerIndex = 0; playerIndex < playerInfo.GetPlayersCount(); playerIndex++)
            {
                playerNameText[playerIndex].text = playerInfo.GetPlayerName(playerIndex);
                playerIconNameText[playerIndex].text = playerInfo.GetPlayerName(playerIndex);
                playerRemainingDistanceText[playerIndex].text = goalDistance.ToString();
                playerInputCount[playerIndex] = 0;
                playerRemainingDistance[playerIndex] = goalDistance;
            }
            resultTitleText.enabled = false;
            resultText.enabled = false;
            darumansanIntroductionCanvas.SetActive(true);
            countDownTimer = startCountDownTime;
            ghostWatchPlayerTimer = 0;
            winnerPlayerID = 0;
        }

        // Update is called once per frame
        void Update()
        {
            CheckGameState();
        }

        private void CheckGameState()
        {
            switch (currentGameState)
            {
                case GameState.Prepare:
                    break;
                case GameState.NotReady:
                    break;
                case GameState.WaitForStart:
                    countDownTimeText.enabled = true;
                    countDownTimer -= Time.deltaTime;
                    CountDown(countDownTimer);
                    break;
                case GameState.GameStart:
                    if (countDownTimeText.isActiveAndEnabled)
                    {
                        HideCountDownText();
                    }
                    darumasanGhostTextWriter.ShowGhostMessageText(); // 画面上に"だるまさんがころんだ"メッセージを表示する
                    DecideWinner();
                    break;
                case GameState.GhostMessageEnded:
                    DeterminePlayerIsRunning(); // 鬼が"だるまさんがころんだ"と言った直後、プレイヤーがまだ走るかどうか判定する
                    break;
                case GameState.Winner:
                    ShowWinner();
                    break;
            }
        }

        private void CountDown(float countDownTimer)
        {
            ShowCountTimeText(countDownTimer);
            if (countDownTimer <= 0.0f)
            {
                Invoke("HideCountDownText", hideCountDownTime);
                Instance.ChangeGameState(GameState.GameStart);
            }
        }

        private void ShowCountTimeText(float countDownTimer)
        {
            int remainingSeconds = Mathf.FloorToInt(countDownTimer);
            countDownTimeText.text = (remainingSeconds > 0) ? remainingSeconds.ToString() : startGameText;
        }

        private void HideCountDownText()
        {
            countDownTimeText.enabled = false;
        }

        public void GameStart()
        {
            Instance.ChangeGameState(GameState.WaitForStart);
        }

        private void ChangeGameState(GameState gameState)
        {
            currentGameState = gameState;
        }

        public GameState GetCurrentGameState()
        {
            return currentGameState;
        }

        public void HandlePlayersInputRun(int playerIndex)
        {
            playerInputCount[playerIndex]++;
            if(playerInputCount[playerIndex] % runFactor == 0)
            {
                playerRemainingDistance[playerIndex]--;
                playerRemainingDistanceText[playerIndex].text = playerRemainingDistance[playerIndex].ToString();
                int moveIconFactor = (iconGoalPosX - iconStartPosX) / goalDistance;
                playerIcon[playerIndex].GetComponent<RectTransform>().offsetMin += new Vector2(moveIconFactor, 0);
                playerIcon[playerIndex].GetComponent<RectTransform>().offsetMax -= new Vector2(-moveIconFactor, 0);
            }
        }

        public void ShowGhostMessageEnd()
        {
            Instance.ChangeGameState(GameState.GhostMessageEnded);
        }

        private void ContinueTheGame()
        {
            ghostWatchPlayerTimer = 0f;
            darumasanGhostTextWriter.EmptyGhostMessageText();
            Instance.ChangeGameState(GameState.GameStart);
        }

        private void DeterminePlayerIsRunning()
        {
            ghostWatchPlayerTimer += Time.deltaTime;
            if(ghostWatchPlayerTimer <= ghostWatchPlayerTime)
            {
                for (int playerIndex = 0; playerIndex < playerInfo.GetPlayersCount(); playerIndex++)
                {
                    if (darumasanPlayerControllers[playerIndex].GetPlayerIsInRunningState() == true)
                    {
                        ResetPlayerPosition(playerIndex);
                    }
                }
            }
            else
            {
                for (int playerIndex = 0; playerIndex < playerInfo.GetPlayersCount(); playerIndex++)
                {
                    darumasanPlayerControllers[playerIndex].ResetPlayerToNonRunningState();
                }
                ContinueTheGame();
            }
        }

        private void ResetPlayerPosition(int playerIndex)
        {
            playerRemainingDistance[playerIndex] = goalDistance;
            playerRemainingDistanceText[playerIndex].text = playerRemainingDistance[playerIndex].ToString();
            playerIcon[playerIndex].GetComponent<RectTransform>().anchoredPosition = new Vector2(iconStartPosX, 0);
        }

        private void DecideWinner()
        {
            for (int playerIndex = 0; playerIndex < playerInfo.GetPlayersCount(); playerIndex++)
            {
                if(playerRemainingDistance[playerIndex] <= 0)
                {
                    winnerPlayerID = playerIndex;
                    Instance.ChangeGameState(GameState.Winner);
                }
            }
        }

        private void ShowWinner()
        {
            resultTitleText.enabled = true;
            resultText.enabled = true;
            string winningText = "勝ち!";
            resultText.text = playerInfo.GetPlayerName(winnerPlayerID) + winningText;
            StartCoroutine(WinGameSet());
        }

        IEnumerator WinGameSet()
        {
            yield return new WaitForSeconds(gameSetTime);
            gameInfo.SetMiniGameWinner("Darumasan", winnerPlayerID);
        }
    }
}