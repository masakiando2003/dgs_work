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
            Player.transform.localScale = new Vector3(1,1,1);
            //SetPosition();
            PhotonNetwork.NickName = Player.name;
        }

        // Update is called once per frame
        void Update()
        {
            //PlayersInRoom.text = "Players -> " + PhotonNetwork.CountOfPlayers;
        }

        public void SetPosition()
        {
            for(int i = 0; i < PhotonNetwork.CountOfPlayers; i++)
            {
                if(PhotonNetwork.PlayerList[i] == PhotonNetwork.LocalPlayer)
                {
                    Player.transform.position = new Vector3(PlayerStatusCanvas.transform.position.x / PhotonNetwork.CountOfPlayers * i, PlayerStatusCanvas.transform.position.y, PlayerStatusCanvas.transform.position.z);
                }
            }
        }
    }
}