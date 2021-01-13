using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DHU2020.DGS.MiniGame.System
{
    [
        CreateAssetMenu(
            fileName = "PVPPlayerInfo",
            menuName = "dgs_work/Settings/OneVSThreePlayerInfo"
        )
    ]
    public class OneVSThreePlayerInfo : ScriptableObject
    {
        public int onePlayerSidePlayerID;
        public int[] threePlayerSidePlayerIDs;

        public void SetOnePlayerSidePlayerID(int playerID)
        {
            onePlayerSidePlayerID = playerID;
        }

        public void SetThreePlayerSidePlayerIDs(int index, int playerID)
        {
            threePlayerSidePlayerIDs[index] = playerID;
        }

        public int GetOnePlayerSidePlayerID()
        {
            return onePlayerSidePlayerID;
        }

        public int GetThreePlayerSidePlayerID(int index)
        {
            return threePlayerSidePlayerIDs[index];
        }
    }
}