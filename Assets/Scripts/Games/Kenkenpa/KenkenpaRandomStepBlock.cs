using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DHU2020.DGS.MiniGame.Setting.PlayerInfo;

namespace DHU2020.DGS.MiniGame.Kenkenpa
{
    public class KenkenpaRandomStepBlock : MonoBehaviour
    {
        public Text stepBlockText;
        public Image stepBlockCircleImage, stepBlockSquareImage, stepBlockCrossImage, stepBlockTriangleImage;
        private KeyCode stepBlockKeyCode;

        public void SetStepBlockKeyCode(KeyCode keyCode, int keyCodeIndex = -1, PlayerControllerInput playerInput = PlayerControllerInput.Keyboard)
        {
            stepBlockKeyCode = keyCode;
            if(playerInput == PlayerControllerInput.Keyboard)
            {
                ShowKeyCodeText(keyCode);
            }
            else
            {
                ShowKeyCodeImage(keyCodeIndex);
            }
        }

        public KeyCode GetStepBlockKeyCode()
        {
            return stepBlockKeyCode;
        }

        private void ShowKeyCodeText(KeyCode keyCode)
        {
            stepBlockCircleImage.enabled = false;
            stepBlockSquareImage.enabled = false;
            stepBlockCrossImage.enabled = false;
            stepBlockTriangleImage.enabled = false;
            string stepBlockKeyCodeString = keyCode.ToString();
            stepBlockKeyCodeString = stepBlockKeyCodeString.Replace("Keypad", "");
            stepBlockText.text = stepBlockKeyCodeString;
        }

        private void ShowKeyCodeImage(int keyCodeIndex)
        {
            stepBlockText.enabled = false;
            stepBlockCircleImage.enabled = false;
            stepBlockSquareImage.enabled = false;
            stepBlockCrossImage.enabled = false;
            stepBlockTriangleImage.enabled = false;
            switch (keyCodeIndex)
            {
                case 0:
                    stepBlockSquareImage.enabled = true;
                    break;
                case 1:
                    stepBlockCrossImage.enabled = true;
                    break;
                case 2:
                    stepBlockCircleImage.enabled = true;
                    break;
                case 3:
                    stepBlockTriangleImage.enabled = true;
                    break;
            }
        }
    }
}