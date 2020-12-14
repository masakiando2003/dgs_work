using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;


namespace DHU2020.DGS.MiniGame.Network
{
    public class NetworkController : MonoBehaviourPunCallbacks
    {
        public string PlayerName = "PlayerName";

        private string RoomName = "room";

        [SerializeField]
        private byte MaxPlayersPerRoom = 4;

        private bool isConnecting;

        public Button StartButton;

        public Text CurrentPlayersInRoom;

        public bool MoveToRoom
        {
            get; private set;
        }

        public PhotonView PhotonView;

        private RoomOptions roomOptions;

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            Connect();
            MoveToRoom = false;
        }

        // Update is called once per frame
        void Update()
        {
            //SwitchButton();
            CurrentPlayersInRoom.text = "Players In Room = " + PhotonNetwork.CountOfPlayers;
            CheckMoveToRoom();
            SwitchButton();
        }

        public void Connect()
        {
            isConnecting = true;
            if (PhotonNetwork.IsConnected)
            {
                JoinRoom();
                Debug.Log("Connected");
                PhotonNetwork.NickName = PlayerName;
            }
            else
            {
                Debug.Log("Connecting");
                PhotonNetwork.GameVersion = "1";
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public void JoinRoom()
        {
            if (isConnecting)
            {
                Debug.Log("Join Room.....");

                roomOptions = new RoomOptions();
                roomOptions.MaxPlayers = MaxPlayersPerRoom;

                PhotonNetwork.JoinOrCreateRoom(RoomName, roomOptions, TypedLobby.Default);
                if(PhotonNetwork.MasterClient == null || PhotonNetwork.PlayerListOthers.Length == 0)
                {
                    PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
                }
            }
        }

        public override void OnConnectedToMaster()
        {
            JoinRoom();
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRoomFailed");
            Debug.Log("Join Random Room");
            PhotonNetwork.JoinRandomRoom();
        }
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRandomFailed");
            string roomName = "room_" + PlayerName;
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = MaxPlayersPerRoom;

            PhotonNetwork.CreateRoom(roomName, roomOptions);
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            if (PhotonNetwork.MasterClient == PhotonNetwork.LocalPlayer)
            {
                PhotonNetwork.SetMasterClient(PhotonNetwork.PlayerListOthers[0]);
            }
            Debug.Log(cause);

            isConnecting = false;
        }
        public override void OnJoinedRoom()
        {
            Debug.Log("Join Room Success !" + PhotonNetwork.CurrentRoom);

            //OnJoinRoomEvent.Invoke();
        }
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log("Player Entered: " + newPlayer.NickName);
        }
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log("Player Left: " + otherPlayer.NickName);
        }

        private void SwitchButton()
        {
            if (PhotonNetwork.MasterClient == PhotonNetwork.LocalPlayer)
            {
                StartButton.gameObject.SetActive(true);
            }
            else
            {
                StartButton.gameObject.SetActive(false);
            }
        }

        public void StartNetworkGame()
        {
            this.MoveToRoom = true;
        }

        public void CheckMoveToRoom()
        {
            if (MoveToRoom)
            {
                PhotonNetwork.LoadLevel("NetworkGameMainMap");
            }
        }

        /*public void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(MoveToRoom);
            }else if (stream.IsReading)
            {
                MoveToRoom = (bool)stream.ReceiveNext();
            }
        }*/
    }
}
