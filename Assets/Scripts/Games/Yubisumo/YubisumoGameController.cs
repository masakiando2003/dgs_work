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
        public Slider playerStatusSlider;
        public Text[] player1NameText, player2NameText;
        public Text countDownTimeText, remainingTimeText, player1HitCountText, player2HitCountText, resultTitleText, resultText;
        public Image drawImage, player1AdvantageImage, player2AdvantageImage;
        public float startCountDownTime = 3.9f, hideCountDownTime = 1f, remainingTime = 20.9f, gameSetTime = 2f;
        public string startGameText = "GO!";
        public int advantageHitCount, winnerHitCount;

        private int player1ID, player2ID, player1HitCount, player2HitCount, winnerID;
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
            Initialization();
        }

        private void Initialization()
        {
            Instance.ChangeGameState(GameState.NotReady);
            yubisumoIntroductionCanvas.SetActive(true);
            yubisumoGameCanvas.SetActive(false);
            player1ID = pvpPlayerInfo.GetPlayer1ID();
            player2ID = pvpPlayerInfo.GetPlayer2ID();
            player1HitCount = player2HitCount = 0;

            for (int i = 0; i < player1NameText.Length; i++)
            {
                player1NameText[i].text = playerInfo.GetPlayerName(player1ID);
            }
            for (int i = 0; i < player2NameText.Length; i++)
            {
                player2NameText[i].text = playerInfo.GetPlayerName(player2ID);
            }

            countDownTimer = startCountDownTime;
            remainingTimer = remainingTime;
            remainingTimeText.text = remainingTime.ToString();

            drawImage.enabled = true;
            player1AdvantageImage.enabled = false;
            player2AdvantageImage.enabled = false;

            playerStatusSlider.minValue = winnerHitCount * -1;
            playerStatusSlider.maxValue = winnerHitCount;
            playerStatusSlider.value = 0;

            resultTitleText.enabled = false;
            resultText.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            CheckGameState();
        }

        private void CountDown(float countDownTimer)
        {
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
                    drawImage.enabled = true;
                    player1HitCount = player1Controller.GetHitCount();
                    player2HitCount = player2Controller.GetHitCount();
                    ShowHitCounts(player1HitCount, player2HitCount);
                    ComparePlayerAdvatange(player1HitCount, player2HitCount);
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
            resultTitleText.enabled = true;
            resultText.enabled = true;
            string drawText = "引き分け...";
            resultText.text = drawText;
            StartCoroutine(DrawGameSet());
        }

        private void ShowWinner(int playerID)
        {
            resultTitleText.enabled = true;
            resultText.enabled = true;
            string winningText = "勝ち!";
            resultText.text = (playerID == player1ID) ? player1NameText[0].text + winningText : player2NameText[0].text + winningText;
            StartCoroutine(SetWinner(playerID));
        }

        IEnumerator SetWinner(int playerID)
        {
            yield return new WaitForSeconds(gameSetTime);
            int winnerPlayerID = playerID;
            int[] loserPlayerID = new int[1];
            loserPlayerID[0] = (winnerPlayerID == player1ID) ? player2ID : player1ID;
            gameInfo.SetMiniGameWinner("Yubisumo", playerID, loserPlayerID);
        }

        IEnumerator DrawGameSet()
        {
            yield return new WaitForSeconds(gameSetTime);
            gameInfo.SetMiniGameWinner("Yubisumo", -1);
        }

        private void CheckRemainingTime(float remainingTimer)
        {
            if(remainingTimer < 1f)
            {
                int winnerPlayerID = GetWinnerPlayer();
                if(winnerPlayerID == -1)
                {
                    Instance.ChangeGameState(GameState.Draw);
                }
                else if(winnerPlayerID == player1ID)
                {
                    Instance.ChangeGameState(GameState.Player1WIN);
                }
                else
                {
                    Instance.ChangeGameState(GameState.Player2WIN);
                }
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

        private void ComparePlayerAdvatange(int player1HitCount, int player2HitCount)
        {
            int count_difference = CaculateHitCount(player1HitCount, player2HitCount);

            if (count_difference >= winnerHitCount)
            {
                if (player1HitCount > player2HitCount)
                {
                    Instance.ChangeGameState(GameState.Player1WIN);
                }
                else
                {
                    Instance.ChangeGameState(GameState.Player2WIN);
                }
            }
            else if (count_difference >= advantageHitCount)
            {
                if (player1HitCount > player2HitCount)
                {
                    drawImage.enabled = false;
                    player1AdvantageImage.enabled = true;
                    player2AdvantageImage.enabled = false;
                }
                else
                {
                    drawImage.enabled = false;
                    player1AdvantageImage.enabled = false;
                    player2AdvantageImage.enabled = true;
                }
            }
            else
            {
                drawImage.enabled = true;
                player1AdvantageImage.enabled = false;
                player2AdvantageImage.enabled = false;
            }
            UpdatePlayerStatusSlider(player1HitCount, player2HitCount);
        }

        private static int CaculateHitCount(int player1HitCount, int player2HitCount)
        {
            return Mathf.Abs(player1HitCount - player2HitCount);
        }

        public void UpdatePlayerStatusSlider(int player1HitCount, int player2HitCount)
        {
            int count_difference = CaculateHitCount(player1HitCount, player2HitCount);
            int playerStatusSliderValue = (player1HitCount > player2HitCount) ? count_difference : count_difference * -1;
            playerStatusSlider.value = playerStatusSliderValue;
        }

        public int GetWinnerPlayer()
        {
            int count_difference = Mathf.Abs(player1HitCount - player2HitCount);
            if (count_difference >= advantageHitCount)
            {
                return (player1HitCount > player2HitCount) ? player1ID : player2ID;
            }
            return -1;
        }

        public GameState GetCurrentGameState()
        {
            return currentGameState;
        }
    }
}