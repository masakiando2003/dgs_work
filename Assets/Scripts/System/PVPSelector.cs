﻿using DHU2020.DGS.MiniGame.Map;
using DHU2020.DGS.MiniGame.Setting;
using DHU2020.DGS.MiniGame.System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DHU2020.DGS.MiniGame.Map.MapInfo;
using Random = UnityEngine.Random;

namespace DHU2020.DGS.MiniGame.System
{
    public class PVPSelector : MonoBehaviour
    {
        public MapInfo mapInfo;
        public Localization localeJP, localeEN;
        public PlayerInfo playerInfo;
        public PVPPlayerInfo pvpPlayerInfo;
        public GameObject[] players;
        public Text[] playerNamesText, rivalPlayerNamesText;
        public Text randomedPlayerText, selectPlayerText, selectPlayerHintText;
        public Color selectColor;
        public float enterGameTime = 1f;
        public List<string> playersName;

        private string selectedRivalPlayerName;
        private int playerIndex, selectedRivalPlayerID, originalSelectedRivalPlayerID;
        private bool selectedRivalPlayerFlag;
        private Language gameLanguage;

        // Start is called before the first frame update
        void Start()
        {
            Initialization();
        }

        private void Initialization()
        {
            gameLanguage = mapInfo.GetGameLanguage();
            if (gameLanguage == Language.Japanese)
            {
                selectPlayerText.text = localeJP.GetLabelContent("SelectRival");
                selectPlayerHintText.text = localeJP.GetLabelContent("SelectHint");
            }
            else
            {
                selectPlayerText.text = localeEN.GetLabelContent("SelectRival");
                selectPlayerHintText.text = localeEN.GetLabelContent("SelectHint");
            }
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

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                originalSelectedRivalPlayerID = selectedRivalPlayerID;
                // ランダムプレイヤー機能も含める
                selectedRivalPlayerID = ((selectedRivalPlayerID - 1) < 0) ? rivalPlayerNamesText.Length : selectedRivalPlayerID - 1;
                SelectRivalPlayer(selectedRivalPlayerID);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                originalSelectedRivalPlayerID = selectedRivalPlayerID;
                // ランダムプレイヤー機能も含める
                selectedRivalPlayerID = ((selectedRivalPlayerID + 1) >= rivalPlayerNamesText.Length+1) ? 0 : selectedRivalPlayerID + 1;
                SelectRivalPlayer(selectedRivalPlayerID);
            }
            else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                selectedRivalPlayerFlag = true;
                int selectedPlayerID = FindObjectOfType<GameManager>().GetSelectedPlayerID();
                // ランダムの場合
                if (selectedRivalPlayerID == rivalPlayerNamesText.Length)
                {
                    bool randomFinishedFlag = false;
                    while (!randomFinishedFlag)
                    {
                        int randomedRivalID = Random.Range(0, playerNamesText.Length);
                        //Debug.Log("randomedRivalID: "+ randomedRivalID);
                        if (randomedRivalID != selectedPlayerID && players[randomedRivalID].GetComponent<PlayerStatusManager>().IsAlive())
                        {
                            selectedRivalPlayerID = randomedRivalID;
                            selectedRivalPlayerName = playersName[selectedRivalPlayerID];
                            randomedPlayerText.text = selectedRivalPlayerName;
                            randomFinishedFlag = true;
                        }
                    }
                }
                else
                {
                    selectedRivalPlayerName = playerInfo.GetPlayerName(selectedRivalPlayerID);
                }
                string selectRivalPlayerName = playersName[selectedRivalPlayerID];
                pvpPlayerInfo.SetPlayerID(playerIndex, playerInfo.GetPlayerID(selectRivalPlayerName));
                //Debug.Log("selectedRivalPlayerID: " + selectedRivalPlayerID + ", selectRivalPlayerName: " + selectRivalPlayerName + ", RivalIndex: "+playersName.FindIndex(s => s == selectedRivalPlayerName));
                //Debug.Log("selectedRivalPlayerID: " + selectedRivalPlayerID + ", selectedRivalPlayerName: " + selectedRivalPlayerName);
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
            playersName.Clear();
            for (int i = 0, j = 0; i < playerNamesText.Length; i++)
            {
                if (i != selectedPlayerID)
                {
                    rivalPlayerNamesText[j].text = playerNamesText[i].text;
                    playersName.Add(playerInfo.GetPlayerName(i));
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
            //Debug.Log("Player Index: "+playerIndex);
        }
    }
}
