using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string stage1;
    public string stage2;
    public string stage3;
    public string stage4;

    private StageManager stageManager;

    void Awake()
    {
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(stageManager.GetStage());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MenuScreen");
        //stageManager.UpdateText();
    }

    public void CustomizeGame()
    {
        SceneManager.LoadScene("CustomizationScreen");
    }

    public void StageSelect()
    {
        SceneManager.LoadScene("StageSelectScreen");
    }

    public void PickStage1()
    {
        stageManager.SetStage(stage1);
        //stageManager.SetText("Stage Selected: " + stage1);
        MainMenu();
    }

    public void PickStage2()
    {
        stageManager.SetStage(stage2);
        //stageManager.SetText("Stage Selected: " + stage2);
        MainMenu();
    }

    public void PickStage3()
    {
        stageManager.SetStage(stage3);
        //stageManager.SetText("Stage Selected: " + stage3);
        MainMenu();
    }

    public void PickStage4()
    {
        stageManager.SetStage(stage4);
        //stageManager.SetText("Stage Selected: " + stage4);
        MainMenu();
    }
}
