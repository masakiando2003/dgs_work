using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DHU2020.DGS.MiniGame.Setting;
using System;

namespace DHU2020.DGS.MiniGame.Yubisuma {
    public class Player : MonoBehaviour
    {
        public Canvas CountCanvas,HandCanvas;
        //カウント用のボタン
        public Button CountButton;
        //手のボタン
        public Button HandButton;
        //ゆびすまを言う次のプレイヤーが数字を選ぶためのボタン 0~remainingPlayer*2
        public GameObject[] NextCountChoice;

        //各プレイヤーが0~2を選ぶためのボタン
        public GameObject[] NextHandChoice;

        //NextCountChoice用のint
        public int Count;
        //NextHandChoice用のint 
        public int Hand;

        //プレイヤーに残っている手
        public int RemainingHand;

        public Sprite CurledUpHand,ThumbsUpHand;

        public Text PlayerName;

        public Image HandLeft, HandRight;

        public int PlayerID;

        private float CheckTime = 1f;

        public PlayerInfo playerInfo;

        private PlayerInfo.PlayerControllerInput playerControllerInput;

        public KeyCode HandPlus, HandMinus,CountPlus,CountMinus,CurrentInput;
        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            switch (GameController.Instance.CurrentState)
            {
                case GameController.State.WaitForStart:
                    DecideNextChoice();
                    break;
                case GameController.State.Choose:
                    CheckInput(PlayerID);
                    NextTurn();
                    ResetHand();
                    SwitchHandImage();
                    break;
                case GameController.State.Calculate:
                    DecideNextChoice();
                    break;
                case GameController.State.Check:
                    CheckCount();
                    break;
                case GameController.State.GameEnd:
                    break;
            }
        }

        private void Initialize()
        {
            InitializeButton(NextCountChoice, transform.position.y,CountButton,CountCanvas);
            InitializeButton(NextHandChoice, transform.position.y + 80f,HandButton,HandCanvas);
            RemainingHand = 2;
            HandRight.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            if(playerInfo == null)
            {
                playerInfo = GameController.Instance.playerInfo;
            }
        }

        private void InitializeButton(GameObject[] buttons,float yPos,Button button,Canvas canvas)
        {
            for(int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = GameObject.Instantiate(button.gameObject);
                buttons[i].transform.SetParent(canvas.transform);
                buttons[i].GetComponentInChildren<Text>().text = i.ToString();
            }
        }

        private void InitializeImage()
        {

        }

        /*private void DecreaseHand()
        {
            RemainingHand--;
            if (RemainingHand == 0)
            {
                Win();
            }
        }
        */
        private void SwitchHandImage()
        {
            switch (Hand)
            {
                case 0:
                    SetHandImage(CurledUpHand, CurledUpHand);
                    break;
                case 1:
                    SetHandImage(CurledUpHand, ThumbsUpHand);
                    break;
                case 2:
                    SetHandImage(ThumbsUpHand, ThumbsUpHand);
                    break;
            }
        }

        private void SetHandImage(Sprite Left,Sprite Right)
        {
            HandLeft.sprite = Left;
            HandRight.sprite = Right;
        }

        private void DecideNextChoice()
        {
            for(int i = 0; i < NextCountChoice.Length; i++)
            {
                NextCountChoice[i].SetActive(false);
            }
            for (int i = 0; i < NextHandChoice.Length; i++)
            {
                NextHandChoice[i].SetActive(false);
            }
        }

        private void NextTurn()
        {
            if (GameController.Instance.DecidePlayer == this.name)
            {
                for (int i = 0; i < NextCountChoice.Length; i++)
                {
                    NextCountChoice[i].SetActive(true);
                }
            }
            else
            {
                for (int i = 0; i < NextCountChoice.Length; i++)
                {
                    NextCountChoice[i].SetActive(false);
                }
            }
            for (int i = 0; i <= RemainingHand; i++)
            {
                NextHandChoice[i].SetActive(true);
            }
        }

        private void ResetHand()
        {
            if(Hand > RemainingHand)
            {
                Hand = RemainingHand;
            }
        }
        private void CheckInputMethod()
        {
            playerControllerInput = playerInfo.GetPlayerControllerInput(PlayerID);
        }
        private void CheckInput(int PlayerID)
        {
            CheckInputMethod();
            NextHandChoice[Hand].GetComponent<Image>().color = Color.blue;
            NextCountChoice[Count].GetComponent<Image>().color = Color.blue;
            if (playerControllerInput == PlayerInfo.PlayerControllerInput.Keyboard)
            {
                HandKeyboardInput(PlayerID);
                CountKeyboardInput(PlayerID);
                if (Input.anyKeyDown)
                {
                    foreach(KeyCode code in Enum.GetValues(typeof(KeyCode))){
                        if (Input.GetKeyDown(code))
                        {
                            CurrentInput = code;
                            break;
                        }
                    }
                    HandChange(CurrentInput);
                    CountChange(CurrentInput);
                }
            }
        }


        private void HandKeyboardInput(int PlayerID)
        {
                switch (PlayerID)
                {
                    case 0:
                        HandPlus = KeyCode.A;
                        HandMinus = KeyCode.Q;
                        break;
                    case 1:
                        HandPlus = KeyCode.W;
                        HandMinus = KeyCode.S;
                        break;
                    case 2:
                        HandPlus = KeyCode.E;
                        HandMinus = KeyCode.D;
                        break;
                    case 3:
                        HandPlus = KeyCode.R;
                        HandMinus = KeyCode.F;
                        break;
                }
        }

        private void CountKeyboardInput(int PlayerID)
        {
            switch (PlayerID)
            {
                case 0:
                    CountPlus = KeyCode.T;
                    CountMinus = KeyCode.G;
                    break;
                case 1:
                    CountPlus = KeyCode.Y;
                    CountMinus = KeyCode.H;
                    break;
                case 2:
                    CountPlus = KeyCode.U;
                    CountMinus = KeyCode.J;
                    break;
                case 3:
                    CountPlus = KeyCode.I;
                    CountMinus = KeyCode.K;
                    break;
            }
        }

        private void HandChange(KeyCode CurrentInput)
        {
            if (CurrentInput == HandPlus)
            {
                if (Hand < RemainingHand)
                {
                    Hand++;
                    NextHandChoice[Hand].GetComponent<Image>().color = Color.blue;
                    NextHandChoice[Hand - 1].GetComponent<Image>().color = Color.white;
                }
            }else if(CurrentInput == HandMinus)
            {
                if(Hand > 0)
                {
                    Hand--;
                    NextHandChoice[Hand].GetComponent<Image>().color = Color.blue;
                    NextHandChoice[Hand + 1].GetComponent<Image>().color = Color.white;
                }
            }
        }
        private void CountChange(KeyCode CurrentInput)
        {
            if (CurrentInput == CountPlus)
            {
                if (Count < 8)
                {
                    Count++;
                    NextCountChoice[Count].GetComponent<Image>().color = Color.blue;
                    NextCountChoice[Count - 1].GetComponent<Image>().color = Color.white;
                }
            }
            else if (CurrentInput == CountMinus)
            {
                if (Count > 0)
                {
                    Count--;
                    NextCountChoice[Count].GetComponent<Image>().color = Color.blue;
                    NextCountChoice[Count + 1].GetComponent<Image>().color = Color.white;
                }
            }
        }
        private void CheckCount()
        {
                if (GameController.Instance.DecidePlayer == this.name)
                {
                    Debug.Log("RemainigHand " + RemainingHand + " / Count " + Count + " / TotalCount " + GameController.Instance.TotalCount + " / Hand " + Hand);
                }
                if (GameController.Instance.TotalCount == Count && RemainingHand >= 0 && GameController.Instance.DecidePlayer == this.name)
                {
                    RemainingHand--;
                    Debug.Log("RemainigHand " + RemainingHand + " / Count " + Count + " / TotalCount" + GameController.Instance.TotalCount + " / Hand" + Hand);

                }
        }

    }
}
