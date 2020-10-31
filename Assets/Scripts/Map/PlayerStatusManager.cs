using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusManager : MonoBehaviour
{
    public GameObject[] PlayerCrossesObjects;
    public GameObject GameOverObject;
    public string PlayerName { get; set; }
    public Text playerNameText;
    public int PlayerID { get; set; }
    private int currentLife, maxLife;
    public KeyCode increaseLifeKey, decreaseLifeKey;
    private bool isAlive;

    // Start is called before the first frame update
    void Start()
    {
        maxLife = PlayerCrossesObjects.Length;
        currentLife = maxLife;
        for(int i=0; i < PlayerCrossesObjects.Length; i++)
        {
            PlayerCrossesObjects[i].SetActive(false);
        }
        GameOverObject.SetActive(false);
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        // 開発用
        if (isAlive)
        {
            if (Input.GetKeyDown(increaseLifeKey))
            {
                IncreaseLife();
            }
            else if (Input.GetKeyDown(decreaseLifeKey))
            {
                DecreaseLife();
                CheckIsGameOver();
            }
        }
    }

    void DecreaseLife()
    {
        currentLife--;
        if (currentLife < 0)
        {
            currentLife = 0;
        }
        EnableCross(currentLife);
    }

    void IncreaseLife()
    {
        currentLife++;
        if(currentLife > maxLife)
        {
            currentLife = maxLife;
        }
        EnableCross(currentLife);
    }

    void EnableCross(int currentLife)
    {
        // 一旦全部クロスを消す
        for (int i = 0; i < PlayerCrossesObjects.Length; i++)
        {
            PlayerCrossesObjects[i].SetActive(false);
        }

        int numOfCrosses = maxLife - currentLife;
        for(int i = 0; i < numOfCrosses; i++)
        {
            PlayerCrossesObjects[i].SetActive(true);
        }
    }

    void CheckIsGameOver()
    {
        if(currentLife < 1)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        GameOverObject.SetActive(true);
        isAlive = false;
        FindObjectOfType<GameManager>().CheckRemainingPlayers();
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    void DisplayPlayerName()
    {
        playerNameText.text = PlayerName;
    }
}
