using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DHU2020.DGS.MiniGame.Yubisuma
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance
        {
            get;private set;
        }

        [Range(0,10)]
        public int maxTurn;

        //Player
        public GameObject[] Players;

        public string DecidePlayer;

        public int PlayerNumber;

        public int TotalCount;

        public int RemainingPlayer;

        public enum State
        {
            WaitForStart,
            Choose,
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
        }

        private void ChangeState(State state)
        {
            switch (state)
            {
                case State.WaitForStart:
                    StartStateWaitForStart();
                    break;
                case State.Check:
                    StartStateCheck();
                    break;
                case State.Choose:
                    StartStateChoose();
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
                case State.Check:
                    UpdateStateCheck();
                    break;
                case State.Choose:
                    UpdateStateChoose();
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
            //Debug.Log(CurrentState + " From WaitForStart");
        }

        public void StartStateCheck()
        {

        }


        public void StartStateChoose()
        {

        }

        public void StartStateGameEnd()
        {

        }

        public void UpdateStateWaitForStart()
        {
            ChangeState(State.Choose);
        }

        public void UpdateStateCheck()
        {
            Check();
        }

        public void UpdateStateChoose()
        {
            TurnSpan -= Time.deltaTime;
            if (TurnSpan <= 0f)
            {
                GameController.Instance.ChangeState(State.Check);
                TurnSpan = 10f;
            }
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
            GetTotalCount();
            CheckWinner();
            if (CurrentState != State.GameEnd)
            {
                SetDecidePlayer();
                GameController.Instance.ChangeState(State.Choose);
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
            }
            Debug.Log("TotalCount " + TotalCount);
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

        public void SetRemainingPlayer()
        {
            RemainingPlayer = Players.Length;
        }


    }
}
