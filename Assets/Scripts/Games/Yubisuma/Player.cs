using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DHU2020.DGS.MiniGame.Yubisuma {
    public class Player : MonoBehaviour
    {
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
        [Range(0, 2)]
        public int Hand;

        //プレイヤーに残っている手
        public int RemainingHand;


        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void Initialize()
        {
            InitializeButton(NextCountChoice, transform.position.y);
            InitializeButton(NextHandChoice, transform.position.y + 80f);
            RemainingHand = 2;
        }

        private void InitializeButton(GameObject[] buttons,float yPos)
        {
            for(int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = GameObject.Instantiate(CountButton.gameObject);
                buttons[i].transform.SetParent(transform);
                buttons[i].transform.position = new Vector2(transform.position.x + (float)i * 80,yPos);
                buttons[i].GetComponentInChildren<Text>().text = i.ToString();
            }
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
        private void Decide()
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

        public void GetNextCount(Button button)
        {
            Count = int.Parse(button.GetComponentInChildren<Text>().text.ToString());
            /*for(int i = 0; i < NextCountChoice.Length; i++)
            {
                NextCountChoice[i].GetComponentInChildren<Text>().color = Color.black;
                NextCountChoice[i].GetComponent<Image>().color = Color.white;
            }
            */
            button.GetComponentInChildren<Text>().color = Color.white;
            button.GetComponent<Image>().color = Color.blue;
        }

        public void GetNextHand(Button button)
        {
            Hand = int.Parse(button.GetComponentInChildren<Text>().text.ToString());
            /*for (int i = 0; i < NextHandChoice.Length; i++)
            {
                NextHandChoice[i].GetComponentInChildren<Text>().color = Color.black;
                NextHandChoice[i].GetComponent<Image>().color = Color.white;
            }
            */
            button.GetComponentInChildren<Text>().color = Color.white;
            button.GetComponent<Image>().color = Color.blue;
        }



        private void CheckCount()
        {
            if(GameController.Instance.TotalCount == Count)
            {
                RemainingHand--;
            }
        }

        private void Win()
        {

        }
    }
}
