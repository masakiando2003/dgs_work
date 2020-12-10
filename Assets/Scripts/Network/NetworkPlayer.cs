using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace DHU2020.DGS.MiniGame.Network
{
    public class NetworkPlayer : MonoBehaviour
    {
        public GameObject Player;
        public GameObject PlayerStatusCanvas;
        //public Text PlayersInRoom;
        // Start is called before the first frame update
        void Start()
        {
            Player = PhotonNetwork.Instantiate(Player.name, PlayerStatusCanvas.transform.position, Quaternion.identity);
            Player.transform.SetParent(PlayerStatusCanvas.transform);
            Player.transform.localScale = new Vector3(1, 1, 1);
            PhotonNetwork.NickName = Player.name;
        }

        // Update is called once per frame
        void Update()
        {
            //PlayersInRoom.text = "Players -> " + PhotonNetwork.CountOfPlayers;
        }
    }
}