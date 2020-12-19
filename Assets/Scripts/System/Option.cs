using DHU2020.DGS.MiniGame.Map;
using DHU2020.DGS.MiniGame.Setting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DHU2020.DGS.MiniGame.System
{
    public class Option : MonoBehaviour
    {
        public PlayerInfo playerInfo;
        public MapInfo mapInfo;
        public Slider maxTurnSlider;
        public GameTitle gameTitle;
        public Text[] optionLabels;
        public Text maxTurnText;

        private int maxTurn, defaultMaxTurn, selectedOptionIndex, selectedPlayerIDControllerIndex;
        private bool changeMaxTurnFlag, setPlayerControllerFlag, isSettingPlayerControllerFlag;

        // Start is called before the first frame update
        void Start()
        {
            Initialization();
        }

        private void Initialization()
        {
            changeMaxTurnFlag = false;
            setPlayerControllerFlag = false;
            isSettingPlayerControllerFlag = false;
            defaultMaxTurn = mapInfo.GetMaxTurns();
            maxTurn = defaultMaxTurn;
            maxTurnSlider.value = maxTurn;
            maxTurnText.text = maxTurn.ToString();
            selectedOptionIndex = 0;
            selectedPlayerIDControllerIndex = 0;
            if (optionLabels.Length > 0)
            {
                GameObject.Find(optionLabels[0].name + "Background").GetComponent<Image>().color = Color.black;
                optionLabels[0].GetComponent<Text>().color = Color.white;
                if(optionLabels[0].name == "MaxTurnLabel")
                {
                    EnableChangeTurnFlag();
                }
                else if(optionLabels[0].name == "PlayerControllerSettingLabel")
                {
                    EnableSetPlayerControllerFlag();
                }
            }
            for(int playerIndex= 0; playerIndex < playerInfo.GetPlayersCount(); playerIndex++)
            {
                int playerID = playerIndex + 1;
                GameObject.Find("Player" + playerID + "ControllerLabelBackground").GetComponent<Image>().color = Color.white;
                GameObject.Find("Player" + playerID + "ControllerLabelText").GetComponent<Text>().color = Color.black;
                if (playerInfo.GetPlayerControllerInput(playerIndex) == PlayerInfo.PlayerControllerInput.Keyboard)
                {
                    GameObject.Find("Player" + playerID + "ControllerKeyboardBackground").GetComponent<Image>().color = Color.black;
                    GameObject.Find("Player" + playerID + "ControllerKeyboardText").GetComponent<Text>().color = Color.white;
                    GameObject.Find("Player" + playerID + "ControllerJoyStickBackground").GetComponent<Image>().color = Color.white;
                    GameObject.Find("Player" + playerID + "ControllerJoyStickText").GetComponent<Text>().color = Color.black;
                }
                else
                {
                    GameObject.Find("Player" + playerID + "ControllerJoyStickBackground").GetComponent<Image>().color = Color.black;
                    GameObject.Find("Player" + playerID + "ControllerJoyStickText").GetComponent<Text>().color = Color.white;
                    GameObject.Find("Player" + playerID + "ControllerKeyboardBackground").GetComponent<Image>().color = Color.white;
                    GameObject.Find("Player" + playerID + "ControllerKeyboardText").GetComponent<Text>().color = Color.black;
                }
            }
            GameObject.Find("SaveTextBackground").GetComponent<Image>().color = Color.white;
            GameObject.Find("SaveText").GetComponent<Text>().color = Color.black;
            GameObject.Find("TitleTextBackground").GetComponent<Image>().color = Color.white;
            GameObject.Find("TitleText").GetComponent<Text>().color = Color.black;
        }

        private void EnableChangeTurnFlag()
        {
            changeMaxTurnFlag = true;
        }

        private void DisableChangeTurnFlag()
        {
            changeMaxTurnFlag = false;
        }

        private void EnableSetPlayerControllerFlag()
        {
            setPlayerControllerFlag = true;
        }

        private void DisableSetPlayerControllerFlag()
        {
            setPlayerControllerFlag = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ChangeSelectOption("up");
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ChangeSelectOption("down");
            }
            if (changeMaxTurnFlag)
            {
                SetMaxTurns();
            }
            if (setPlayerControllerFlag)
            {
                SetPlayerControllers();
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                switch (selectedOptionIndex)
                {
                    case -1:
                        UnsaveChanges();
                        break;
                    case -2:
                        SaveChanges();
                        break;
                }
            }
        }

        private void SetMaxTurns()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                maxTurn = (maxTurn -1 < 10) ? 10 : maxTurn - 1;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                maxTurn = (maxTurn + 1 > 99) ? 99 : maxTurn + 1;
            }
            maxTurnSlider.value = maxTurn;
            maxTurnText.text = maxTurn.ToString();
        }

        private void SetPlayerControllers()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (!isSettingPlayerControllerFlag)
                {
                    isSettingPlayerControllerFlag = true;
                    selectedPlayerIDControllerIndex = 1;
                    GameObject.Find("PlayerControllerSettingLabelBackground").GetComponent<Image>().color = Color.white;
                    GameObject.Find("PlayerControllerSettingLabel").GetComponent<Text>().color = Color.black;
                    GameObject.Find("Player" + selectedPlayerIDControllerIndex + "ControllerLabelBackground").GetComponent<Image>().color = Color.black;
                    GameObject.Find("Player" + selectedPlayerIDControllerIndex + "ControllerLabelText").GetComponent<Text>().color = Color.white;
                }
                else
                {
                    isSettingPlayerControllerFlag = false;
                    GameObject.Find("Player" + selectedPlayerIDControllerIndex + "ControllerLabelBackground").GetComponent<Image>().color = Color.white;
                    GameObject.Find("Player" + selectedPlayerIDControllerIndex + "ControllerLabelText").GetComponent<Text>().color = Color.black;
                    GameObject.Find("PlayerControllerSettingLabelBackground").GetComponent<Image>().color = Color.black;
                    GameObject.Find("PlayerControllerSettingLabel").GetComponent<Text>().color = Color.white;
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && isSettingPlayerControllerFlag)
            {
                GameObject.Find("Player" + selectedPlayerIDControllerIndex + "ControllerLabelBackground").GetComponent<Image>().color = Color.white;
                GameObject.Find("Player" + selectedPlayerIDControllerIndex + "ControllerLabelText").GetComponent<Text>().color = Color.black;
                selectedPlayerIDControllerIndex = (selectedPlayerIDControllerIndex + 1) > playerInfo.GetPlayersCount()
                                                    ? 1 : selectedPlayerIDControllerIndex + 1;
                GameObject.Find("Player" + selectedPlayerIDControllerIndex + "ControllerLabelBackground").GetComponent<Image>().color = Color.black;
                GameObject.Find("Player" + selectedPlayerIDControllerIndex + "ControllerLabelText").GetComponent<Text>().color = Color.white;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && isSettingPlayerControllerFlag)
            {
                GameObject.Find("Player" + selectedPlayerIDControllerIndex + "ControllerLabelBackground").GetComponent<Image>().color = Color.white;
                GameObject.Find("Player" + selectedPlayerIDControllerIndex + "ControllerLabelText").GetComponent<Text>().color = Color.black;
                selectedPlayerIDControllerIndex = (selectedPlayerIDControllerIndex - 1) < 1
                                                    ? playerInfo.GetPlayersCount() : selectedPlayerIDControllerIndex - 1;
                GameObject.Find("Player" + selectedPlayerIDControllerIndex + "ControllerLabelBackground").GetComponent<Image>().color = Color.black;
                GameObject.Find("Player" + selectedPlayerIDControllerIndex + "ControllerLabelText").GetComponent<Text>().color = Color.white;
            }
        }

        private void ChangeSelectOption(string input)
        {
            // 各プレイヤー操作を設定する時、他の項目に変更出来ない
            if(isSettingPlayerControllerFlag) { return; }

            // セーブ: -1
            // タイトル(セーブしない): -2
            switch (input) {
                case "down":
                    selectedOptionIndex++;
                    if (selectedOptionIndex >= optionLabels.Length)
                    {
                        selectedOptionIndex = -2;
                    }
                    break;
                case "up":
                    selectedOptionIndex--;
                    if (selectedOptionIndex < -2)
                    {
                        selectedOptionIndex = optionLabels.Length - 1;
                    }
                    break;
            }
            
            SelectOptionItem(selectedOptionIndex);
        }

        private void SelectOptionItem(int selectedOptionIndex)
        {
            // 一旦全部"クリアします
            for(int i=0; i < optionLabels.Length; i++)
            {
                GameObject.Find(optionLabels[i].name + "Background").GetComponent<Image>().color = Color.white;
                optionLabels[i].GetComponent<Text>().color = Color.black;
            }
            GameObject.Find("SaveTextBackground").GetComponent<Image>().color = Color.white;
            GameObject.Find("SaveText").GetComponent<Text>().color = Color.black;
            GameObject.Find("TitleTextBackground").GetComponent<Image>().color = Color.white;
            GameObject.Find("TitleText").GetComponent<Text>().color = Color.black;

            switch (selectedOptionIndex)
            {
                case -1:
                    DisableChangeTurnFlag();
                    GameObject.Find("TitleTextBackground").GetComponent<Image>().color = Color.black;
                    GameObject.Find("TitleText").GetComponent<Text>().color = Color.white;
                    break;
                case -2:
                    DisableChangeTurnFlag();
                    GameObject.Find("SaveTextBackground").GetComponent<Image>().color = Color.black;
                    GameObject.Find("SaveText").GetComponent<Text>().color = Color.white;
                    break;
                default:
                    GameObject.Find(optionLabels[selectedOptionIndex].name + "Background").GetComponent<Image>().color = Color.black;
                    optionLabels[selectedOptionIndex].GetComponent<Text>().color = Color.white;
                    if(optionLabels[selectedOptionIndex].name == "MaxTurnLabel")
                    {
                        EnableChangeTurnFlag();
                    }
                    else
                    {
                        DisableChangeTurnFlag();
                    }
                    if(optionLabels[selectedOptionIndex].name == "PlayerControllerSettingLabel")
                    {
                        EnableSetPlayerControllerFlag();
                    }
                    else
                    {
                        DisableSetPlayerControllerFlag();
                    }
                    break;
            }
        }

        private void SaveMaxTurns()
        {
            mapInfo.SetMaxTurns(maxTurn);
        }

        private void SavePlayerControllerInput()
        {
            
        }

        private void SaveChanges()
        {
            SaveMaxTurns();
            SavePlayerControllerInput();
            gameTitle.ReturnToMenu();
        }

        private void UnsaveChanges()
        {
            gameTitle.ReturnToMenu();
        }
    }
}