using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DHU2020.DGS.MiniGame.Game;
using Random = UnityEngine.Random;

namespace DHU2020.DGS.MiniGame.System
{
    public class GameSelector : MonoBehaviour
    {
        public GameInfo gameInfo;
        public GameObject[] games;
        public float loadGameTime = 2f;
        public Color selectColor;
        public Text selectedGameText;

        private List<int> randomedGameIndexes = new List<int>();
        private int selectedGameIndex, defaultSelectGameIndex, originalSelectedGameIndex, gameIndex;
        private string selectedGame;
        private bool selectedGameFlag, selectRandomGameFlag;

        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            selectedGameIndex = gameIndex = defaultSelectGameIndex;
            defaultSelectGameIndex = 0;
            selectColor.a = 1f;
            GameObject.Find("Game1Border").GetComponent<Image>().color = selectColor;
            selectedGameFlag = false;
            selectRandomGameFlag = false;
        }

        public void RandomizeGames()
        {
            selectedGameFlag = false;
            randomedGameIndexes.Clear();
            for (int i = 0; i < games.Length; i++)
            {
                bool randomGameFlag = true;
                while (randomGameFlag)
                {
                    int randomGameIndex = Random.Range(0, gameInfo.GetTotalGameCounts());
                    if (!randomedGameIndexes.Contains(randomGameIndex))
                    {
                        randomedGameIndexes.Add(randomGameIndex);
                        //GameObject.Find("Game" + (i + 1) + "Text").GetComponent<Text>().text = gameInfo.GetGameTitleJapanese(randomGameIndex);
                        GameObject.Find("Game" + (i + 1) + "Image").GetComponent<Image>().sprite = gameInfo.GetGameImage(randomGameIndex);
                        if (i == 0)
                        {
                            selectedGameText.text = gameInfo.GetGameTitleJapanese(randomGameIndex);
                            selectedGame = gameInfo.GetGameTitleEnglish(randomGameIndex);
                        }
                        randomGameFlag = false;
                    }
                }
            }

            GameObject.Find("Game1Border").GetComponent<Image>().color = selectColor;
            GameObject.Find("Game2Border").GetComponent<Image>().color = Color.black;
            GameObject.Find("Game3Border").GetComponent<Image>().color = Color.black;
            GameObject.Find("Game4Border").GetComponent<Image>().color = Color.black;
        }

        // Update is called once per frame
        void Update()
        {
            if(selectedGameFlag) { return; }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (!selectRandomGameFlag)
                {
                    originalSelectedGameIndex = gameIndex;
                    gameIndex = ((gameIndex - 1) < 0) ? games.Length - 1 : gameIndex - 1;
                    SelectGame(gameIndex);
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (!selectRandomGameFlag)
                {
                    originalSelectedGameIndex = gameIndex;
                    gameIndex = ((gameIndex + 1) >= games.Length) ? 0 : gameIndex + 1;
                    SelectGame(gameIndex);
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                selectRandomGameFlag = !selectRandomGameFlag;
                if (selectRandomGameFlag)
                {
                    SelectRandomGame(gameIndex);
                }
                else
                {
                    SelectGame(gameIndex);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                selectedGameFlag = true;
                if (selectRandomGameFlag)
                {
                    selectedGameIndex = Random.Range(0, games.Length);
                    selectedGameText.text = gameInfo.GetGameTitleJapanese(randomedGameIndexes[selectedGameIndex]);
                    selectedGame = gameInfo.GetGameSceneNameByJapaneseName(selectedGameText.text);
                }
                if (gameInfo.GetGameType(randomedGameIndexes[selectedGameIndex]) == GameInfo.GameType.PVP)
                {
                    FindObjectOfType<GameManager>().ActiviatCanvas("PVPSelectPlayerCanvas");
                }
                else if (gameInfo.GetGameType(randomedGameIndexes[selectedGameIndex]) == GameInfo.GameType.ThreePlayers)
                {
                }
                else if (gameInfo.GetGameType(randomedGameIndexes[selectedGameIndex]) == GameInfo.GameType.All)
                {
                    FindObjectOfType<GameManager>().EnterGame();
                }
            }
        }

        public void SelectGame(int gameIndex)
        {
            selectedGameIndex = gameIndex;

            GameObject.Find("RandomGameBorder").GetComponent<Image>().color = Color.black;
            GameObject.Find("Game" + (gameIndex + 1) + "Border").GetComponent<Image>().color = selectColor;
            if(originalSelectedGameIndex != gameIndex)
            {
                GameObject.Find("Game" + (originalSelectedGameIndex + 1) + "Border").GetComponent<Image>().color = Color.black;
            }
            selectedGameText.text = gameInfo.GetGameTitleJapanese(randomedGameIndexes[selectedGameIndex]);
            selectedGame = gameInfo.GetGameSceneNameByJapaneseName(selectedGameText.text);
        }

        public void SelectRandomGame(int gameIndex)
        {
            GameObject.Find("RandomGameBorder").GetComponent<Image>().color = selectColor;
            GameObject.Find("Game" + (gameIndex + 1) + "Border").GetComponent<Image>().color = Color.black;
            selectedGameText.text = "ランダム";
        }

        public float GetLoadGameTime()
        {
            return loadGameTime;
        }

        public string GetGameTitle()
        {
            return selectedGame;
        }
    }
}
