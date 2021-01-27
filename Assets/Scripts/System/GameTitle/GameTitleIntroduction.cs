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
    public class GameTitleIntroduction : MonoBehaviour
    {
        public MapInfo mapInfo;
        public Localization localeJP, localeEN;
        public GameObject introductionCanvas, howToPlayCanvas;
        public Text pressAnyButtonText, pressAnyButtonText2, descriptionText, gameRuleDescriptionText, toGameRuleText, toIntroductionText;

        public enum Canvas
        {
            Introduction,
            HowToPlay
        }

        private Language gameLangauge;
        private Canvas canvas;

        // Start is called before the first frame update
        void Start()
        {
            Initialization();
        }

        private void Initialization()
        {
            gameLangauge = mapInfo.GetGameLanguage();
            if (gameLangauge == Language.Japanese)
            {
                descriptionText.text = localeJP.GetLabelContent("Description").Replace("_", Environment.NewLine);
                pressAnyButtonText.text = localeJP.GetLabelContent("PressAnyButton");
                toGameRuleText.text = localeJP.GetLabelContent("HowToPlay");
                toIntroductionText.text = localeJP.GetLabelContent("Introduction");
                gameRuleDescriptionText.text = localeJP.GetLabelContent("GameRule").Replace("_", Environment.NewLine);
            }
            else
            {
                descriptionText.text = localeEN.GetLabelContent("Description").Replace("_", Environment.NewLine);
                pressAnyButtonText.text = localeEN.GetLabelContent("PressAnyButton");
                toGameRuleText.text = localeEN.GetLabelContent("HowToPlay");
                toIntroductionText.text = localeEN.GetLabelContent("Introduction");
                gameRuleDescriptionText.text = localeEN.GetLabelContent("GameRule").Replace("_", Environment.NewLine);
            }
            introductionCanvas.SetActive(true);
            howToPlayCanvas.SetActive(false);
            canvas = Canvas.Introduction;
        }

        public void ActivateCanvas(Canvas canvas)
        {
            if(canvas == Canvas.Introduction)
            {
                introductionCanvas.SetActive(true);
                howToPlayCanvas.SetActive(false);
            }
            else if(canvas == Canvas.HowToPlay)
            {
                introductionCanvas.SetActive(false);
                howToPlayCanvas.SetActive(true);
            }
        }
    }
}
