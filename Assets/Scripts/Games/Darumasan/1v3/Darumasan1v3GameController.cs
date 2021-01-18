using DHU2020.DGS.MiniGame.Game;
using DHU2020.DGS.MiniGame.Map;
using DHU2020.DGS.MiniGame.Setting;
using DHU2020.DGS.MiniGame.System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DHU2020.DGS.MiniGame.Map.MapInfo;

namespace DHU2020.DGS.MiniGame.Darumasan
{
    public class Darumasan1v3GameController : MonoBehaviour
    {
        public static Darumasan1v3GameController Instance
        {
            get; private set;
        }

        public MapInfo mapInfo;
        public Localization localeJP, localeEN;
        public int goalDistance = 300, runFactor = 2;
        public int iconStartPosX = -600, iconGoalPosX = 600;
        public GameObject[] playerIcon;
        public GameObject darumansanIntroductionCanvas, darumansanGameCavnas;
        public Darumasan1v3PlayerController[] darumasan1v3PlayerControllers;
        public DarumasanGhostPlayerController darumasanGhostPlayerController;
        public Image player1RightHandImage, player1LeftHandImage, player1StandImage, player1CaughtByGhostImage;
        public Image player2RightHandImage, player2LeftHandImage, player2StandImage, player2CaughtByGhostImage;
        public Image player3RightHandImage, player3LeftHandImage, player3StandImage, player3CaughtByGhostImage;
        public Text[] playerNameText, playerRemainingDistanceText, playerRemainingLabelText;
        public Text countDownTimeText, resultTitleText, resultText, remainingTimeText, remainingTimeLabelText, ghostPlayerNameText, goalLineText;
        public GameInfo gameInfo;
        public PlayerInfo playerInfo;
        public OneVSThreePlayerInfo oneVsThreePlayerInfo;
        public float startCountDownTime = 3.9f, hideCountDownTime = 1f, remainingTime = 120.9f, gameSetTime = 2f, ghostWatchPlayerTime = 3f;
        public string startGameText = "GO!";

        private int winnerPlayerID;
        private float countDownTimer, ghostWatchPlayerTimer;
        private string winnerSide;
        [SerializeField] private int[] playerRemainingDistance, playerInputCount, playerIDs;
        private static GameState currentGameState;
        private Language gameLanguage;

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
            gameLanguage = mapInfo.GetGameLanguage();
            playerRemainingDistance = new int[darumasan1v3PlayerControllers.Length];
            playerInputCount = new int[darumasan1v3PlayerControllers.Length];
            playerIDs = new int[darumasan1v3PlayerControllers.Length];
            int ghostPlayerID = oneVsThreePlayerInfo.GetOnePlayerSidePlayerID();
            darumasanGhostPlayerController.SetOnePlayerSidePlayerID(ghostPlayerID);
            string ghostPlayerName = playerInfo.GetPlayerName(ghostPlayerID);
            ghostPlayerNameText.text = ghostPlayerName;
            for (int playerIndex = 0; playerIndex < darumasan1v3PlayerControllers.Length; playerIndex++)
            {
                if (gameLanguage == Language.Japanese)
                {
                    playerRemainingLabelText[playerIndex].text = localeJP.GetLabelContent("Remaining") + ":";
                }
                else
                {
                    playerRemainingLabelText[playerIndex].text = localeEN.GetLabelContent("Remaining") + ":";
                }
                int playerID = oneVsThreePlayerInfo.GetThreePlayerSidePlayerID(playerIndex);
                playerIDs[playerIndex] = playerID;
                darumasan1v3PlayerControllers[playerIndex].SetThreePlayerSidePlayerID(playerID);
                darumasan1v3PlayerControllers[playerIndex].SetPlayerRunKeyCode(playerID);
                playerNameText[playerIndex].text = playerInfo.GetPlayerName(playerID);
                playerRemainingDistanceText[playerIndex].text = goalDistance.ToString();
                playerInputCount[playerIndex] = 0;
                playerRemainingDistance[playerIndex] = goalDistance;
            }
            resultTitleText.enabled = false;
            resultText.enabled = false;
            if (gameLanguage == Language.Japanese)
            {
                goalLineText.text = "←" + localeJP.GetLabelContent("GoalLine");
                resultTitleText.text = localeJP.GetLabelContent("Result") + ":";
                remainingTimeLabelText.text = localeJP.GetLabelContent("RemainingTime") + ":";
            }
            else
            {
                goalLineText.text = "←" + localeEN.GetLabelContent("GoalLine");
                resultTitleText.text = localeEN.GetLabelContent("Result") + ":";
                remainingTimeLabelText.text = localeEN.GetLabelContent("RemainingTime") + ":";
            }
            darumansanIntroductionCanvas.SetActive(true);
            countDownTimer = startCountDownTime;
            remainingTimeText.text = Mathf.FloorToInt(remainingTime).ToString();
            ghostWatchPlayerTimer = 0;
            winnerPlayerID = 0;
            player1StandImage.enabled = true;
            player2StandImage.enabled = true;
            player3StandImage.enabled = true;
            player1LeftHandImage.enabled = false;
            player2LeftHandImage.enabled = false;
            player3LeftHandImage.enabled = false;
            player1RightHandImage.enabled = false;
            player2RightHandImage.enabled = false;
            player3RightHandImage.enabled = false;
            player1CaughtByGhostImage.enabled = false;
            player2CaughtByGhostImage.enabled = false;
            player3CaughtByGhostImage.enabled = false;
            winnerSide = "";
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
                    darumasanGhostPlayerController.EmptyGhostMessageText();
                    countDownTimeText.enabled = true;
                    countDownTimer -= Time.deltaTime;
                    CountDown(countDownTimer);
                    break;
                case GameState.GameStart:
                    if (countDownTimeText.isActiveAndEnabled)
                    {
                        HideCountDownText();
                    }
                    ShowRemainingTime();
                    darumasanGhostPlayerController.StartToShowGhostMessageText();
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

        private void ShowRemainingTime()
        {
            remainingTime -= Time.deltaTime;
            if(remainingTime >= 0.0f)
            {
                remainingTimeText.text = Mathf.FloorToInt(remainingTime).ToString();
            }
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
            int playerID = Array.IndexOf(playerIDs, playerIndex);
            playerInputCount[playerID]++;

            if (playerInputCount[playerID] % runFactor == 0)
            {
                playerRemainingDistance[playerID]--;
                playerRemainingDistanceText[playerID].text = playerRemainingDistance[playerID].ToString();
                int moveIconFactor = (iconGoalPosX - iconStartPosX) / goalDistance;
                playerIcon[playerID].GetComponent<RectTransform>().offsetMin += new Vector2(moveIconFactor, 0);
                switch (playerID)
                {
                    case 0:
                        if (playerRemainingDistance[playerID] % 2 == 0)
                        {
                            player1RightHandImage.enabled = true;
                            player1LeftHandImage.enabled = false;
                            player1StandImage.enabled = false;
                        }
                        else
                        {
                            player1RightHandImage.enabled = false;
                            player1LeftHandImage.enabled = true;
                            player1StandImage.enabled = false;
                        }
                        break;
                    case 1:
                        if (playerRemainingDistance[playerID] % 2 == 0)
                        {
                            player2RightHandImage.enabled = true;
                            player2LeftHandImage.enabled = false;
                            player2StandImage.enabled = false;
                        }
                        else
                        {
                            player2RightHandImage.enabled = false;
                            player2LeftHandImage.enabled = true;
                            player2StandImage.enabled = false;
                        }
                        break;
                    case 2:
                        if (playerRemainingDistance[playerID] % 2 == 0)
                        {
                            player3RightHandImage.enabled = true;
                            player3LeftHandImage.enabled = false;
                            player3StandImage.enabled = false;
                        }
                        else
                        {
                            player3RightHandImage.enabled = false;
                            player3LeftHandImage.enabled = true;
                            player3StandImage.enabled = false;
                        }
                        break;
                }
                playerIcon[playerID].GetComponent<RectTransform>().offsetMax -= new Vector2(-moveIconFactor, 0);
            }
        }

        public void PlayerStand(int playerIndex)
        {
            switch (playerIndex)
            {
                case 0:
                    player1StandImage.enabled = true;
                    player1LeftHandImage.enabled = false;
                    player1RightHandImage.enabled = false;
                    player1CaughtByGhostImage.enabled = false;
                    break;
                case 1:
                    player2StandImage.enabled = true;
                    player2LeftHandImage.enabled = false;
                    player2RightHandImage.enabled = false;
                    player2CaughtByGhostImage.enabled = false;
                    break;
                case 2:
                    player3StandImage.enabled = true;
                    player3LeftHandImage.enabled = false;
                    player3RightHandImage.enabled = false;
                    player3CaughtByGhostImage.enabled = false;
                    break;
            }
        }

        public void ShowGhostMessageEnd()
        {
            Instance.ChangeGameState(GameState.GhostMessageEnded);
        }

        private void ContinueTheGame()
        {
            ghostWatchPlayerTimer = 0f;
            darumasanGhostPlayerController.EmptyGhostMessageText();
            // 全てのプレイヤーが立つ画像を表示する
            PlayerStand(0);
            PlayerStand(1);
            PlayerStand(2);
            Instance.ChangeGameState(GameState.GameStart);
        }

        private void DeterminePlayerIsRunning()
        {
            ghostWatchPlayerTimer += Time.deltaTime;
            if (ghostWatchPlayerTimer <= ghostWatchPlayerTime)
            {
                for (int playerIndex = 0; playerIndex < playerInfo.GetPlayersCount()-1; playerIndex++)
                {
                    if (darumasan1v3PlayerControllers[playerIndex].GetPlayerIsInRunningState() == true)
                    {
                        ResetPlayerPosition(playerIndex);
                    }
                }
            }
            else
            {
                for (int playerIndex = 0; playerIndex < darumasan1v3PlayerControllers.Length; playerIndex++)
                {
                    darumasan1v3PlayerControllers[playerIndex].ResetPlayerToNonRunningState();
                }
                ContinueTheGame();
            }
        }

        private void ResetPlayerPosition(int playerIndex)
        {
            playerRemainingDistance[playerIndex] = goalDistance;
            playerRemainingDistanceText[playerIndex].text = playerRemainingDistance[playerIndex].ToString();
            switch (playerIndex)
            {
                case 0:
                    player1CaughtByGhostImage.enabled = true;
                    player1StandImage.enabled = false;
                    player1LeftHandImage.enabled = false;
                    player1RightHandImage.enabled = false;
                    break;
                case 1:
                    player2CaughtByGhostImage.enabled = true;
                    player2StandImage.enabled = false;
                    player2LeftHandImage.enabled = false;
                    player2RightHandImage.enabled = false;
                    break;
                case 2:
                    player3CaughtByGhostImage.enabled = true;
                    player3StandImage.enabled = false;
                    player3LeftHandImage.enabled = false;
                    player3RightHandImage.enabled = false;
                    break;
            }
            playerIcon[playerIndex].GetComponent<RectTransform>().anchoredPosition = new Vector2(iconStartPosX, 0);
        }

        private void DecideWinner()
        {
            if (remainingTime <= 0.0f)
            {
                winnerSide = "ghost";
                Instance.ChangeGameState(GameState.Winner);
            }
            else
            {
                for (int playerIndex = 0; playerIndex < playerInfo.GetPlayersCount() - 1; playerIndex++)
                {
                    if (playerRemainingDistance[playerIndex] <= 0)
                    {
                        winnerSide = "players";
                        Instance.ChangeGameState(GameState.Winner);
                    }
                }
            }
        }

        private void ShowWinner()
        {
            resultTitleText.enabled = true;
            resultText.enabled = true;
            string winningText = "勝ち!";
            if(winnerSide == "ghost")
            {
                winnerPlayerID = oneVsThreePlayerInfo.GetOnePlayerSidePlayerID();
                resultText.text = playerInfo.GetPlayerName(winnerPlayerID) + winningText;
            }
            else
            {
                string winnerPlayerNameList = "";
                for (int i = 0; i < playerIDs.Length; i++)
                {
                    winnerPlayerNameList += playerInfo.GetPlayerName(playerIDs[i]) + Environment.NewLine;
                }
                resultText.text = winnerPlayerNameList + winningText;
            }
            StartCoroutine(WinGameSet());
        }

        IEnumerator WinGameSet()
        {
            yield return new WaitForSeconds(gameSetTime);
            if (winnerSide == "ghost")
            {
                gameInfo.SetMiniGameWinner("Darumasan", winnerPlayerID);
            }
            else
            {
                List<int> winnerPlayerIDs = new List<int>();
                for (int i = 0; i < playerIDs.Length; i++)
                {
                    winnerPlayerIDs.Add(playerIDs[i]);
                }
                int[] loserPlayers = new int[1];
                loserPlayers[0] = oneVsThreePlayerInfo.GetOnePlayerSidePlayerID();
                gameInfo.SetMiniGameWinner("Darumasan", 0, loserPlayers, winnerPlayerIDs);
            }
        }
    }
}