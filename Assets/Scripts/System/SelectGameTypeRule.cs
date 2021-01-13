using DHU2020.DGS.MiniGame.Game;
using DHU2020.DGS.MiniGame.Map;
using DHU2020.DGS.MiniGame.System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static DHU2020.DGS.MiniGame.Map.MapInfo;
using Random = UnityEngine.Random;

namespace DHU2020.DGS.MiniGame.Setting
{
    public class SelectGameTypeRule : MonoBehaviour
    {
        public PlayerInfo playerInfo;
        public GameInfo gameInfo;
        public Color selectColor;
        public MapInfo mapInfo;
        public Text battleRoyaleText, randomText;
        public Text playerNameTextJP, gameNameTextJP, descriptionTextJP, randomGameTypeTextJP;
        public Text playerNameTextEN, gameNameTextEN, descriptionTextEN, randomGameTypeTextEN;

        private int selectedGameIndex, selectRuleTypeID;
        private string displayPlayerName, gameName, sceneName;
        private float loadGameTime;
        private bool selectedGameTypeFlag;
        private Language gameLanguage;

        private void Start()
        {
            Initialization();
        }

        private void Initialization()
        {
            selectRuleTypeID = 0;
            loadGameTime = FindObjectOfType<GameSelector>().GetLoadGameTime();
            selectedGameTypeFlag = false;
            playerNameTextJP.text = "";
            gameNameTextJP.text = "";
            randomGameTypeTextJP.text = "";
            playerNameTextEN.text = "";
            gameNameTextEN.text = "";
            randomGameTypeTextEN.text = "";
            gameLanguage = mapInfo.GetGameLanguage();
            if(gameLanguage == Language.Japanese)
            {
                playerNameTextEN.enabled = false;
                gameNameTextEN.enabled = false;
                descriptionTextEN.enabled = false;
                randomGameTypeTextEN.enabled = false;
                battleRoyaleText.text = "バトルロイヤル";
                randomText.text = "ランダム";
            }
            else
            {
                playerNameTextJP.enabled = false;
                gameNameTextJP.enabled = false;
                descriptionTextJP.enabled = false;
                randomGameTypeTextJP.enabled = false;
                battleRoyaleText.text = "Battle Royale";
                randomText.text = "Random";
            }
        }

        private void Update()
        {
            SelectGameType();
        }

        private void SelectGameType()
        {
            if(selectedGameTypeFlag == true) { return; }

            if (gameLanguage == Language.Japanese)
            {
                playerNameTextJP.text = displayPlayerName;
                gameNameTextJP.text = gameName;
            }
            else
            {
                playerNameTextEN.text = displayPlayerName;
                gameNameTextEN.text = gameName;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                selectRuleTypeID = ((selectRuleTypeID - 1) < 0) ? 2 : selectRuleTypeID - 1;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                selectRuleTypeID = ((selectRuleTypeID + 1) > 2) ? 0 : selectRuleTypeID + 1;
            }

            switch (selectRuleTypeID)
            {
                case 0:
                    GameObject.Find("BattleRoyaleBorder").GetComponent<Image>().color = selectColor;
                    GameObject.Find("1vs3Border").GetComponent<Image>().color = Color.black;
                    GameObject.Find("RandomGameTypeBorder").GetComponent<Image>().color = Color.black;
                    break;
                case 1:
                    GameObject.Find("1vs3Border").GetComponent<Image>().color = selectColor;
                    GameObject.Find("BattleRoyaleBorder").GetComponent<Image>().color = Color.black;
                    GameObject.Find("RandomGameTypeBorder").GetComponent<Image>().color = Color.black;
                    break;
                case 2:
                    GameObject.Find("RandomGameTypeBorder").GetComponent<Image>().color = selectColor;
                    GameObject.Find("1vs3Border").GetComponent<Image>().color = Color.black;
                    GameObject.Find("BattleRoyaleBorder").GetComponent<Image>().color = Color.black;
                    break;
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                selectedGameTypeFlag = true;
                switch (selectRuleTypeID)
                {
                    case 0:
                        sceneName = gameInfo.GetGameTitleEnglish(selectedGameIndex);
                        StartCoroutine(LoadGame(sceneName, loadGameTime));
                        break;
                    case 1:
                        sceneName = gameInfo.GetGameTitleEnglish(selectedGameIndex)+"1v3";
                        FindObjectOfType<GameManager>().ActiviatCanvas("SelectGhostPlayerCanvas");
                        break;
                    case 2:
                        int randomGameTypeIndex = Random.Range(0, 2);
                        if(randomGameTypeIndex == 0)
                        {
                            if (gameLanguage == Language.Japanese)
                            {
                                randomGameTypeTextJP.text = "バトルロイヤル";
                            }
                            else
                            {
                                randomGameTypeTextEN.text = "Battle Royale";

                            }
                            sceneName = gameInfo.GetGameTitleEnglish(selectedGameIndex);
                            StartCoroutine(LoadGame(sceneName, loadGameTime));
                        }
                        else
                        {
                            if (gameLanguage == Language.Japanese)
                            {
                                randomGameTypeTextJP.text = "1 vs 3";
                            }
                            else
                            {
                                randomGameTypeTextEN.text = "1 vs 3";
                            }
                            sceneName = gameInfo.GetGameTitleEnglish(selectedGameIndex) + "1v3";
                            FindObjectOfType<GameManager>().ActiviatCanvas("SelectGhostPlayerCanvas");
                        }
                        break;
                }
            }
        }

        public void SetPlayerName(int playerIndex)
        {
            string playerName = playerInfo.GetPlayerName(playerIndex);
            gameLanguage = mapInfo.GetGameLanguage();
            if (gameLanguage == Language.Japanese)
            {
                displayPlayerName = playerName + "は";
            }
            else
            {
                displayPlayerName = playerName;
            }
        }

        public void SetGameNameText(int gameIndex)
        {
            gameLanguage = mapInfo.GetGameLanguage();
            if (gameLanguage == Language.Japanese)
            {
                gameName = gameInfo.GetGameTitleJapanese(gameIndex);
            }
            else
            {
                gameName = gameInfo.GetGameTitleEnglish(gameIndex);
            }
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
    }
}
