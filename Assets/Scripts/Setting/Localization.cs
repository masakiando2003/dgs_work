using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DHU2020.DGS.MiniGame.Map.MapInfo;

namespace DHU2020.DGS.MiniGame.Setting
{
    [
        CreateAssetMenu(
            fileName = "MapInfo",
            menuName = "dgs_work/Settings/Localization"
        )
    ]
    public class Localization : ScriptableObject
    {
        public List<string> labelName, labelText;

        public string GetLabelContent(string specifiedLabel)
        {
            int labelIndex = labelName.FindIndex(n => n == specifiedLabel);
            return labelText[labelIndex];
        }
    }
}