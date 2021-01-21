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
        public Text pressAnyButtonText;

        private Language gameLangauge;

        // Start is called before the first frame update
        void Start()
        {
            Initialization();
        }

        private void Initialization()
        {
            gameLangauge = mapInfo.GetGameLanguage();
            if(gameLangauge == Language.Japanese)
            {
                pressAnyButtonText.text = localeJP.GetLabelContent("PressAnyButton");
            }
            else
            {
                pressAnyButtonText.text = localeEN.GetLabelContent("PressAnyButton");
            }
        }
    }
}
