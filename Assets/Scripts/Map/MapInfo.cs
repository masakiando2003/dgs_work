using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DHU2020.DGS.MiniGame.Map
{
    [
        CreateAssetMenu(
            fileName = "MapInfo",
            menuName = "dgs_work/Settings/MapInfo"
        )
    ]
    public class MapInfo : ScriptableObject
    {
        public int minTurns = 10;
        [Range(10, 25)]
        public int maxTurns = 25;
        [SerializeField] int currentTurn = 0;
        public float loadMainMapSeconds;
        public enum Language
        {
            Japanese,
            English
        }
        public Language langauge;

        public int GetCurrentTurn()
        {
            return currentTurn;
        }

        public int GetMinTurns()
        {
            return minTurns;
        }

        public void SetMaxTurns(int turns)
        {
            maxTurns = turns;
            PlayerPrefs.SetInt("MaxTurns", maxTurns);
        }

        public int GetMaxTurns()
        {
            int maxTurns = PlayerPrefs.GetInt("MaxTurns");
            return maxTurns;
        }

        public void SetLanguage(Language selectedLanguage)
        {
            langauge = selectedLanguage;
            PlayerPrefs.SetString("GameLanguage", selectedLanguage.ToString());
        }

        public Language GetGameLanguage()
        {
            string gameLanguage = PlayerPrefs.GetString("GameLanguage");
            if(gameLanguage == "Japanese")
            {
                return Language.Japanese;
            }
            else if (gameLanguage == "English")
            {
                return Language.English;
            }
            else
            {
                return langauge;
            }
            //return langauge;
        }

        public void StartNewGame()
        {
            currentTurn = 0;
        }

        public void ProceedNextTurn()
        {
            currentTurn++;
            LoadMainMap();
        }

        void LoadMainMap()
        {
            SceneManager.LoadScene("MainMap");
        }

        public float GetLoadMapSeconds()
        {
            return loadMainMapSeconds;
        }

        public void SetDebugMode(bool isDebug)
        {
            PlayerPrefs.SetInt("debugMode", (isDebug == true) ? 1 : 0);
        }

        public bool GetDebugMode()
        {
            int debugMode = PlayerPrefs.GetInt("debugMode");
            return debugMode == 1 ? true : false;
        }
    }
}
