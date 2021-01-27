using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DHU2020.DGS.MiniGame.UI
{
    public class ProgressBar : MonoBehaviour
    {
        public float Timespan;
        public Image ProgressBarRound;
        public Text ProgressText;
        private float CurrentTime;
        private bool flag;
        // Start is called before the first frame update
        void Start()
        {
            if (Timespan <= 0f)
            {
                Timespan = 10f;
            }
            if (ProgressBarRound == null)
            {
                ProgressBarRound = GetComponent<Image>();
            }
            if(ProgressText == null)
            {
                ProgressText = GetComponent<Text>();
            }
            CurrentTime = Timespan;
        }

        // Update is called once per frame
        void Update()
        {
            if (flag)
            {
                TimeCount();
            }
        }

        private void TimeCount()
        {
            if (CurrentTime > 0)
            {
                CurrentTime -= Time.deltaTime;
            }else if(CurrentTime < 0)
            {
                CurrentTime = 0f;
            }
            ProgressMove();
        }

        private void ProgressMove()
        {
            float amount = Time.deltaTime / Timespan;
            ProgressBarRound.fillAmount += amount;
            ProgressText.text = CurrentTime.ToString();
        }

        private void OnEnable()
        {
            flag = true;
            if (Timespan <= 0f)
            {
                Timespan = 10f;
            }
            if (ProgressBarRound == null)
            {
                ProgressBarRound = GetComponent<Image>();
            }
            if (ProgressText == null)
            {
                ProgressText = GetComponent<Text>();
            }
            CurrentTime = Timespan;
            ProgressBarRound.fillAmount = 0f;
        }

        private void OnDisable()
        {
            flag = false;
        }
    }
}