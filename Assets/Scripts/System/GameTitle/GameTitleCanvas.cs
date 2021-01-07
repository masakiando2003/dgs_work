using DHU2020.DGS.MiniGame.Map;
using DHU2020.DGS.MiniGame.Setting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DHU2020.DGS.MiniGame.Map.MapInfo;

public class GameTitleCanvas : MonoBehaviour
{
    public MapInfo mapInfo;
    public Localization japaneseLocale, englishLocale;
    public Text localGameText, networkGameText, optionText, gameTitleText;

    // Start is called before the first frame update
    void Start()
    {
        Language gameLanguage = mapInfo.GetGameLanguage();
        if(gameLanguage == Language.Japanese)
        {
            string gameTitle1 = japaneseLocale.GetLabelContent("GameTitle1");
            string gameTitle2 = japaneseLocale.GetLabelContent("GameTitle2");
            string gameTitle = gameTitle1 + Environment.NewLine + gameTitle2;
            gameTitleText.text = gameTitle;
            localGameText.text = japaneseLocale.GetLabelContent("LocalGame");
            networkGameText.text = japaneseLocale.GetLabelContent("NetworkGame");
            optionText.text = japaneseLocale.GetLabelContent("Option");
        }
        else
        {
            string gameTitle1 = englishLocale.GetLabelContent("GameTitle1");
            string gameTitle2 = englishLocale.GetLabelContent("GameTitle2");
            string gameTitle = gameTitle1 + Environment.NewLine + gameTitle2;
            gameTitleText.text = gameTitle;
            localGameText.text = englishLocale.GetLabelContent("LocalGame");
            networkGameText.text = englishLocale.GetLabelContent("NetworkGame");
            optionText.text = englishLocale.GetLabelContent("Option");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
