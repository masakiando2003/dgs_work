using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DHU2020.DGS.MiniGame.System
{
    [
        CreateAssetMenu(
            fileName = "PVPPlayerInfo",
            menuName = "dgs_work/Settings/PVPPlayerInfo"
        )
    ]
    public class PVPPlayerInfo : ScriptableObject
    {
        public int player1ID;
        public int player2ID;

        public void SetPlayerID(int firstPlayerID, int secondPlayerID)
        {
            player1ID = firstPlayerID;
            player2ID = secondPlayerID;
        }

        public int GetPlayer1ID()
        {
            return player1ID;
        }

        public int GetPlayer2ID()
        {
            return player2ID;
        }
    }
}