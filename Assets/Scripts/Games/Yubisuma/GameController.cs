using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DHU2020.DGS.MiniGame.Setting;
using DHU2020.DGS.MiniGame.Game;
using UnityEngine.UI;


namespace DHU2020.DGS.MiniGame.Yubisuma
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance
        {
            get;private set;
        }

        public PlayerInfo playerInfo;

        public GameInfo gameInfo;

        [Range(0,10)]
        public int maxTurn;

        //Player
        public GameObject[] Players;

        public string DecidePlayer
        {
            get; private set;
        }
        public int[] PlayerIDs;

        public int[] loserPlayerIDs;

        public int PlayerNumber;

        public int TotalCount
        {
            get; private set;
        }

        public GameObject ProgressBar;

        public int RemainingPlayer;

        public enum State
        {
            WaitForStart,
            Choose,
            Calculate,
            Check,
            GameEnd
        }

        public float TurnSpan = 10f;


        public State CurrentState
        {
            get; private set;
        }

        private void Awake()
        {
            if(Instance != null)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateState();
        }

        private void Initialize()
        {
            PlayerNumber = 0;
            ChangeState(State.WaitForStart);
            Players = GameObject.FindGameObjectsWithTag("YubisumaPlayer");
            PlayerIDs = new int[Players.Length];
            for(int i = 0; i < Players.Length;i++)
            {
                Players[i].GetComponent<Player>().PlayerName.text = playerInfo.playerNames[i];
                Players[i].GetComponent<Player>().PlayerID = playerInfo.GetPlayerID(Players[i].GetComponent<Player>().PlayerName.text);
                PlayerIDs[i] = Players[i].GetComponent<Player>().PlayerID;
            }
            loserPlayerIDs = new int[Players.Length - 1];

        }

        private void ChangeState(State state)
        {
            switch (state)
            {
                case State.WaitForStart:
                    StartStateWaitForStart();
                    break;
                case State.Choose:
                    StartStateChoose();
                    break;
                case State.Calculate:
                    StartStateCalculate();
                    break;
                case State.Check:
                    StartStateCheck();
                    break;
                case State.GameEnd:
                    StartStateGameEnd();
                    break;
            }
            Debug.Log(state + " From " + CurrentState);
            CurrentState = state;
        }


        private void UpdateState()
        {
            switch (CurrentState)
            {
                case State.WaitForStart:
                    UpdateStateWaitForStart();
                    break;
                case State.Choose:
                    UpdateStateChoose();
                    break;
                case State.Calculate:
                    UpdateStateCalculate();
                    break;
                case State.Check:
                    UpdateStateCheck();
                    break;
                case State.GameEnd:
                    break;
            }
            //Debug.Log(CurrentState);
        }

        public void StartStateWaitForStart()
        {
            SetDecidePlayer();
            GameController.Instance.ChangeState(State.Choose);
            ProgressBar.SetActive(false);
            //Debug.Log(CurrentState + " From WaitForStart");
        }


        public void StartStateChoose()
        {

        }

        public void StartStateCalculate()
        {

        }

        public void StartStateCheck()
        {

        }

        public void StartStateGameEnd()
        {

        }

        public void UpdateStateWaitForStart()
        {
            ChangeState(State.Choose);
            ProgressBar.SetActive(true);
        }

        public void UpdateStateChoose()
        {
            TurnSpan -= Time.deltaTime;
            if (TurnSpan <= 0f)
            {
                GameController.Instance.ChangeState(State.Calculate);
                TurnSpan = 10f;
                ProgressBar.SetActive(false);
            }
        }

        public void UpdateStateCalculate()
        {
            GetTotalCount();
            ChangeState(State.Check);
        }

        public void UpdateStateCheck()
        {
            Check();
        }



        public void UpdateStateGameEnd()
        {

        }

        
        /*public IEnumerator ChooseRoutine()
        {
            yield return new WaitForSeconds(10f);
                GameController.Instance.ChangeState(State.Check);
        }
        */

        public void Check()
        {
            CheckWinner();
            if (CurrentState != State.GameEnd)
            {
                SetDecidePlayer();
                GameController.Instance.ChangeState(State.Choose);
                ProgressBar.SetActive(true);
            }
        }

        public void CheckWinner()
        {
            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i].GetComponent<Player>().RemainingHand == 0)
                {
                    GameController.Instance.ChangeState(State.GameEnd);
                    Debug.Log(DecidePlayer + "Win");
                    SetLoserPlayers(Players[i].GetComponent<Player>().PlayerID);
                    SetWinner(Players[i].GetComponent<Player>().PlayerID);
                    break;
                }
            }
        }

        public void GetTotalCount()
        {
            TotalCount = 0;
            for(int i = 0; i < Players.Length; i++)
            {
                TotalCount += Players[i].GetComponent<Player>().Hand;
                Debug.Log("TotalCount " + TotalCount);
            }
            
        }

        private void SetDecidePlayer()
        {
            if(PlayerNumber == Players.Length)
            {
                PlayerNumber = 0;
            }
            DecidePlayer = Players[PlayerNumber].name;
            Debug.Log(DecidePlayer);
            PlayerNumber++;
        }

        public void SetLoserPlayers(int Winner)
        {
            for(int i = 0; i < Players.Length; i++)
            {
                if (i != Winner)
                {
                    loserPlayerIDs[i] = PlayerIDs[i];
                }
            }
        }

        IEnumerator SetWinner(int playerID)
        {
            yield return new WaitForSeconds(3f);
            gameInfo.SetMiniGameWinner("Yubisuma", playerID, loserPlayerIDs);
        }

        public void SetRemainingPlayer()
        {
            RemainingPlayer = Players.Length;
        }


    }
}
