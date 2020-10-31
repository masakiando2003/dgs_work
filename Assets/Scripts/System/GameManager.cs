using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// GameManagerのインスタンス
    /// </summary>
    public static GameManager Instance
    {
        get; private set;
    }

    public GameObject[] players;
    public int numOfWinningPlayers = 1;
    private int remainingPlayers;
    [Range(1,99)]
    public int maxTurns;
    private int currentTurn;
    public Text currentTurnText, maxTurnsText;

    // Start is called before the first frame update
    void Start()
    {
        currentTurn = 1;
        currentTurnText.text = currentTurn.ToString();
        maxTurnsText.text = maxTurns.ToString();
        remainingPlayers = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckRemainingPlayers()
    {
        remainingPlayers = 0;// 一旦リセットします
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponentInChildren<PlayerStatusManager>().IsAlive())
            {
                remainingPlayers++;
            }
        }
        Debug.Log("RemainingPlayers: " + remainingPlayers);

        if (remainingPlayers <= numOfWinningPlayers)
        {
            Winner();
        }
    }

    void Winner()
    {
        List<int> winnerPlayerIDs = new List<int>();
        List<string> winnerPlayerNames = new List<string>();
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponentInChildren<PlayerStatusManager>().IsAlive())
            {
                winnerPlayerIDs.Add(players[i].GetComponentInChildren<PlayerStatusManager>().PlayerID);
                winnerPlayerNames.Add(players[i].GetComponentInChildren<PlayerStatusManager>().PlayerName);
            }
        }
    }

    public void AddTurn()
    {
        currentTurn++;
        currentTurnText.text = currentTurn.ToString();
    }

    bool checkTurnsIsReachedMax()
    {
        return currentTurn >= maxTurns;
    }
}
