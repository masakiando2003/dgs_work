using DHU2020.DGS.MiniGame.Map;
using DHU2020.DGS.MiniGame.Setting;
using DHU2020.DGS.MiniGame.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DHU2020.DGS.MiniGame.System
{
    public class PVPSelector : MonoBehaviour
    {
        public PlayerInfo playerInfo;
        public PVPPlayerInfo pvpPlayerInfo;
        public GameObject[] players;
        public Text[] playerNames, rivalPlayerNames;
        public Text randomedPlayerText;
        public Color selectColor;
        public float enterGameTime = 1f;

        private string selectedRivalPlayerName;
        private int playerIndex, selectedRivalPlayerID, originalSelectedRivalPlayerID;
        private bool selectedRivalPlayerFlag;

        // Start is called before the first frame update
        void Start()
        {
            Initialization();
        }

        private void Initialization()
        {
            playerIndex = 0;
            selectColor.a = 1f;
            GameObject.Find("FirstPlayerBorder").GetComponent<Image>().color = selectColor;
            randomedPlayerText.text = "";
            selectedRivalPlayerFlag = false;
        }

        // Update is called once per frame
        void Update()
        {
            if(selectedRivalPlayerFlag) { return; }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || (Input.GetAxis("P" + (playerIndex + 1) + "Horizontal") == -1))
            {
                originalSelectedRivalPlayerID = selectedRivalPlayerID;
                // ランダムプレイヤー機能も含める
                selectedRivalPlayerID = ((selectedRivalPlayerID - 1) < 0) ? rivalPlayerNames.Length : selectedRivalPlayerID - 1;
                SelectRivalPlayer(selectedRivalPlayerID);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || (Input.GetAxis("P" + (playerIndex + 1) + "Horizontal") == 1))
            {
                originalSelectedRivalPlayerID = selectedRivalPlayerID;
                // ランダムプレイヤー機能も含める
                selectedRivalPlayerID = ((selectedRivalPlayerID + 1) >= rivalPlayerNames.Length+1) ? 0 : selectedRivalPlayerID + 1;
                SelectRivalPlayer(selectedRivalPlayerID);
            }
            else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) ||
                     Input.GetButtonDown("P" + (playerIndex + 1) + "DecideButton"))
            {
                selectedRivalPlayerFlag = true;
                int selectedPlayerID = FindObjectOfType<GameManager>().GetSelectedPlayerID();
                // ランダムの場合
                if (selectedRivalPlayerID == rivalPlayerNames.Length)
                {
                    bool randomFinishedFlag = false;
                    while (!randomFinishedFlag)
                    {
                        int randomedRivalID = Random.Range(0, playerNames.Length);
                        Debug.Log("randomedRivalID: "+ randomedRivalID);
                        if (randomedRivalID != selectedPlayerID && players[randomedRivalID].GetComponent<PlayerStatusManager>().IsAlive())
                        {
                            selectedRivalPlayerID = randomedRivalID;
                            selectedRivalPlayerName = playerInfo.GetPlayerName(selectedRivalPlayerID);
                            randomedPlayerText.text = selectedRivalPlayerName;
                            randomFinishedFlag = true;
                        }
                    }
                }
                else
                {
                    selectedRivalPlayerName = playerInfo.GetPlayerName(selectedRivalPlayerID);
                }
                pvpPlayerInfo.SetPlayerID(selectedPlayerID, playerInfo.GetPlayerID(selectedRivalPlayerName));
                StartCoroutine(EnterGame());
            }
        }

        IEnumerator EnterGame()
        {
            yield return new WaitForSeconds(enterGameTime);
            FindObjectOfType<GameManager>().EnterGame();
        }

        public void InitializeRivalPlayerList()
        {
            selectedRivalPlayerFlag = false;
            int selectedPlayerID = FindObjectOfType<GameManager>().GetSelectedPlayerID();
            for (int i = 0, j = 0; i < playerNames.Length; i++)
            {
                if(i != selectedPlayerID)
                {
                    rivalPlayerNames[j].text = playerNames[i].text;
                    j++;
                }
            }
            randomedPlayerText.text = "";
        }

        public void SelectRivalPlayer(int selectedPlayerID)
        {
            string playerNo = "", originalPlayerNo;
            switch (selectedPlayerID)
            {
                case 0:
                    playerNo = "First";
                    break;
                case 1:
                    playerNo = "Second";
                    break;
                case 2:
                    playerNo = "Third";
                    break;
                default:
                    playerNo = "Random";
                    break;
            }
            switch (originalSelectedRivalPlayerID)
            {
                case 0:
                    originalPlayerNo = "First";
                    break;
                case 1:
                    originalPlayerNo = "Second";
                    break;
                case 2:
                    originalPlayerNo = "Third";
                    break;
                default:
                    originalPlayerNo = "Random";
                    break;
            }

            GameObject.Find(playerNo + "PlayerBorder").GetComponent<Image>().color = selectColor;
            GameObject.Find(originalPlayerNo + "PlayerBorder").GetComponent<Image>().color = Color.black;
        }

        public void SetPVPSelectorPlayerIndex(int player)
        {
            playerIndex = player;
        }
    }
}
