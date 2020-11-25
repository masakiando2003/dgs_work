using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DHU2020.DGS.MiniGame.Kenkenpa
{
    public class KenkenpaRandomStepBlock : MonoBehaviour
    {
        [SerializeField] KeyCode stepBlockKeyCode;

        public void SetStepBlockKeyCode(KeyCode keyCode)
        {
            stepBlockKeyCode = keyCode;
            ShowKeyCodeText();
        }

        public KeyCode GetStepBlockKeyCode()
        {
            return stepBlockKeyCode;
        }

        private void ShowKeyCodeText()
        {
            string stepBlockKeyCodeString = stepBlockKeyCode.ToString();
            stepBlockKeyCodeString = stepBlockKeyCodeString.Replace("Keypad", "");
            GetComponentInChildren<Text>().text = stepBlockKeyCodeString;
        }
    }
}