using DHU2020.DGS.MiniGame.Game;
using DHU2020.DGS.MiniGame.Map;
using DHU2020.DGS.MiniGame.Setting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static DHU2020.DGS.MiniGame.Map.MapInfo;
using Random = UnityEngine.Random;

namespace DHU2020.DGS.MiniGame.System
{
    public class SelectGhostPlayer : MonoBehaviour
    {
        public GameInfo gameInfo;
        public MapInfo mapInfo;
        public PlayerInfo playerInfo;
        public OneVSThreePlayerInfo oneVSThreePlayerInfo;
        public GameObject[] playerObject;
        public Color selectColor;
        public Image randomPlayerBorder;
        public Text[] playerNamesText;
        public Text playerNameTextJP, playerNameTextEN, selectGhostTextJP, selectGhostTextEN, randomPlayerText, randomedPlayerText;
        
        private int playerIndex, numOfPlayers, selectGhostPlayerFromPlayer, selectedGhostPlayerIndex, originalSelectedGhostPlayerIndex, selectedGameIndex;
        private bool selectedGhostPlayer, selectRandomGhostPlayerFlag;
        private Language gameLanguage;

        private void Start()
        {
            Initialization();
        }

        private void Initialization()
        {
            playerIndex = 0;
            gameLanguage = mapInfo.GetGameLanguage();
            if (gameLanguage == Language.Japanese)
            {
                playerNameTextEN.enabled = false;
                selectGhostTextEN.enabled = false;
                randomPlayerText.text = "ランダム";
            }
            else
            {
                playerNameTextJP.enabled = false;
                selectGhostTextJP.enabled = false;
                randomPlayerText.text = "Random";
            }
            selectedGhostPlayerIndex = 0;
            selectGhostPlayerFromPlayer = 0;
            selectedGhostPlayer = false;
            selectRandomGhostPlayerFlag = false;
            numOfPlayers = playerInfo.GetPlayersCount();
            for(int i = 0; i < numOfPlayers; i++)
            {
                string playerObjectName = playerObject[i].name;
                if(i == 0)
                {
                    GameObject.Find(playerObjectName + "Border").GetComponent<Image>().color = selectColor;
                }
                else
                {
                    GameObject.Find(playerObjectName + "Border").GetComponent<Image>().color = Color.black;
                }
                string playerName = playerInfo.GetPlayerName(i);
                playerNamesText[i].text = playerName;
            }
        }

        private void Update()
        {
            if (selectedGhostPlayer) { return; }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || (Input.GetAxis("P" + (playerIndex + 1) + "Horizontal") == -1))
            {
                if (!selectRandomGhostPlayerFlag)
                {
                    originalSelectedGhostPlayerIndex = selectedGhostPlayerIndex;
                    selectedGhostPlayerIndex = ((selectedGhostPlayerIndex - 1) < 0) ? numOfPlayers - 1 : selectedGhostPlayerIndex - 1;
                    SelectGhost();
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || (Input.GetAxis("P" + (playerIndex + 1) + "Horizontal") == 1))
            {
                if (!selectRandomGhostPlayerFlag)
                {
                    originalSelectedGhostPlayerIndex = selectedGhostPlayerIndex;
                    selectedGhostPlayerIndex = ((selectedGhostPlayerIndex + 1) > numOfPlayers - 1) ? 0 : selectedGhostPlayerIndex + 1;
                    SelectGhost();
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || (Input.GetAxis("P" + (playerIndex + 1) + "Vertical") != 0))
            {
                if(selectRandomGhostPlayerFlag == true)
                {
                    selectRandomGhostPlayerFlag = false;
                }
                else
                {
                    selectRandomGhostPlayerFlag = true;
                }

                if (selectRandomGhostPlayerFlag == true)
                {
                    SelectRandomGhostPlayer();
                }
                else
                {
                    SelectGhost();
                }
            }
            else if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || 
                    Input.GetButtonDown("P" + (playerIndex + 1) + "DecideButton"))
            {
                if (selectRandomGhostPlayerFlag)
                {
                    int randomGhostPlayerIndex = Random.Range(0, playerInfo.GetPlayersCount());
                    selectedGhostPlayerIndex = randomGhostPlayerIndex;
                    randomedPlayerText.text = playerInfo.GetPlayerName(selectedGhostPlayerIndex);
                }
                int threePlayerSideID = 0;
                for(int i = 0; i < playerInfo.GetPlayersCount(); i++)
                {
                    if(i == selectedGhostPlayerIndex)
                    {
                        oneVSThreePlayerInfo.SetOnePlayerSidePlayerID(i);
                    }
                    else
                    {
                        oneVSThreePlayerInfo.SetThreePlayerSidePlayerIDs(threePlayerSideID, i);
                        threePlayerSideID++;
                    }
                }
                string sceneName = gameInfo.GetGameTitleEnglish(selectedGameIndex)+"1v3";
                float loadGameTime = FindObjectOfType<GameSelector>().GetLoadGameTime();
                StartCoroutine(LoadGame(sceneName, loadGameTime));
            }
        }

        private void SelectGhost()
        {
            string selectedPlayerObjectName = "";
            string originalSelectedPlayerObjectName = "";
            switch (selectedGhostPlayerIndex)
            {
                case 0:
                    selectedPlayerObjectName = "FirstPlayer";
                    break;
                case 1:
                    selectedPlayerObjectName = "SecondPlayer";
                    break;
                case 2:
                    selectedPlayerObjectName = "ThirdPlayer";
                    break;
                case 3:
                    selectedPlayerObjectName = "FourthPlayer";
                    break;
            }
            switch (originalSelectedGhostPlayerIndex)
            {
                case 0:
                    originalSelectedPlayerObjectName = "FirstPlayer";
                    break;
                case 1:
                    originalSelectedPlayerObjectName = "SecondPlayer";
                    break;
                case 2:
                    originalSelectedPlayerObjectName = "ThirdPlayer";
                    break;
                case 3:
                    originalSelectedPlayerObjectName = "FourthPlayer";
                    break;
            }
            randomPlayerBorder.color = Color.black;
            GameObject.Find(originalSelectedPlayerObjectName+"Border").GetComponent<Image>().color = Color.black;
            GameObject.Find(selectedPlayerObjectName + "Border").GetComponent<Image>().color = selectColor;
        }

        private void SelectRandomGhostPlayer()
        {
            string selectedPlayerObjectName = "";
            switch (selectedGhostPlayerIndex)
            {
                case 0:
                    selectedPlayerObjectName = "FirstPlayer";
                    break;
                case 1:
                    selectedPlayerObjectName = "SecondPlayer";
                    break;
                case 2:
                    selectedPlayerObjectName = "ThirdPlayer";
                    break;
                case 3:
                    selectedPlayerObjectName = "FourthPlayer";
                    break;
            }
            randomPlayerBorder.color = selectColor;
            GameObject.Find(selectedPlayerObjectName + "Border").GetComponent<Image>().color = Color.black;
        }

        public void SetGhostPlayerFromPlayer(int playerIndex)
        {
            selectGhostPlayerFromPlayer = playerIndex;
            playerNameTextJP.text = playerInfo.GetPlayerName(selectGhostPlayerFromPlayer);
            playerNameTextEN.text = playerInfo.GetPlayerName(selectGhostPlayerFromPlayer);
        }

        public void SetSelectedGameIndex(int gameIndex)
        {
            selectedGameIndex = gameIndex;
        }

        IEnumerator LoadGame(string selectedGame, float loadGameTime)
        {
            yield return new WaitForSeconds(loadGameTime);
            SceneManager.LoadScene(selectedGame);
        }

        public void SetSelectGhostPlayerPlayerIndex(int player)
        {
            playerIndex = player;
        }
    }

}