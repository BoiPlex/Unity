using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    //public Text stageSelectedText;
    //private string stageText;

    private string stagePicked = "Stage1";

    public void SetStage(string stage)
    {
        stagePicked = stage;
    }

    public string GetStage()
    {
        return stagePicked;
    }

    /*
    public void SetText(string text)
    {
        stageText = text;
    }

    public void UpdateText()
    {
        stageSelectedText.text = stageText;
    }
    */
}
