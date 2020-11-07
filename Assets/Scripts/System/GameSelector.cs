using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DHU2020.DGS.MiniGame.Game;
using Random = UnityEngine.Random;
using System.Linq;

public class GameSelector : MonoBehaviour
{
    public GameInfo gameInfo;
    public GameObject[] games;
    public int defaultSelectGameIndex = 0;
    public float loadGameTime = 2f;
    public Color selectColor;
    public Text selectedGameText;

    private List<int> randomedGameIndexes = new List<int>();
    [SerializeField] private int selectedGameIndex, originalSelectedGameIndex, gameIndex;
    private string selectedGame;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        selectedGameIndex = gameIndex = defaultSelectGameIndex;
        selectColor.a = 1f;
        games[gameIndex].transform.Find("Game1Border").GetComponent<Image>().color = selectColor;
        selectedGameText.text = "";
    }

    public void RandomizeGames()
    {
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
                    GameObject.Find("Game" + (i + 1) + "Text").GetComponent<Text>().text = gameInfo.GetGameTitleJapanese(randomGameIndex);
                    if (i == 0)
                    {
                        selectedGameText.text = gameInfo.GetGameTitleJapanese(randomGameIndex);
                        selectedGame = gameInfo.GetGameTitleEnglish(randomGameIndex);
                    }
                    randomGameFlag = false;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            originalSelectedGameIndex = gameIndex;
            gameIndex = ((gameIndex - 1) < 0) ? games.Length - 1 : gameIndex - 1;
            SelectGame(gameIndex);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            originalSelectedGameIndex = gameIndex;
            gameIndex = ((gameIndex + 1) >= games.Length) ? 0 : gameIndex + 1;
            SelectGame(gameIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            EnterGame();
        }
    }

    public void SelectGame(int gameIndex)
    {
        selectedGameIndex = gameIndex;

        GameObject.Find("Game" + (gameIndex + 1) + "Border").GetComponent<Image>().color = selectColor;
        GameObject.Find("Game" + (originalSelectedGameIndex + 1) + "Border").GetComponent<Image>().color = Color.black;
        selectedGameText.text = gameInfo.GetGameTitleJapanese(randomedGameIndexes[selectedGameIndex]);
        selectedGame = gameInfo.GetGameTitleEnglish(randomedGameIndexes[selectedGameIndex]);
    }

    public void EnterGame()
    {
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(loadGameTime);
        SceneManager.LoadScene(selectedGame);
    }
    
}
