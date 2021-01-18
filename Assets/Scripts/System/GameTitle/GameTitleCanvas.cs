using DHU2020.DGS.MiniGame.Map;
using DHU2020.DGS.MiniGame.Setting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DHU2020.DGS.MiniGame.Map.MapInfo;

namespace DHU2020.DGS.MiniGame.System
{
    public class GameTitleCanvas : MonoBehaviour
    {
        public MapInfo mapInfo;
        public Localization localeJP, localeEN;
        public Text newGameText, creditText, optionText, gameTitleText, gameTitleControlHintsText;

        // Start is called before the first frame update
        void Start()
        {
            Language gameLanguage = mapInfo.GetGameLanguage();
            if (gameLanguage == Language.Japanese)
            {
                string gameTitle1 = localeJP.GetLabelContent("GameTitle1");
                string gameTitle2 = localeJP.GetLabelContent("GameTitle2");
                string gameTitle = gameTitle1 + Environment.NewLine + gameTitle2;
                gameTitleText.text = gameTitle;
                newGameText.text = localeJP.GetLabelContent("NewGame");
                optionText.text = localeJP.GetLabelContent("Option");
                creditText.text = localeJP.GetLabelContent("Credit");
                gameTitleControlHintsText.text = localeJP.GetLabelContent("ControlHints");
            }
            else
            {
                string gameTitle1 = localeEN.GetLabelContent("GameTitle1");
                string gameTitle2 = localeEN.GetLabelContent("GameTitle2");
                string gameTitle = gameTitle1 + Environment.NewLine + gameTitle2;
                gameTitleText.text = gameTitle;
                newGameText.text = localeEN.GetLabelContent("NewGame");
                optionText.text = localeEN.GetLabelContent("Option");
                creditText.text = localeEN.GetLabelContent("Credit");
                gameTitleControlHintsText.text = localeEN.GetLabelContent("ControlHints");
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}