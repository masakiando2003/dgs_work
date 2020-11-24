using DHU2020.DGS.MiniGame.Game;
using DHU2020.DGS.MiniGame.Setting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public KenkenpaPlayerController[] kenkenpaPlayerControllers;
        public GameObject kenkenpaIntroductionCanvas, kenkenpaGameCavnas, playerStepBlockPrefab;
        public Text[] playerNameText, playerLapCountText;
        public Text countDownTimeText, remainingTimeText, resultTitleText, resultText;
        public GameInfo gameInfo;
        public PlayerInfo playerInfo;
        public float startCountDownTime = 3.9f, hideCountDownTime = 1f, remainingTime = 20.9f, gameSetTime = 2f;
        public string startGameText = "GO!";
        public enum GameState
        {
            NotReady,
            WaitForStart,
            GameStart,
            Winner,
            Draw,
            GameSet
        }

        private int[] playerLapCounts, playerPosition, losePlayers;
        private List<int> winnerPlayerIDs = new List<int>();
        private float countDownTimer, remainingTimer;
        private readonly int lapMaxSteps = 8;
        private static GameState currentGameState;
        private List<string> winnerPlayerList = new List<string>();

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
            playerLapCounts = new int[PlaerRouteAreas.Length];
            playerPosition = new int[PlaerRouteAreas.Length];
            for (int playerIndex = 0; playerIndex < PlaerRouteAreas.Length; playerIndex++)
            {
                playerLapCounts[playerIndex] = 0;
                playerPosition[playerIndex] = 0;
                playerNameText[playerIndex].text = playerInfo.GetPlayerName(playerIndex);
                playerLapCountText[playerIndex].text = playerLapCounts[playerIndex].ToString();
                string stepBlockNumberID = "Player"+(playerIndex+1)+"StepBlockNumber1Text";
                GameObject.Find(stepBlockNumberID).GetComponent<Text>().color = Color.blue;
            }
            resultTitleText.enabled = false;
            resultText.enabled = false;
            kenkenpaIntroductionCanvas.SetActive(true);
            kenkenpaGameCavnas.SetActive(false);
            countDownTimer = startCountDownTime;
            remainingTimer = remainingTime;
            remainingTimeText.text = remainingTimer.ToString();

            countDownTimer = startCountDownTime;
            remainingTimer = remainingTime;
            remainingTimeText.text = remainingTime.ToString();

            winnerPlayerIDs.Clear();
            winnerPlayerList.Clear();
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
                    RandomPlayersStepBlock();
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
                    WaitForPlayerButtonPressed();
                    break;
                case GameState.Winner:
                    ShowWinners();
                    break;
                case GameState.Draw:
                    DrawGame();
                    break;
            }
        }

        private void RandomPlayersStepBlock()
        {

        }

        private void WaitForPlayerButtonPressed()
        {
            for(int playerIndex = 0; playerIndex < kenkenpaPlayerControllers.Length; playerIndex++)
            {
                List<KeyCode> playerInputs = kenkenpaPlayerControllers[playerIndex].GetEnteredButtons();

                if(playerInputs.Count == 0) { continue; }

                if (PlayerInputAsSameAsStepBlockButton(playerIndex, playerInputs))
                {
                    PlayerPositionStepFoward(playerIndex);
                }
                else
                {
                    ResetPlayerPosition(playerIndex);
                }
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
                CheckWinner();
            }
        }

        private void CheckWinner()
        {
            int[] playerTotalLaps = new int[PlaerRouteAreas.Length];
            for (int playerIndex = 0; playerIndex < PlaerRouteAreas.Length; playerIndex++)
            {
                playerTotalLaps[playerIndex] = GetPlayerLapCount(playerIndex);
            }
            int maxTotalLap = playerTotalLaps.Max();
            int maxTotalLapPlayerCounts = 0, playerCount=0;
            for (int playerIndex = 0; playerIndex < PlaerRouteAreas.Length; playerIndex++)
            {
                if (playerTotalLaps[playerIndex] == maxTotalLap)
                {
                    maxTotalLapPlayerCounts++;
                    winnerPlayerList.Add(playerInfo.GetPlayerName(playerIndex));
                    winnerPlayerIDs.Add(playerIndex);
                }
                else
                {
                    losePlayers[playerCount++] = playerIndex;
                }
            }
            if(maxTotalLapPlayerCounts > 0 && maxTotalLapPlayerCounts< PlaerRouteAreas.Length)
            {
                Debug.Log("GameState.Winner");
                Instance.ChangeGameState(GameState.Winner);
            }
            else
            {
                Debug.Log("GameState.Draw");
                Instance.ChangeGameState(GameState.Draw);
            }
        }

        private void ShowWinners()
        {
            resultTitleText.enabled = true;
            resultText.enabled = true;
            resultText.text = "";
            for (int i = 0; i < winnerPlayerList.Count; i++)
            {
                resultText.text += winnerPlayerList[i] + "\n";
            }
            resultText.text += "勝ち";
            StartCoroutine(WinGameSet());
        }

        private void DrawGame()
        {
            resultTitleText.enabled = true;
            resultText.enabled = true;
            string drawText = "引き分け...";
            resultText.text = drawText;
            Instance.ChangeGameState(GameState.GameSet);
            StartCoroutine(DrawGameSet());
        }

        IEnumerator WinGameSet()
        {
            yield return new WaitForSeconds(gameSetTime);
            // 一人プレイヤー以上が勝つ出来るので、二番目パラメータのplayerIDが0で設定します
            gameInfo.SetMiniGameWinner("Kenkenpa", 0, losePlayers, winnerPlayerIDs);
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

        private bool PlayerInputAsSameAsStepBlockButton(int playerIndex, List<KeyCode> playerInputs)
        {
            return false;
        }

        // 途中でボタン入力を間違えてしまったら、最初のポジションに再開します
        public void ResetPlayerPosition(int playerIndex)
        {
            playerPosition[playerIndex] = 0;
        }

        public void PlayerPositionStepFoward(int playerIndex)
        {
            playerPosition[playerIndex]++;
            int playerCurrentStep = playerPosition[playerIndex];
            if(playerCurrentStep > lapMaxSteps)
            {
                IncreasePlayerLapCount(playerIndex);
                ResetPlayerPosition(playerIndex);
            }
        }

        public int GetPlayerPosition(int playerIndex)
        {
            return playerPosition[playerIndex];
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