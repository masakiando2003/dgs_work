using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DHU2020.DGS.MiniGame.Yubisuma {
    public class Player : MonoBehaviour
    {
        public Canvas CountCanvas,HandCanvas,SpriteCanvas;
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
        }

        private void InitializeButton(GameObject[] buttons,float yPos,Button button,Canvas canvas)
        {
            for(int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = GameObject.Instantiate(button.gameObject);
                buttons[i].transform.SetParent(canvas.transform);
                //buttons[i].transform.position = new Vector2(transform.position.x + (float)i * 30,yPos);
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
                    HandChange(CurledUpHand, CurledUpHand);
                    break;
                case 1:
                    HandChange(CurledUpHand, ThumbsUpHand);
                    break;
                case 2:
                    HandChange(ThumbsUpHand, ThumbsUpHand);
                    break;
            }
        }

        private void HandChange(Sprite Left,Sprite Right)
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

        private void Win()
        {

        }
    }
}
