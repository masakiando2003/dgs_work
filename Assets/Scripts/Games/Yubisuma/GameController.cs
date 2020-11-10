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
        public Player[] Players;

        public Player DecidePlayer;

        public int PlayerNumber;

        public int TotalCount;

        public int remainingPlayer;

        public enum State
        {
            WaitForStart,
            Wait,
            Play,
            GameEnd
        }

        public State CurrentState;

        // Start is called before the first frame update
        void Start()
        {
            PlayerNumber = 0;
            ChangeState(State.WaitForStart);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void ChangeState(State state)
        {
            CurrentState = state;
            switch (CurrentState)
            {
                case State.WaitForStart:WaitForStart();
                    break;
                case State.Play:
                    break;
                case State.Wait:
                    break;
            }
        }

        private void UpdateState(State state)
        {
            CurrentState = state;
            switch (CurrentState)
            {
                case State.WaitForStart:
                    break;
                case State.Play:
                    break;
                case State.Wait:Wait();
                    break;
                case State.GameEnd:
                    break;
            }
        }

        private void WaitForStart()
        {
            SetDecidePlayer();
            ChangeState(State.Wait);
        }

        private IEnumerator Wait()
        {
            yield return new WaitForSeconds(10f);
            UpdateState(State.Play);
        }

        private void Play()
        {
            CheckWinner();
            if(CurrentState != State.GameEnd)
            {
                UpdateState(State.Wait);
            }
        }

        private void GetTotalCount()
        {
            TotalCount = 0;
            for(int i = 0; i < Players.Length; i++)
            {
                TotalCount += Players[i].Count;
            }
        }

        private void SetDecidePlayer()
        {
            if(PlayerNumber == Players.Length - 1)
            {
                PlayerNumber = 0;
            }
            DecidePlayer = Players[PlayerNumber];
            PlayerNumber++;
        }

        public void CheckWinner()
        {
            for(int i = 0; i < Players.Length; i++)
            {
                if (Players[i].RemainingHand==0)
                {
                    UpdateState(State.GameEnd);
                    break;
                }
            }
        }
    }
}
