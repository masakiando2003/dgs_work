using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DHU2020.DGS.MiniGame.Setting;
using DHU2020.DGS.MiniGame.Map;

namespace DHU2020.DGS.MiniGame.Yubisuma
{
    public class YubisumaIntroduction : MonoBehaviour
    {
        public static YubisumaIntroduction Instance
        {
            get; private set;
        }

        public MapInfo mapInfo;
        public Localization localeJP, localeEN;
        public float showStartGameTextTime = 3f, fadeStartGameTextTime = 0.5f;
        public Text ControlText, HowToPlayText;
        public GameObject YubisumaGameCanvas;
        private Localization CurrentLocale;

        private bool Flag;
        private MapInfo.Language gameLanguage;

        public enum IntroductionState
        {
            HowToPlay,
            Control,
            ReadyForStart
        }
        public IntroductionState CurrentState {
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
            if (GameController.Instance.CurrentState == GameController.State.Introduction)
            {
                if(CurrentState == IntroductionState.HowToPlay)
                {
                    HowToPlayText.gameObject.SetActive(true);
                    ControlText.gameObject.SetActive(false);
                    if (Input.anyKeyDown)
                    {
                        CurrentState = IntroductionState.Control;
                    }
                }else if(CurrentState == IntroductionState.Control)
                {
                    HowToPlayText.gameObject.SetActive(false);
                    ControlText.gameObject.SetActive(true);
                    if (Input.anyKeyDown)
                    {
                        if (Input.GetKeyDown(KeyCode.Backspace))
                        {
                            CurrentState = IntroductionState.HowToPlay;
                        }
                        else
                        {
                            CurrentState = IntroductionState.ReadyForStart;
                        }
                    }
                }
            }
        }

        private void Initialize()
        {
            gameLanguage = mapInfo.GetGameLanguage();
            if(gameLanguage == MapInfo.Language.English)
            {
                CurrentLocale = localeEN;
            }else if(gameLanguage == MapInfo.Language.Japanese)
            {
                CurrentLocale = localeJP;
            }
            HowToPlayText.text = CurrentLocale.GetLabelContent("HowToPlay") + "\n" + 
                CurrentLocale.GetLabelContent("Rule1") + "\n" + 
                CurrentLocale.GetLabelContent("Rule2") + "\n" + 
                CurrentLocale.GetLabelContent("Rule3");
            ControlText.text = CurrentLocale.GetLabelContent("Control") + "\n" +
                CurrentLocale.GetLabelContent("Keyboard") + "\n" +
                CurrentLocale.GetLabelContent("KeyboardSetting1") + "\n" +
                CurrentLocale.GetLabelContent("KeyboardSetting2") + "\n" +
                CurrentLocale.GetLabelContent("JoyStick") + "\n" +
                CurrentLocale.GetLabelContent("JoyStickSetting");
            CurrentState = IntroductionState.HowToPlay;
        }
    }
}
