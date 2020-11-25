using DHU2020.DGS.MiniGame.Game;
using DHU2020.DGS.MiniGame.Setting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
            Prepare,
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
            losePlayers = new int[PlaerRouteAreas.Length];
            for (int playerIndex = 0; playerIndex < PlaerRouteAreas.Length; playerIndex++)
            {
                playerLapCounts[playerIndex] = 0;
                playerPosition[playerIndex] = 0;
                losePlayers[playerIndex] = 0;
                playerNameText[playerIndex].text = playerInfo.GetPlayerName(playerIndex);
                playerLapCountText[playerIndex].text = playerLapCounts[playerIndex].ToString();
                string stepBlockNumberID = "Player"+(playerIndex+1)+"StepBlockNumber1Text";
                GameObject.Find(stepBlockNumberID).GetComponent<Text>().color = Color.blue;
            }
            resultTitleText.enabled = false;
            resultText.enabled = false;
            kenkenpaIntroductionCanvas.SetActive(true);
            //kenkenpaGameCavnas.SetActive(false);
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
                case GameState.Prepare:
                    RandomPlayersStepBlock();
                    break;
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
            for(int playerIndex = 0; playerIndex < PlaerRouteAreas.Length; playerIndex++)
            {
                RandomPlayerStepBlock(playerIndex);
            }
            Instance.ChangeGameState(GameState.NotReady);
        }

        private void RandomPlayerStepBlock(int playerIndex)
        {
            List<KeyCode> playerButtons = kenkenpaPlayerControllers[playerIndex].GetPlayerButtons();
            for (int step = 1; step <= lapMaxSteps; step++)
            {
                int randomBlockPattern = Random.Range(0, 2); // シングルブロックまたはダブルブロック
                int randomButtonNum = Random.Range(0, playerButtons.Count);
                int randomButtonNum2 = Random.Range(0, playerButtons.Count);
                while(randomButtonNum2 == randomButtonNum)
                {
                    randomButtonNum2 = Random.Range(0, playerButtons.Count);
                }

                GameObject singleBlockArea = GameObject.Find("Player" + (playerIndex + 1) + "StepSingleBlockArea" + step);
                GameObject doubleBlockArea = GameObject.Find("Player" + (playerIndex + 1) + "StepDoubleBlockArea" + step);

                // 一旦全てのブロックエリアを活性化する
                singleBlockArea.SetActive(true);
                doubleBlockArea.SetActive(true);

                KenkenpaRandomStepBlock randomSingleBlock = GameObject.Find("Player" + (playerIndex + 1) + "StepSingleBlockArea" + step + "Block").
                    GetComponent<KenkenpaRandomStepBlock>();
                randomSingleBlock.SetStepBlockKeyCode(playerButtons[randomButtonNum]);
                KenkenpaRandomStepBlock randomDoubleBlock1 = GameObject.Find("Player" + (playerIndex + 1) + "StepDoubleBlockArea" + step + "Block1").
                            GetComponent<KenkenpaRandomStepBlock>();
                randomDoubleBlock1.SetStepBlockKeyCode(playerButtons[randomButtonNum]);
                KenkenpaRandomStepBlock randomDoubleBlock2 = GameObject.Find("Player" + (playerIndex + 1) + "StepDoubleBlockArea" + step + "Block2").
                    GetComponent<KenkenpaRandomStepBlock>();
                randomDoubleBlock2.SetStepBlockKeyCode(playerButtons[randomButtonNum2]);

                switch (randomBlockPattern)
                {
                    case 0:
                        if (doubleBlockArea != null)
                        {
                            doubleBlockArea.SetActive(false);
                        }
                        break;
                    case 1:
                        if (singleBlockArea != null)
                        {
                            singleBlockArea.SetActive(false);
                        }
                        break;
                }
            }
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
            if (playerCurrentStep > lapMaxSteps)
            {
                IncreasePlayerLapCount(playerIndex);
                RandomPlayerStepBlock(playerIndex);
                ResetPlayerPosition(playerIndex);
            }
            string stepBlockNumberID = "Player" + playerIndex + "StepBlockNumber"+(playerPosition[playerIndex]+1) +"Text";
            GameObject.Find(stepBlockNumberID).GetComponent<Text>().color = Color.blue;
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