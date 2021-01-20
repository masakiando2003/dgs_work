using DHU2020.DGS.MiniGame.Game;
using DHU2020.DGS.MiniGame.Map;
using DHU2020.DGS.MiniGame.Setting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static DHU2020.DGS.MiniGame.Map.MapInfo;
using Random = UnityEngine.Random;

namespace DHU2020.DGS.MiniGame.Kenkenpa
{
    public class KenkenpaGameController : MonoBehaviour
    {
        public static KenkenpaGameController Instance
        {
            get; private set;
        }

        public MapInfo mapInfo;
        public Localization localeJP, localeEN;
        public GameObject[] PlaerRouteAreas, PlayerStepBlockAreas;
        public KenkenpaPlayerController[] kenkenpaPlayerControllers;
        public GameObject kenkenpaIntroductionCanvas, kenkenpaGameCavnas, playerStepBlockPrefab;
        public Text[] playerNameText, playerLapCountText, playerLapLabelText;
        public Text countDownTimeText, remainingTimeText, resultTitleText, resultText, timerLabelText;
        public GameInfo gameInfo;
        public PlayerInfo playerInfo;
        public float startCountDownTime = 3.9f, hideCountDownTime = 1f, remainingTime = 20.9f, gameSetTime = 2f;
        public float showResultSignTime = 1f, showRightBlockTime = 0.8f, showRightChosenBlockTime = 0.8f;
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
        private Language gameLanguage;

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
            gameLanguage = mapInfo.GetGameLanguage();
            playerLapCounts = new int[PlaerRouteAreas.Length];
            playerPosition = new int[PlaerRouteAreas.Length];
            losePlayers = new int[PlaerRouteAreas.Length];
            for (int playerIndex = 0; playerIndex < PlaerRouteAreas.Length; playerIndex++)
            {
                playerLapCounts[playerIndex] = 0;
                playerPosition[playerIndex] = 0;
                losePlayers[playerIndex] = 0;
                playerNameText[playerIndex].text = playerInfo.GetPlayerName(playerIndex);
                if (gameLanguage == Language.Japanese)
                {
                    playerLapLabelText[playerIndex].text = localeJP.GetLabelContent("Laps") + ":";
                }
                else
                {
                    playerLapLabelText[playerIndex].text = localeEN.GetLabelContent("Laps") + ":";
                }
                playerLapCountText[playerIndex].text = playerLapCounts[playerIndex].ToString();
                string stepBlockNumberID = "Player"+(playerIndex+1)+"StepBlockNumber1Text";
                GameObject.Find(stepBlockNumberID).GetComponent<Text>().color = Color.red;
                for (int j = 1; j <= lapMaxSteps; j++)
                {
                    GameObject.Find("Player" + (playerIndex + 1) + "StepBlockArea" + j + "Right").GetComponent<Image>().enabled = false;
                    GameObject.Find("Player" + (playerIndex + 1) + "StepBlockArea" + j + "Wrong").GetComponent<Image>().enabled = false;
                }
            }
            if (gameLanguage == Language.Japanese)
            {
                timerLabelText.text = localeJP.GetLabelContent("RemainingTime") + ":";
                resultTitleText.text = localeJP.GetLabelContent("Result") + ":";
            }
            else
            {
                timerLabelText.text = localeEN.GetLabelContent("RemainingTime") + ":";
                resultTitleText.text = localeEN.GetLabelContent("Result") + ":";
            }
            resultTitleText.enabled = false;
            resultText.enabled = false;
            kenkenpaIntroductionCanvas.SetActive(true);
            countDownTimer = startCountDownTime;
            remainingTimer = remainingTime;
            remainingTimeText.text = remainingTimer.ToString();

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
                //int randomBlockPattern = 0;
                int randomButtonNum = Random.Range(0, playerButtons.Count);
                int randomButtonNum2 = Random.Range(0, playerButtons.Count);
                while(randomButtonNum2 == randomButtonNum)
                {
                    randomButtonNum2 = Random.Range(0, playerButtons.Count);
                }
                
                GameObject singleBlockArea = GameObject.Find("Player" + (playerIndex + 1) + "StepSingleBlockArea" + step);
                foreach(Image img in singleBlockArea.GetComponentsInChildren<Image>())
                {
                    img.enabled = true;
                }
                foreach (Text txt in singleBlockArea.GetComponentsInChildren<Text>())
                {
                    txt.enabled = true;
                }
                GameObject doubleBlockArea = GameObject.Find("Player" + (playerIndex + 1) + "StepDoubleBlockArea" + step);
                foreach (Image img in doubleBlockArea.GetComponentsInChildren<Image>())
                {
                    img.enabled = true;
                }
                foreach (Text txt in doubleBlockArea.GetComponentsInChildren<Text>())
                {
                    txt.enabled = true;
                }

                // 一旦全てのブロックエリアを活性化する
                switch (randomBlockPattern)
                {
                    case 0:
                        KenkenpaRandomStepBlock randomSingleBlock = GameObject.Find("Player" + (playerIndex + 1) + "StepSingleBlockArea" + step + "Block").
                            GetComponent<KenkenpaRandomStepBlock>();
                        randomSingleBlock.SetStepBlockKeyCode(playerButtons[randomButtonNum]);
                        foreach (Image img in doubleBlockArea.GetComponentsInChildren<Image>())
                        {
                            img.enabled = false;
                        }
                        foreach (Text txt in doubleBlockArea.GetComponentsInChildren<Text>())
                        {
                            txt.enabled = false;
                        }
                        foreach (KenkenpaRandomStepBlock k in doubleBlockArea.GetComponentsInChildren<KenkenpaRandomStepBlock>())
                        {
                            k.SetStepBlockKeyCode(KeyCode.None);
                        }
                        break;
                    case 1:
                        KenkenpaRandomStepBlock randomDoubleBlock1 = GameObject.Find("Player" + (playerIndex + 1) + "StepDoubleBlockArea" + step + "Block1").
                                    GetComponent<KenkenpaRandomStepBlock>();
                        randomDoubleBlock1.SetStepBlockKeyCode(playerButtons[randomButtonNum]);
                        KenkenpaRandomStepBlock randomDoubleBlock2 = GameObject.Find("Player" + (playerIndex + 1) + "StepDoubleBlockArea" + step + "Block2").
                            GetComponent<KenkenpaRandomStepBlock>();
                        randomDoubleBlock2.SetStepBlockKeyCode(playerButtons[randomButtonNum2]);
                        foreach (Image img in singleBlockArea.GetComponentsInChildren<Image>())
                        {
                            img.enabled = false;
                        }
                        foreach (Text txt in singleBlockArea.GetComponentsInChildren<Text>())
                        {
                            txt.enabled = false;
                        }
                        foreach (KenkenpaRandomStepBlock k in singleBlockArea.GetComponentsInChildren<KenkenpaRandomStepBlock>())
                        {
                            k.SetStepBlockKeyCode(KeyCode.None);
                        }
                        break;
                }
            }
        }

        public void PlayerButtonPressed(int playerIndex)
        {
            List<KeyCode> playerInputs = kenkenpaPlayerControllers[playerIndex].GetEnteredButtons();

            if (playerInputs.Count == 0) {
                kenkenpaPlayerControllers[playerIndex].SetButtonNotPressed();
            }
            else
            {
                int currentPlayerPosition = GetPlayerPosition(playerIndex);
                int playerID = playerIndex + 1;
                int blockCount = 0;
                GameObject currentStepBlock = GameObject.Find("Player" + playerID + "StepBlockArea" + currentPlayerPosition);
                List<KeyCode> stepBlocksKeyCodes = new List<KeyCode>();
                foreach (KenkenpaRandomStepBlock kpsb in currentStepBlock.GetComponentsInChildren<KenkenpaRandomStepBlock>())
                {
                    if(kpsb.GetStepBlockKeyCode() != KeyCode.None)
                    {
                        stepBlocksKeyCodes.Add(kpsb.GetStepBlockKeyCode());
                        blockCount++;
                    }
                }

                if (playerInputs.Count == stepBlocksKeyCodes.Count)
                {
                    if (PlayerInputAsSameAsStepBlockButton(playerIndex, currentPlayerPosition-1, playerInputs, stepBlocksKeyCodes, blockCount))
                    {
                        PlayerPositionStepFoward(playerIndex);
                    }
                    else
                    {
                        ResetPlayerPosition(playerIndex);
                    }
                }
                kenkenpaPlayerControllers[playerIndex].SetButtonNotPressed();
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
                Instance.ChangeGameState(GameState.Winner);
            }
            else
            {
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
            if(gameLanguage == Language.Japanese)
            {
                resultText.text += localeJP.GetLabelContent("Win") + "!!!";
            }
            else
            {
                resultText.text += localeEN.GetLabelContent("Win") + "!!!";
            }
            StartCoroutine(WinGameSet());
        }

        private void DrawGame()
        {
            resultTitleText.enabled = true;
            resultText.enabled = true;
            string drawText;
            if(gameLanguage == Language.Japanese)
            {
                drawText = localeJP.GetLabelContent("Draw")+"...";
            }
            else
            {
                drawText = localeEN.GetLabelContent("Draw") + "...";
            }
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

        private bool PlayerInputAsSameAsStepBlockButton(int playerIndex, int currentPlayerPosition, List<KeyCode> playerInputs, List<KeyCode> stepBlocksKeyCodes, int blockCount)
        {
            bool flag = false;
            int rightInputCount = 0;
            foreach(KeyCode k in stepBlocksKeyCodes)
            {
                if (!playerInputs.Contains(k))
                {
                    //break;
                }
                else
                {
                    int playerID = playerIndex + 1;
                    int step = currentPlayerPosition + 1;
                    string areaID = "";
                    if(blockCount == 1)
                    {
                        areaID = "Player"+ playerID + "StepSingleBlockArea"+ step;
                    }
                    else
                    {
                        areaID = "Player" + playerID + "StepDoubleBlockArea" + step;
                    }
                    foreach (KenkenpaRandomStepBlock kpsb in GameObject.Find(areaID).GetComponentsInChildren<KenkenpaRandomStepBlock>())
                    {
                        if (playerInputs.Contains(kpsb.GetStepBlockKeyCode()))
                        {
                            StartCoroutine(ShowRightChosenBlock(kpsb.gameObject.name));
                        }
                    }
                    rightInputCount++;
                }
            }
            if(rightInputCount == stepBlocksKeyCodes.Count)
            {
                flag = true;
            }
            return flag;
        }

        // 途中でボタン入力を間違えてしまったら、最初のポジションに再開します
        public void ResetPlayerPosition(int playerIndex, int increaseLapFlag = 0)
        {
            int playerID = playerIndex + 1;
            int currentPosition = playerPosition[playerIndex];
            int blockID = currentPosition;
            if(blockID + 1 > lapMaxSteps)
            {
                blockID = lapMaxSteps - 1;
            }
            string currentStepBlockID = "Player" + playerID + "StepBlockNumber"+ (blockID+1) + "Text";
            GameObject.Find(currentStepBlockID).GetComponent<Text>().color = Color.black;
            string wrongBlockName = "Player"+ playerID+"StepBlockArea"+ (blockID + 1) + "Wrong";
            if(increaseLapFlag == 0)
            {
                StartCoroutine(ShowResultSign(wrongBlockName));
            }
            playerPosition[playerIndex] = 0;
            string firstStepBlockID = "Player" + playerID + "StepBlockNumber1Text";
            GameObject.Find(firstStepBlockID).GetComponent<Text>().color = Color.red;
        }

        public void PlayerPositionStepFoward(int playerIndex)
        {
            playerPosition[playerIndex]++;
            int playerCurrentStep = playerPosition[playerIndex];
            int playerID = playerIndex + 1;
            int currentBlockPosition = playerPosition[playerIndex] > lapMaxSteps ? lapMaxSteps : playerPosition[playerIndex];
            int nextBlockPosition = (currentBlockPosition + 1) > lapMaxSteps ? 1 : (currentBlockPosition + 1);
            string currentStepBlockID = "Player" + playerID + "StepBlockNumber"+ currentBlockPosition + "Text";
            GameObject.Find(currentStepBlockID).GetComponent<Text>().color = Color.black;
            string rightBlockName = "Player" + playerID + "StepBlockArea" + currentBlockPosition + "Right";
            StartCoroutine(ShowResultSign(rightBlockName));
            string nextStepBlockID = "Player" + playerID + "StepBlockNumber" + nextBlockPosition + "Text";
            GameObject.Find(nextStepBlockID).GetComponent<Text>().color = Color.red;
            if (playerCurrentStep >= lapMaxSteps)
            {
                IncreasePlayerLapCount(playerIndex);
                RandomPlayerStepBlock(playerIndex);
                ResetPlayerPosition(playerIndex, 1);
            }
        }

        private IEnumerator ShowRightChosenBlock(string blockName)
        {
            GameObject.Find(blockName).GetComponentsInChildren<Image>()[1].color = Color.blue;
            yield return new WaitForSeconds(showRightChosenBlockTime);
            GameObject.Find(blockName).GetComponentsInChildren<Image>()[1].color = Color.white;
        }

        private IEnumerator ShowRightBlock(string rightBlockName)
        {
            GameObject.Find(rightBlockName).GetComponent<Image>().enabled = true;
            yield return new WaitForSeconds(showRightBlockTime);
            GameObject.Find(rightBlockName).GetComponent<Image>().enabled = false;
        }

        private IEnumerator ShowResultSign(string ResultBlockName)
        {
            GameObject.Find(ResultBlockName).GetComponent<Image>().enabled = true;
            yield return new WaitForSeconds(showResultSignTime);
            GameObject.Find(ResultBlockName).GetComponent<Image>().enabled = false;
        }

        public int GetPlayerPosition(int playerIndex)
        {
            return (playerPosition[playerIndex]+1) > lapMaxSteps ? lapMaxSteps : playerPosition[playerIndex] + 1;
        }

        public void IncreasePlayerLapCount(int playerIndex)
        {
            playerLapCounts[playerIndex]++;
            int playerID = playerIndex + 1;
            GameObject.Find("Player" + playerID + "LapCountText").GetComponent<Text>().text = playerLapCounts[playerIndex].ToString();
        }

        public int GetPlayerLapCount(int index)
        {
            return playerLapCounts[index];
        }
    }
}