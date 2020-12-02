using DHU2020.DGS.MiniGame.Map;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DHU2020.DGS.MiniGame.System
{
    public class Option : MonoBehaviour
    {
        public MapInfo mapInfo;
        public Slider maxTurnSlider;
        public GameTitle gameTitle;
        public Text[] optionLabels;
        public Text maxTurnText;

        private int maxTurn, defaultMaxTurn, selectedOptionIndex;
        private bool changeMaxTurnFlag;

        // Start is called before the first frame update
        void Start()
        {
            Initialization();
        }

        private void Initialization()
        {
            defaultMaxTurn = mapInfo.GetMaxTurns();
            maxTurn = defaultMaxTurn;
            maxTurnSlider.value = maxTurn;
            maxTurnText.text = maxTurn.ToString();
            selectedOptionIndex = 0;
            if(optionLabels.Length > 0)
            {
                GameObject.Find(optionLabels[0].name + "Background").GetComponent<Image>().color = Color.black;
                optionLabels[0].GetComponent<Text>().color = Color.white;
                if(optionLabels[0].name == "MaxTurnLabel")
                {
                    EnableChangeTurnFlag();
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
            if (Input.GetKeyDown(KeyCode.Return))
            {
                switch (selectedOptionIndex)
                {
                    case -1:
                        SaveChanges();
                        break;
                    case -2:
                        UnsaveChanges();
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

        private void ChangeSelectOption(string input)
        {
            // セーブ: -1
            // タイトル(セーブしない): -2
            switch (input) {
                case "down":
                    selectedOptionIndex--;
                    if (selectedOptionIndex < -2)
                    {
                        selectedOptionIndex = optionLabels.Length - 1;
                    }
                    break;
                case "up":
                    selectedOptionIndex++;
                    if (selectedOptionIndex >= optionLabels.Length)
                    {
                        selectedOptionIndex = -2;
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
                    GameObject.Find("SaveTextBackground").GetComponent<Image>().color = Color.black;
                    GameObject.Find("SaveText").GetComponent<Text>().color = Color.white;
                    break;
                case -2:
                    DisableChangeTurnFlag();
                    GameObject.Find("TitleTextBackground").GetComponent<Image>().color = Color.black;
                    GameObject.Find("TitleText").GetComponent<Text>().color = Color.white;
                    break;
                default:
                    Debug.Log(optionLabels[selectedOptionIndex].name);
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
                    break;
            }
        }

        private void SaveMaxTurns()
        {
            mapInfo.SetMaxTurns(maxTurn);
        }

        private void SaveChanges()
        {
            SaveMaxTurns();
            gameTitle.ReturnToMenu();
        }

        private void UnsaveChanges()
        {
            gameTitle.ReturnToMenu();
        }
    }
}