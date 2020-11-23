using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DHU2020.DGS.MiniGame.Kenkenpa.KenkenpaGameController;

namespace DHU2020.DGS.MiniGame.Kenkenpa
{
    public class KenkenpaPlayerController : MonoBehaviour
    {
        public KenkenpaGameController kenkenpaGameController;
        public KeyCode hitKeyCode1, hitKeyCode2, hitKeyCode3, hitKeyCode4;
        
        // Start is called before the first frame update
        void Start()
        {
            GameState currentGameState = kenkenpaGameController.GetCurrentGameState();

            if (currentGameState != GameState.GameStart) { return; }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}