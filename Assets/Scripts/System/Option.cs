using DHU2020.DGS.MiniGame.Map;
using DHU2020.DGS.MiniGame.Setting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DHU2020.DGS.MiniGame.Map.MapInfo;
using static DHU2020.DGS.MiniGame.Setting.PlayerInfo;

namespace DHU2020.DGS.MiniGame.System
{
    public class Option : MonoBehaviour
    {
        public Localization japaneseLocale, englishLocale;
        public MapInfo mapInfo;
        public Slider maxTurnSlider;
        public GameObject gameRuleCanvas;
        public GameTitle gameTitle;
        public Text[] optionLabels;
        public Text maxTurnLabel, langaugeLabel, saveLabel, titleLabel, maxTurnText, settingHints;
        
        private Language selectedLanguage;
        private int maxTurn, defaultMaxTurn, selectedOptionIndex;
        private string setMaxTurnsHints, setLanguageHints, saveHints, titleHints;
        private bool changeMaxTurnFlag, changeLanguageFlag, gameRuleFlag;

        // Start is called before the first frame update
        void Start()
        {
            Initialization();
        }

        private void Initialization()
        {
            changeMaxTurnFlag = false;
            changeLanguageFlag = false;
            gameRuleFlag = false;
            defaultMaxTurn = mapInfo.GetMaxTurns();
            maxTurn = defaultMaxTurn;
            maxTurnSlider.value = maxTurn;
            maxTurnText.text = maxTurn.ToString();
            selectedOptionIndex = 0;
            selectedLanguage = mapInfo.GetGameLanguage();
            if(selectedLanguage == Language.Japanese)
            {
                ShowLanguage("Japanese");
            }
            else
            {
                ShowLanguage("English");
            }
            if (optionLabels.Length > 0)
            {
                GameObject.Find(optionLabels[0].name + "Background").GetComponent<Image>().color = Color.black;
                optionLabels[0].GetComponent<Text>().color = Color.white;
                if(optionLabels[0].name == "MaxTurnLabel")
                {
                    EnableChangeTurnFlag();
                    settingHints.text = setMaxTurnsHints;
                }
                else if(optionLabels[0].name == "LanguageLabel")
                {
                    EnableSetLanguageFlag();
                    settingHints.text = setLanguageHints;
                }
            }
            GameObject.Find("SaveLabelBackground").GetComponent<Image>().color = Color.white;
            GameObject.Find("SaveLabel").GetComponent<Text>().color = Color.black;
            GameObject.Find("TitleLabelBackground").GetComponent<Image>().color = Color.white;
            GameObject.Find("TitleLabel").GetComponent<Text>().color = Color.black;
        }

        private void EnableChangeTurnFlag()
        {
            changeMaxTurnFlag = true;
        }

        private void DisableChangeTurnFlag()
        {
            changeMaxTurnFlag = false;
        }

        private void EnableSetLanguageFlag()
        {
            changeLanguageFlag = true;
        }

        private void DisableSetLanguageFlag()
        {
            changeLanguageFlag = false;
        }

        private void EnableGameRuleSettingFlag()
        {
            gameRuleFlag = true;
        }

        private void DisbleGameRuleSettingFlag()
        {
            gameRuleFlag = false;
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
            if (changeLanguageFlag)
            {
                SetLanguage();
            }
            if (gameRuleFlag)
            {
                GameRuleSetting();
            }
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
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
                maxTurn = (maxTurn -1 < mapInfo.GetMinTurns()) ? mapInfo.GetMinTurns() : maxTurn - 1;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                maxTurn = (maxTurn + 1 > mapInfo.GetMaxTurns()) ? mapInfo.GetMaxTurns() : maxTurn + 1;
            }
            maxTurnSlider.value = maxTurn;
            maxTurnText.text = maxTurn.ToString();
        }

        private void SetLanguage()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                if(selectedLanguage == Language.Japanese)
                {
                    selectedLanguage = Language.English;
                    ShowLanguage("English");
                }
                else
                {
                    selectedLanguage = Language.Japanese;
                    ShowLanguage("Japanese");
                }
            }
        }

        private void GameRuleSetting()
        {
            changeMaxTurnFlag = false;
            changeLanguageFlag = false;
            gameRuleCanvas.SetActive(true);
        }

        public void ReturnToOption()
        {
            gameRuleCanvas.SetActive(false);
        }

        private void ShowLanguage(string language)
        {
            if (language == "Japanese")
            {
                GameObject.Find("LanguageJapaneseLabel").GetComponent<Image>().color = Color.black;
                GameObject.Find("LanguageJapaneseText").GetComponent<Text>().color = Color.white;
                GameObject.Find("LanguageEnglishLabel").GetComponent<Image>().color = Color.white;
                GameObject.Find("LanguageEnglishText").GetComponent<Text>().color = Color.black;
                maxTurnLabel.text = japaneseLocale.GetLabelContent("MaxTurns");
                langaugeLabel.text = japaneseLocale.GetLabelContent("Language");
                saveLabel.text = japaneseLocale.GetLabelContent("Save");
                titleLabel.text = japaneseLocale.GetLabelContent("Title");
                setMaxTurnsHints = japaneseLocale.GetLabelContent("MaxTurnsHints");
                setLanguageHints = japaneseLocale.GetLabelContent("LanguageHints");
                saveHints = japaneseLocale.GetLabelContent("SaveHints");
                titleHints = japaneseLocale.GetLabelContent("TitleHints");
                settingHints.text = setLanguageHints;
            }
            else if(language == "English")
            {
                GameObject.Find("LanguageEnglishLabel").GetComponent<Image>().color = Color.black;
                GameObject.Find("LanguageEnglishText").GetComponent<Text>().color = Color.white;
                GameObject.Find("LanguageJapaneseLabel").GetComponent<Image>().color = Color.white;
                GameObject.Find("LanguageJapaneseText").GetComponent<Text>().color = Color.black;
                maxTurnLabel.text = englishLocale.GetLabelContent("MaxTurns");
                langaugeLabel.text = englishLocale.GetLabelContent("Language");
                saveLabel.text = englishLocale.GetLabelContent("Save");
                titleLabel.text = englishLocale.GetLabelContent("Title");
                setMaxTurnsHints = englishLocale.GetLabelContent("MaxTurnsHints");
                setLanguageHints = englishLocale.GetLabelContent("LanguageHints");
                saveHints = englishLocale.GetLabelContent("SaveHints");
                titleHints = englishLocale.GetLabelContent("TitleHints");
                settingHints.text = setLanguageHints;
            }
        }

        private void ChangeSelectOption(string input)
        {
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
            GameObject.Find("SaveLabelBackground").GetComponent<Image>().color = Color.white;
            GameObject.Find("SaveLabel").GetComponent<Text>().color = Color.black;
            GameObject.Find("TitleLabelBackground").GetComponent<Image>().color = Color.white;
            GameObject.Find("TitleLabel").GetComponent<Text>().color = Color.black;

            switch (selectedOptionIndex)
            {
                case -1:
                    DisableChangeTurnFlag();
                    DisableSetLanguageFlag();
                    GameObject.Find("TitleLabelBackground").GetComponent<Image>().color = Color.black;
                    GameObject.Find("TitleLabel").GetComponent<Text>().color = Color.white;
                    settingHints.text = titleHints;
                    break;
                case -2:
                    DisableChangeTurnFlag();
                    DisableSetLanguageFlag();
                    GameObject.Find("SaveLabelBackground").GetComponent<Image>().color = Color.black;
                    GameObject.Find("SaveLabel").GetComponent<Text>().color = Color.white;
                    settingHints.text = saveHints;
                    break;
                default:
                    GameObject.Find(optionLabels[selectedOptionIndex].name + "Background").GetComponent<Image>().color = Color.black;
                    optionLabels[selectedOptionIndex].GetComponent<Text>().color = Color.white;
                    if(optionLabels[selectedOptionIndex].name == "MaxTurnLabel")
                    {
                        EnableChangeTurnFlag();
                        settingHints.text = setMaxTurnsHints;
                    }
                    else
                    {
                        DisableChangeTurnFlag();
                    }
                    if(optionLabels[selectedOptionIndex].name == "LanguageLabel")
                    {
                        EnableSetLanguageFlag();
                        settingHints.text = setLanguageHints;
                    }
                    else
                    {
                        DisableSetLanguageFlag();
                    }
                    break;
            }
        }

        private void SaveMaxTurns()
        {
            mapInfo.SetMaxTurns(maxTurn);
        }

        private void SaveGameLanguage()
        {
            mapInfo.SetLanguage(selectedLanguage);
        }

        private void SaveChanges()
        {
            SaveMaxTurns();
            SaveGameLanguage();
            gameTitle.ReturnToMenu();
        }

        private void UnsaveChanges()
        {
            gameTitle.ReturnToMenu();
        }
    }
}