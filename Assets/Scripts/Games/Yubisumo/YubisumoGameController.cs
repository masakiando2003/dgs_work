using DHU2020.DGS.MiniGame.Game;
using DHU2020.DGS.MiniGame.Setting;
using DHU2020.DGS.MiniGame.System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DHU2020.DGS.MiniGame.Yubisumo
{
    public class YubisumoGameController : MonoBehaviour
    {
        public static YubisumoGameController Instance
        {
            get; private set;
        }

        public GameObject yubisumoIntroductionCanvas, yubisumoGameCanvas;
        public YubisumoPlayerController player1Controller, player2Controller;
        public GameInfo gameInfo;
        public PVPPlayerInfo pvpPlayerInfo;
        public PlayerInfo playerInfo;
        public Text player1NameText, player2NameText, countDownTimeText, remainingTimeText, player1HitCountText, player2HitCountText;
        public float startCountDownTime = 3.9f, hideCountDownTime = 1f, remainingTime = 20.9f, gameSetTime = 2f;
        public string startGameText = "GO!";

        private int player1ID, player2ID, player1HitCount, player2HitCount;
        private float countDownTimer, remainingTimer;
        public enum GameState
        {
            NotReady,
            WaitForStart,
            GameStart,
            Player1WIN,
            Player2WIN,
            Draw
        }
        private static GameState currentGameState;

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
            Instance.ChangeGameState(GameState.NotReady);
            yubisumoIntroductionCanvas.SetActive(true);
            yubisumoGameCanvas.SetActive(false);
            player1ID = pvpPlayerInfo.GetPlayer1ID();
            player2ID = pvpPlayerInfo.GetPlayer2ID();
            player1HitCount = player2HitCount = 0;

            player1NameText.text = playerInfo.GetPlayerName(player1ID);
            player2NameText.text = playerInfo.GetPlayerName(player2ID);

            countDownTimer = startCountDownTime;
            remainingTimer = remainingTime;
            remainingTimeText.text = remainingTime.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            CheckGameState();
        }

        private void CountDown(float countDownTimer)
        {
            Debug.Log("countDownTimer: "+ countDownTimer);
            ShowCountTimeText(countDownTimer);
            if(countDownTimer <= 0.0f)
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

        private void CheckGameState()
        {
            switch (currentGameState)
            {
                case GameState.NotReady:
                    break;
                case GameState.WaitForStart:
                    countDownTimeText.enabled = true;
                    countDownTimer -= Time.deltaTime;
                    CountDown(countDownTimer);
                    break;
                case GameState.GameStart:
                    HideCountDownText();
                    player1HitCount = player1Controller.GetHitCount();
                    player2HitCount = player2Controller.GetHitCount();
                    ShowHitCounts(player1HitCount, player2HitCount);
                    CompareHitCount(player1HitCount, player2HitCount);
                    remainingTimer -= Time.deltaTime;
                    ShowRemainingTimer(remainingTimer);
                    CheckRemainingTime(remainingTimer);
                    break;
                case GameState.Player1WIN:
                    ShowWinner(player1ID);
                    break;
                case GameState.Player2WIN:
                    ShowWinner(player2ID);
                    break;
                case GameState.Draw:
                    DrawGame();
                    break;
            }
        }

        private void DrawGame()
        {

        }

        private void ShowWinner(int playerID)
        {
            StartCoroutine(SetWinner(playerID));
        }

        IEnumerator SetWinner(int playerID)
        {
            yield return new WaitForSeconds(gameSetTime);
            int winnerPlayerID = playerID;
            int[] loserPlayerID = new int[1];
            loserPlayerID[0] = (playerID == player1ID) ? player1ID : player2ID;
            gameInfo.SetMiniGameWinner("Yubisumo", playerID, loserPlayerID);
        }

        IEnumerator DrawGameSet()
        {
            yield return new WaitForSeconds(gameSetTime);
            gameInfo.SetMiniGameWinner("Yubisumo", -1);
        }

        private void CheckRemainingTime(float remainingTimer)
        {
            if(remainingTimer <= 0.0f)
            {
                Instance.ChangeGameState(GameState.Draw);
            }
        }

        private void ShowRemainingTimer(float remainingTimer)
        {
            remainingTimeText.text = Mathf.FloorToInt(remainingTimer).ToString();
        }

        private void ShowHitCounts(int player1HitCount, int player2HitCount)
        {
            player1HitCountText.text = player1HitCount.ToString();
            player2HitCountText.text = player2HitCount.ToString();
        }

        private void CompareHitCount(int player1HitCount, int player2HitCount)
        {

        }

        public GameState GetCurrentGameState()
        {
            return currentGameState;
        }
    }
}