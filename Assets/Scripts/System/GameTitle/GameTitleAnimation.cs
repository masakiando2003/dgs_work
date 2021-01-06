using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DHU2020.DGS.MiniGame.System
{
    public class GameTitleAnimation : MonoBehaviour
    {
        public GameTitle gameTitle;

        public void ActivateGameOptionPanel()
        {
            gameTitle.EnableGameTitlePanel();
        }
    }
}