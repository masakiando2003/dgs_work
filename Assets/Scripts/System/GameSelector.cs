using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DHU2020.DGS.MiniGame.Game;
using Random = UnityEngine.Random;
using DHU2020.DGS.MiniGame.Map;
using static DHU2020.DGS.MiniGame.Map.MapInfo;
using DHU2020.DGS.MiniGame.Setting;

namespace DHU2020.DGS.MiniGame.System
{
    public class GameSelector : MonoBehaviour
    {
        public MapInfo mapInfo;
        public Localization localeJP, localeEN;
        public GameInfo gameInfo;
        public GameObject[] games;
        public float loadGameTime = 2f, randomGameTime = 1.5f, activateCanvasTime = 1.5f;
        public Color selectColor;
        public Text selectedGameText, selectGameText, randomGameText, selectGameHintText;

        private List<int> randomedGameIndexes = new List<int>();
        private int playerIndex, selectedGameIndex, defaultSelectGameIndex, originalSelectedGameIndex, gameIndex;
        private float controlFlowHorizontal, controlFlowVertical;
        private string selectedGame;
        private bool selectedGameFlag, selectRandomGameFlag, leftAxisDown, rightAxisDown, upAxisDown, downAxisDown;
        private Language gameLanguage;

        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            playerIndex = 0;
            controlFlowHorizontal = 0;
            controlFlowVertical = 0;
            leftAxisDown = false;
            rightAxisDown = false;
            upAxisDown = false;
            downAxisDown = false;
            gameLanguage = mapInfo.GetGameLanguage();
            if(gameLanguage == Language.Japanese)
            {
                selectGameText.text = localeJP.GetLabelContent("SelectAnyGame");
                randomGameText.text = localeJP.GetLabelContent("Random");
                selectGameHintText.text = localeJP.GetLabelContent("SelectGameHintText");
            }
            else
            {
                selectGameText.text = localeEN.GetLabelContent("SelectAnyGame");
                randomGameText.text = localeEN.GetLabelContent("Random");
                selectGameHintText.text = localeEN.GetLabelContent("SelectGameHintText");
            }
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
                            if(gameLanguage == Language.Japanese)
                            {
                                selectedGameText.text = gameInfo.GetGameTitleJapanese(randomGameIndex);
                            }
                            else
                            {
                                selectedGameText.text = gameInfo.GetGameTitleEnglish(randomGameIndex);
                            }
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
            if (selectedGameFlag == true) { return; }

            if (Input.GetAxis("P" + (playerIndex+1) + "Vertical") > 0 && !upAxisDown)
            {
                upAxisDown = true;
                StartCoroutine(JoystickAxisMoved("up"));

            }
            else if (Input.GetAxis("P" + (playerIndex + 1) + "Vertical") == 0)
            {
                upAxisDown = false;
            }

            if (Input.GetAxis("P" + (playerIndex + 1) + "Vertical") < 0 && !downAxisDown)
            {
                downAxisDown = true;
                StartCoroutine(JoystickAxisMoved("down"));

            }
            else if (Input.GetAxis("P" + (playerIndex + 1) + "Vertical") == 0)
            {
                downAxisDown = false;
            }

            if (Input.GetAxis("P" + (playerIndex + 1) + "Horizontal") > 0 && !rightAxisDown)
            {
                rightAxisDown = true;
                StartCoroutine(JoystickAxisMoved("right"));

            }
            else if (Input.GetAxis("P" + (playerIndex + 1) + "Horizontal") == 0)
            {
                rightAxisDown = false;
            }

            if (Input.GetAxis("P" + (playerIndex + 1) + "Horizontal") < 0 && !leftAxisDown)
            {
                leftAxisDown = true;
                StartCoroutine(JoystickAxisMoved("left"));

            }
            else if (Input.GetAxis("P" + (playerIndex + 1) + "Horizontal") == 0)
            {
                leftAxisDown = false;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) )
            {
                if (!selectRandomGameFlag)
                {
                    originalSelectedGameIndex = gameIndex;
                    gameIndex = ((gameIndex - 1) < 0) ? games.Length - 1 : gameIndex - 1;
                    SelectGame(gameIndex);
                    controlFlowHorizontal = 0;
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) )
            {
                if (!selectRandomGameFlag)
                {
                    originalSelectedGameIndex = gameIndex;
                    gameIndex = ((gameIndex + 1) >= games.Length) ? 0 : gameIndex + 1;
                    SelectGame(gameIndex);
                    controlFlowHorizontal = 0;
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) )
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
                controlFlowVertical = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || 
                     Input.GetButtonDown("P" + (playerIndex + 1) + "DecideButton"))
            {
                selectedGameFlag = true;
                if (selectRandomGameFlag)
                {
                    StartCoroutine(SelectRandomGameFromRandomedGames());
                }
                if (gameInfo.GetGameType(randomedGameIndexes[selectedGameIndex]) == GameInfo.GameType.PVP)
                {
                    FindObjectOfType<GameManager>().ActivateCanvas("PVPSelectPlayerCanvas");
                }
                else if (gameInfo.GetGameType(randomedGameIndexes[selectedGameIndex]) == GameInfo.GameType.ThreePlayers)
                {
                }
                else if (gameInfo.GetGameType(randomedGameIndexes[selectedGameIndex]) == GameInfo.GameType.All)
                {
                    FindObjectOfType<GameManager>().EnterGame();
                }
                else if(gameInfo.GetGameType(randomedGameIndexes[selectedGameIndex]) == GameInfo.GameType.MultipleType)
                {
                    FindObjectOfType<GameManager>().ActivateCanvas("SelectGameTypeCanvas");
                }
            }
        }

        private IEnumerator SelectRandomGameFromRandomedGames()
        {
            selectedGameIndex = Random.Range(0, games.Length);
            if (gameLanguage == Language.Japanese)
            {
                selectedGameText.text = gameInfo.GetGameTitleJapanese(randomedGameIndexes[selectedGameIndex]);
                selectedGame = gameInfo.GetGameSceneNameByJapaneseName(selectedGameText.text);
            }
            else
            {
                selectedGameText.text = gameInfo.GetGameTitleEnglish(randomedGameIndexes[selectedGameIndex]);
                selectedGame = gameInfo.GetGameTitleEnglish(randomedGameIndexes[selectedGameIndex]);
            }
            yield return new WaitForSeconds(randomGameTime);
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
            if (gameLanguage == Language.Japanese)
            {
                selectedGameText.text = gameInfo.GetGameTitleJapanese(randomedGameIndexes[selectedGameIndex]);
                selectedGame = gameInfo.GetGameSceneNameByJapaneseName(selectedGameText.text);
            }
            else
            {
                string gameNameEN = gameInfo.GetGameTitleEnglish(randomedGameIndexes[selectedGameIndex]);
                selectedGameText.text = gameNameEN;
                selectedGame = gameNameEN;
            }
        }

        public int GetSelectedGameIndex()
        {
            return randomedGameIndexes[selectedGameIndex];
        }

        public void SelectRandomGame(int gameIndex)
        {
            GameObject.Find("RandomGameBorder").GetComponent<Image>().color = selectColor;
            GameObject.Find("Game" + (gameIndex + 1) + "Border").GetComponent<Image>().color = Color.black;
            if (gameLanguage == Language.Japanese)
            {
                selectedGameText.text = localeJP.GetLabelContent("Random");
            }
            else
            {
                selectedGameText.text = localeEN.GetLabelContent("Random");
            }
        }

        public float GetLoadGameTime()
        {
            return loadGameTime;
        }

        public string GetGameTitle()
        {
            return selectedGame;
        }

        public void SetChooseGamePlayerIndex(int player)
        {
            playerIndex = player;
        }

        private IEnumerator JoystickAxisMoved(string axisName)
        {
            switch (axisName)
            {
                case "up":
                    upAxisDown = true;
                    yield return new WaitForEndOfFrame();
                    upAxisDown = false;
                    break;
                case "down":
                    downAxisDown = true;
                    yield return new WaitForEndOfFrame();
                    downAxisDown = false;
                    break;
                case "left":
                    leftAxisDown = true;
                    yield return new WaitForEndOfFrame();
                    leftAxisDown = false;
                    break;
                case "right":
                    rightAxisDown = true;
                    yield return new WaitForEndOfFrame();
                    rightAxisDown = false;
                    break;
            }
        }
    }
}
