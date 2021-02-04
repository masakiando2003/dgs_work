using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DHU2020.DGS.MiniGame.Setting;
using DHU2020.DGS.MiniGame.Game;
using DHU2020.DGS.MiniGame.Map;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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
            Introduction,
            Prepare,
            Choose,
            Calculate,
            Check,
            GameEnd
        }

        public float TurnSpan = 10f;

        private float span;


        public bool IntroductionFlag;

        public Canvas IntroductionCanvas,GameCanvas;

        public Text WinnerText;

        private int WinnerID;

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
            ChangeState(State.Introduction);
            Players = GameObject.FindGameObjectsWithTag("YubisumaPlayer");
            PlayerIDs = new int[Players.Length];
            for(int i = 0; i < Players.Length;i++)
            {
                Players[i].GetComponent<Player>().PlayerName.text = playerInfo.playerNames[i];
                Players[i].GetComponent<Player>().PlayerID = playerInfo.GetPlayerID(Players[i].GetComponent<Player>().PlayerName.text);
                PlayerIDs[i] = Players[i].GetComponent<Player>().PlayerID;
            }
            loserPlayerIDs = new int[Players.Length - 1];
            span = TurnSpan;
            WinnerText.gameObject.SetActive(false);
            WinnerID = 100;
        }

        private void ChangeState(State state)
        {
            switch (state)
            {
                case State.Introduction:
                    StartStateIntroduction();
                    break;
                case State.Prepare:
                    StartStatePrepare();
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
                case State.Introduction:
                    UpdateStateIntroduction();
                    break;
                case State.Prepare:
                    UpdateStatePrepare();
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
                    UpdateStateGameEnd();
                    break;
            }
            //Debug.Log(CurrentState);
        }

        public void StartStateIntroduction()
        {
            IntroductionFlag = false;
        }

        public void StartStatePrepare()
        {
            SetDecidePlayer();
            GameController.Instance.ChangeState(State.Choose);
            ProgressBar.SetActive(false);
            //Debug.Log(CurrentState + " From Prepare");
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

        public void UpdateStateIntroduction()
        {
            if(YubisumaIntroduction.Instance.CurrentState == YubisumaIntroduction.IntroductionState.ReadyForStart)
            {
                IntroductionCanvas.gameObject.SetActive(false);
                GameCanvas.gameObject.SetActive(true);
            }
            else
            {
                IntroductionCanvas.gameObject.SetActive(true);
                GameCanvas.gameObject.SetActive(false);
            }
            if (GameCanvas.isActiveAndEnabled)
            {
                ChangeState(State.Prepare);
            }
        }

        public void UpdateStatePrepare()
        {
            ProgressBar.SetActive(true);
            ChangeState(State.Choose);
        }

        public void UpdateStateChoose()
        {
            span -= Time.deltaTime;
            if (span <= 0f)
            {
                GameController.Instance.ChangeState(State.Calculate);
                span = TurnSpan;
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
            WinnerText.gameObject.SetActive(true);
            WinnerText.text = playerInfo.GetPlayerName(WinnerID) + " Win";
            SceneManager.LoadScene("MainMap");
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
                    Debug.Log(DecidePlayer + " Win / PlayerID: " + Players[i].GetComponent<Player>().PlayerID);
                    SetLoserPlayers(Players[i].GetComponent<Player>().PlayerID);
                    WinnerID = Players[i].GetComponent<Player>().PlayerID;
                    SetWinner(WinnerID);
                    GameController.Instance.ChangeState(State.GameEnd);
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
            int j = 0;
            for (int i = 0; i < Players.Length; i++)
            {
                if (i != Winner)
                {
                    loserPlayerIDs[j] = PlayerIDs[i];
                    j++;
                }
            }
        }

        IEnumerator SetWinner(int playerID)
        {
            yield return new WaitForSeconds(3f);
            gameInfo.SetMiniGameWinner("Yubisuma", playerID, loserPlayerIDs);
            Debug.Log("Winner Selected");
        }

        public void SetRemainingPlayer()
        {
            RemainingPlayer = Players.Length;
        }

        /*
           private void InputCheck()
        {
            if (Input.GetKeyDown("joystick button 0"))
            {
                Debug.Log("button0");
            }
            if (Input.GetKeyDown("joystick button 1"))
            {
                Debug.Log("button1");
            }
            if (Input.GetKeyDown("joystick button 2"))
            {
                Debug.Log("button2");
            }
            if (Input.GetKeyDown("joystick button 3"))
            {
                Debug.Log("button3");
            }
            if (Input.GetKeyDown("joystick button 4"))
            {
                Debug.Log("button4");
            }
            if (Input.GetKeyDown("joystick button 5"))
            {
                Debug.Log("button5");
            }
            if (Input.GetKeyDown("joystick button 6"))
            {
                Debug.Log("button6");
            }
            if (Input.GetKeyDown("joystick button 7"))
            {
                Debug.Log("button7");
            }
            if (Input.GetKeyDown("joystick button 8"))
            {
                Debug.Log("button8");
            }
            if (Input.GetKeyDown("joystick button 9"))
            {
                Debug.Log("button9");
            }
            float hori = Input.GetAxis("Horizontal");
            float vert = Input.GetAxis("Vertical");
            if ((hori != 0) || (vert != 0))
            {
                Debug.Log("stick:" + hori + "," + vert);
            }
        }
        */
    }
}
