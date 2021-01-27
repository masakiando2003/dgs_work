using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DHU2020.DGS.MiniGame.Setting
{
    [
    CreateAssetMenu(
        fileName = "PlayerInputInfo",
        menuName = "dgs_work/Settings/PlayerInputInfo"
    )
]
    public class PlayerInputInfo : ScriptableObject
    {
        public KeyCode Player1HitButton1=KeyCode.A, Player1HitButton2=KeyCode.W, Player1HitButton3=KeyCode.S, Player1HitButton4=KeyCode.D;
        public KeyCode Player2HitButton1=KeyCode.F, Player2HitButton2=KeyCode.T, Player2HitButton3=KeyCode.G, Player2HitButton4=KeyCode.H;
        public KeyCode Player3HitButton1=KeyCode.J, Player3HitButton2=KeyCode.I, Player3HitButton3=KeyCode.K, Player3HitButton4=KeyCode.L;
        public KeyCode Player4HitButton1=KeyCode.Alpha4, Player4HitButton2=KeyCode.Alpha5, Player4HitButton3=KeyCode.Alpha6, Player4HitButton4=KeyCode.Alpha8;
        
        public List<KeyCode> PlayerKeyCodes = new List<KeyCode>();



        public List<KeyCode> GetPlayerKeyCodes(int PlayerID)
        {
            PlayerKeyCodes.Clear();
            switch (PlayerID)
            {
                case 0:
                    PlayerKeyCodes.Add(Player1HitButton1);
                    PlayerKeyCodes.Add(Player1HitButton2);
                    PlayerKeyCodes.Add(Player1HitButton3);
                    PlayerKeyCodes.Add(Player1HitButton4);
                    break;
                case 1:
                    PlayerKeyCodes.Add(Player2HitButton1);
                    PlayerKeyCodes.Add(Player2HitButton2);
                    PlayerKeyCodes.Add(Player2HitButton3);
                    PlayerKeyCodes.Add(Player2HitButton4);
                    break;
                case 2:
                    PlayerKeyCodes.Add(Player3HitButton1);
                    PlayerKeyCodes.Add(Player3HitButton2);
                    PlayerKeyCodes.Add(Player3HitButton3);
                    PlayerKeyCodes.Add(Player3HitButton4);
                    break;
                case 3:
                    PlayerKeyCodes.Add(Player4HitButton1);
                    PlayerKeyCodes.Add(Player4HitButton2);
                    PlayerKeyCodes.Add(Player4HitButton3);
                    PlayerKeyCodes.Add(Player4HitButton4);
                    break;
            }
            return PlayerKeyCodes;
        }
    }
}