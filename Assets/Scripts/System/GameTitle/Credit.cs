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
    public class Credit : MonoBehaviour
    {
        public MapInfo mapInfo;
        public Localization localeJP, localeEN;
        public Text teamNameText, planner_GraphicDesignerTitleText, prorammerTitleText, controlHintText;

        private Language gameLanguage;

        // Start is called before the first frame update
        void Start()
        {
            gameLanguage = mapInfo.GetGameLanguage();
            if(gameLanguage == Language.Japanese)
            {
                teamNameText.text = localeJP.GetLabelContent("TeamName") + Environment.NewLine + "("+ localeJP.GetLabelContent("UniversityName") + ")";
                planner_GraphicDesignerTitleText.text = localeJP.GetLabelContent("Planner_GraphicDesigner") + ":";
                prorammerTitleText.text = localeJP.GetLabelContent("Programmer") + ":";
                controlHintText.text = localeJP.GetLabelContent("ControlHint");
            }
            else
            {
                teamNameText.text = localeEN.GetLabelContent("TeamName") + Environment.NewLine + "(" + localeEN.GetLabelContent("UniversityName") + ")";
                planner_GraphicDesignerTitleText.text = localeEN.GetLabelContent("Planner_GraphicDesigner") + ":";
                prorammerTitleText.text = localeEN.GetLabelContent("Programmer") + ":";
                controlHintText.text = localeEN.GetLabelContent("ControlHint");
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}