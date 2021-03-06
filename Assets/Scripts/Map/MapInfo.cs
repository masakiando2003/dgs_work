﻿using System.Collections;
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
        [Range(1, 99)]
        public int maxTurns = 99;
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

        public void SetMaxTurns(int turns)
        {
            maxTurns = turns;
        }

        public int GetMaxTurns()
        {
            return maxTurns;
        }

        public void SetLanguage(Language selectedLanguage)
        {
            langauge = selectedLanguage;
        }

        public Language GetGameLanguage()
        {
            return langauge;
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
    }
}
