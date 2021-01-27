using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DHU2020.DGS.MiniGame.Map;
using UnityEngine.SceneManagement;
using DHU2020.DGS.MiniGame.Setting;
using static DHU2020.DGS.MiniGame.Map.MapInfo;

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
        
        public PlayerInfo playerInfo;
        public MapInfo mapInfo;
        public Localization localeJP, localeEN;
        public GameObject[] players;
        public Text[] playerNames, turnLabels, playerStatusLabels, playerLifeTexts;
        public Text winnerTitleText, congradulationText, drawTitleText, drawContentText, retryText, remainingPlayersText;
        public int numOfWinningPlayers = 1;
  
        public Text currentTurnText, maxTurnsText, turnCanvasTurnText, selectGamePlayerText, winnerText;

        public GameObject turnCanvas, selectGameCanvas, selectGameTypeCanvas, selectGhostPlayerCanvas, winnerCanvas, drawGameCanvas, PVPSelectPlayerCanvas;
        public CanvasGroup turnCanvasGroup, selectGameCanvasGroup, winnerCanvasGroup;
        public float showCanvasTime = 1f, canvasFadeInSpeed = 3f, canvasFadeOutSpeed = 3f, changeCanvasTime = 3f;

        private int remainingPlayers, currentTurn, maxTurns, selectedPlayerID;
        private List<int> PVPPlayerIDs = new List<int>();
        private Language gameLanguage;

        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            //mapInfo.StartGame();
            currentTurn = mapInfo.GetCurrentTurn();
            currentTurnText.text = currentTurn.ToString();
            maxTurns = mapInfo.GetMaxTurns();
            maxTurnsText.text = maxTurns.ToString();
            turnCanvas.SetActive(false);
            selectGameCanvas.SetActive(false);
            selectGameTypeCanvas.SetActive(false);
            selectGhostPlayerCanvas.SetActive(false);
            winnerCanvas.SetActive(false);
            PVPSelectPlayerCanvas.SetActive(false);
            //playerInfo.SetDefaultLifes();
            for (int i=0; i < players.Length; i++)
            {
                if (playerInfo.GetPlayerName(i).Equals(""))
                {
                    playerInfo.RandomizePlayerName(i);
                }
                playerNames[i].GetComponent<Text>().text = playerInfo.GetPlayerName(i);
                players[i].GetComponent<PlayerStatusManager>().CheckLife(i);
                playerLifeTexts[i].text = playerInfo.GetCurrentLife(i).ToString();
            }
            CheckWinningStatus();

            gameLanguage = mapInfo.GetGameLanguage();
            if(gameLanguage == Language.Japanese)
            {
                for(int i = 0; i < turnLabels.Length; i++)
                {
                    turnLabels[i].text = localeJP.GetLabelContent("Turn");
                }
                for(int i = 0; i < playerStatusLabels.Length; i++)
                {
                    playerStatusLabels[i].text = localeJP.GetLabelContent("Status")+":";
                }
                winnerTitleText.text = localeJP.GetLabelContent("Winner") + ":";
                congradulationText.text = localeJP.GetLabelContent("Congradulation") + "!";
                drawTitleText.text = localeJP.GetLabelContent("Result") + ":";
                drawContentText.text = localeJP.GetLabelContent("Draw") + "...";
                retryText.text = localeJP.GetLabelContent("Retry") + "...";
            }
            else
            {
                for (int i = 0; i < turnLabels.Length; i++)
                {
                    turnLabels[i].text = localeEN.GetLabelContent("Turn");
                }
                for (int i = 0; i < playerStatusLabels.Length; i++)
                {
                    playerStatusLabels[i].text = localeEN.GetLabelContent("Status")+":";
                }
                winnerTitleText.text = localeEN.GetLabelContent("Winner") + ":";
                congradulationText.text = localeEN.GetLabelContent("Congradulation") + "!";
                drawTitleText.text = localeEN.GetLabelContent("Result") + ":";
                drawContentText.text = localeEN.GetLabelContent("Draw") + "...";
                retryText.text = localeEN.GetLabelContent("Retry") + "...";
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ActiviatCanvas(string CanvasName)
        {
            int selectedGameIndex;
            switch (CanvasName)
            {
                case "PVPSelectPlayerCanvas":
                    PVPSelectPlayerCanvas.SetActive(true);
                    PVPSelectPlayerCanvas.GetComponent<PVPSelector>().SetPVPSelectorPlayerIndex(selectedPlayerID);
                    PVPSelectPlayerCanvas.GetComponent<PVPSelector>().InitializeRivalPlayerList();
                    for (int i = 0; i < players.Length; i++)
                    {
                        players[i].GetComponent<PlayerStatusManager>().SetPlayingAnimation(false);
                    }
                    break;
                case "SelectGameTypeCanvas":
                    selectGameTypeCanvas.SetActive(true);
                    selectGameTypeCanvas.GetComponent<SelectGameTypeRule>().SetPlayerName(selectedPlayerID);
                    selectGameCanvas.GetComponent<GameSelector>().SetChooseGamePlayerIndex(selectedPlayerID);
                    selectedGameIndex = selectGameCanvas.GetComponent<GameSelector>().GetSelectedGameIndex();

                    selectGameTypeCanvas.GetComponent<SelectGameTypeRule>().SetChooseGameTypeRulePlayerIndex(selectedPlayerID);
                    selectGameTypeCanvas.GetComponent<SelectGameTypeRule>().SetSelectedGameIndex(selectedGameIndex);
                    selectGameTypeCanvas.GetComponent<SelectGameTypeRule>().SetGameNameText(selectedGameIndex);
                    for (int i = 0; i < players.Length; i++)
                    {
                        players[i].GetComponent<PlayerStatusManager>().SetPlayingAnimation(false);
                    }
                    break;
                case "SelectGhostPlayerCanvas":
                    selectGhostPlayerCanvas.SetActive(true);
                    selectGameCanvas.GetComponent<GameSelector>().SetChooseGamePlayerIndex(selectedPlayerID);
                    selectedGameIndex = selectGameCanvas.GetComponent<GameSelector>().GetSelectedGameIndex();

                    selectGhostPlayerCanvas.GetComponent<SelectGhostPlayer>().SetSelectGhostPlayerPlayerIndex(selectedPlayerID);
                    selectGhostPlayerCanvas.GetComponent<SelectGhostPlayer>().SetSelectedGameIndex(selectedGameIndex);
                    selectGhostPlayerCanvas.GetComponent<SelectGhostPlayer>().SetGhostPlayerFromPlayer(selectedPlayerID);
                    for (int i = 0; i < players.Length; i++)
                    {
                        players[i].GetComponent<PlayerStatusManager>().SetPlayingAnimation(false);
                    }
                    break;
                default:
                    break;
            }
        }

        int GetRemainingPlayers()
        {
            remainingPlayers = 0;// 一旦リセットします
            for (int i = 0; i < players.Length; i++)
            {
                if (playerInfo.GetCurrentLife(i) > 0)
                {
                    remainingPlayers++;
                }
            }
            remainingPlayersText.text = remainingPlayers.ToString();

            return remainingPlayers;
        }

        public int GetSelectedPlayerID()
        {
            return selectedPlayerID;
        }

        void Winner()
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].GetComponentInChildren<PlayerStatusManager>().IsAlive())
                {
                    StartCoroutine(ShowWinner(i));
                    break;
                }
            }
        }

        IEnumerator ShowWinner(int winnerPlayerIndex)
        {
            winnerCanvas.SetActive(true);
            winnerText.text = playerNames[winnerPlayerIndex].GetComponent<Text>().text;
            yield return StartCoroutine(CanvasFadeEffect.FadeCanvas(turnCanvasGroup, 1f, 0f, canvasFadeOutSpeed));

        }

        IEnumerator ShowWinners(List<int> winnerPlayerIDs)
        {
            string winnerPlayerIDs_str = "";
            for(int i = 0; i < winnerPlayerIDs.Count; i++)
            {
                winnerPlayerIDs_str += playerInfo.GetPlayerName(winnerPlayerIDs[i]);
                if(i <= winnerPlayerIDs.Count - 1)
                {
                    winnerPlayerIDs_str += Environment.NewLine;
                }
            }
            winnerCanvas.SetActive(true);
            winnerText.text = winnerPlayerIDs_str;
            yield return new WaitForSeconds(changeCanvasTime);
            SceneManager.LoadScene("GameTitle");
        }

        IEnumerator DrawGame()
        {
            drawGameCanvas.SetActive(true);
            yield return new WaitForSeconds(changeCanvasTime);
            SceneManager.LoadScene("GameTitle");
        }

        void CheckWinners()
        {
            List<int> winnerPlayerIDs = new List<int>();
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].GetComponentInChildren<PlayerStatusManager>().IsAlive())
                {
                    winnerPlayerIDs.Add(i);
                }
            }
            if(winnerPlayerIDs.Count == players.Length)
            {
                StartCoroutine(DrawGame());
            }
            else
            {
                StartCoroutine(ShowWinners(winnerPlayerIDs));
            }
        }

        public void CheckWinningStatus()
        {
            PVPSelectPlayerCanvas.SetActive(false);
            selectGameCanvas.SetActive(false);
            selectGameTypeCanvas.SetActive(false);
            if (GetRemainingPlayers() < 2)
            {
                Invoke("Winner", showCanvasTime);
            }
            else if(currentTurn >= maxTurns)
            {
                // 残っている全員勝利する
                // 全員まだ生きているなら引き分けになります
                Invoke("CheckWinners", showCanvasTime);
            }
            else
            {
                currentTurn++;
                turnCanvasTurnText.text = currentTurn.ToString();
                for(int i = 0; i < players.Length; i++)
                {
                    players[i].GetComponent<PlayerStatusManager>().SetPlayingAnimation(true);
                }
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

        void HideTurnCanvas()
        {
            turnCanvas.SetActive(false);
            Invoke("ShowSelectGameCanvas", showCanvasTime);
        }

        void ShowSelectGameCanvas()
        {
            selectGameCanvas.SetActive(true);
            selectGameCanvas.GetComponent<GameSelector>().RandomizeGames();
            if (players[(currentTurn - 1) % players.Length].GetComponent<PlayerStatusManager>().IsAlive())
            {
                selectedPlayerID = (currentTurn - 1) % players.Length;
                selectGamePlayerText.text = playerNames[selectedPlayerID].GetComponent<Text>().text;
            }
            else
            {
                // 生存しているプレイヤーを探す
                // 次のプレイヤーが優先なので逆順で探す
                for(int selectPlayerID = players.Length - 1; selectPlayerID > 0; selectPlayerID--)
                {
                    if(players[selectPlayerID].GetComponent<PlayerStatusManager>().IsAlive())
                    {
                        selectedPlayerID = selectPlayerID;
                        selectGamePlayerText.text = playerNames[selectPlayerID].GetComponent<Text>().text;
                        break;
                    }
                }
            }
        }

        public void EnterGame()
        {
            float loadGameTime = FindObjectOfType<GameSelector>().GetLoadGameTime();
            string selectedGame = FindObjectOfType<GameSelector>().GetGameTitle();
            StartCoroutine(LoadGame(selectedGame, loadGameTime));
        }

        IEnumerator LoadGame(string selectedGame, float loadGameTime)
        {
            yield return new WaitForSeconds(loadGameTime);
            SceneManager.LoadScene(selectedGame);
        }
    }
}