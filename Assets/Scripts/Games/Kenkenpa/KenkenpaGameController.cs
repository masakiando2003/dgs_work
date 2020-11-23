using DHU2020.DGS.MiniGame.Game;
using DHU2020.DGS.MiniGame.Setting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DHU2020.DGS.MiniGame.Kenkenpa
{
    public class KenkenpaGameController : MonoBehaviour
    {
        public static KenkenpaGameController Instance
        {
            get; private set;
        }

        public GameObject[] PlaerRouteAreas;
        public GameObject kenkenpaIntroductionCanvas, kenkenpaGameCavnas, playerStepBlockPrefab;
        public Text[] playerNameText, playerLapCountText;
        public Text countDownTimeText, remainingTimeText, resultTitleText, resultText;
        public GameInfo gameInfo;
        public PlayerInfo playerInfo;
        public float startCountDownTime = 3.9f, hideCountDownTime = 1f, remainingTime = 20.9f, gameSetTime = 2f;
        public string startGameText = "GO!";

        private int[] playerLapCounts, playerPosition;
        private List<int> winnerIDs = new List<int>();
        private float countDownTimer, remainingTimer;
        public enum GameState
        {
            NotReady,
            WaitForStart,
            GameStart,
            Winner,
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
            kenkenpaIntroductionCanvas.SetActive(true);
            kenkenpaGameCavnas.SetActive(false);
            playerLapCounts = new int[PlaerRouteAreas.Length];
            playerPosition = new int[PlaerRouteAreas.Length];
            for (int playerIndex = 0; playerIndex < PlaerRouteAreas.Length; playerIndex++)
            {
                playerLapCounts[playerIndex] = 0;
                playerPosition[playerIndex] = 0;
                playerNameText[playerIndex].text = playerInfo.GetPlayerName(playerIndex);
                playerLapCountText[playerIndex].text = playerLapCounts[playerIndex].ToString();
            }
            countDownTimer = startCountDownTime;
            remainingTimer = remainingTime;
            remainingTimeText.text = remainingTimer.ToString();

            countDownTimer = startCountDownTime;
            remainingTimer = remainingTime;
            remainingTimeText.text = remainingTime.ToString();

            winnerIDs.Clear();
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

        // Update is called once per frame
        void Update()
        {
            CheckGameState();
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
                    remainingTimer -= Time.deltaTime;
                    ShowRemainingTimer(remainingTimer);
                    CheckRemainingTime(remainingTimer);
                    break;
                case GameState.Winner:
                    break;
                case GameState.Draw:
                    DrawGame();
                    break;
            }
        }

        private void ShowRemainingTimer(float remainingTimer)
        {
            remainingTimeText.text = Mathf.FloorToInt(remainingTimer).ToString();
        }

        private void CheckRemainingTime(float remainingTimer)
        {
            if (remainingTimer < 1f)
            {

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


        IEnumerator DrawGameSet()
        {
            yield return new WaitForSeconds(gameSetTime);
            gameInfo.SetMiniGameWinner("Kenkenpa", -1);
        }
        
        public GameState GetCurrentGameState()
        {
            return currentGameState;
        }

        // 途中でボタン入力を間違えてしまったら、最初のポジションに再開します
        public void ResetPlayerPosition(int playerIndex)
        {
            playerPosition[playerIndex] = 0;
        }

        public void PlayerPositionStepFoward(int playerIndex)
        {
            playerPosition[playerIndex]++;
        }

        public void IncreasePlayerLapCount(int playerIndex)
        {
            playerLapCounts[playerIndex]++;
        }

        public int GetPlayerLapCount(int index)
        {
            return playerLapCounts[index];
        }
    }
}