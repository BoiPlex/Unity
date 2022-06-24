using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject p1;

    public int P1Life;

    public GameObject p1Wins;

    public GameObject[] p1Health;

    public AudioSource hurtSound;

    private bool gameEnding = false;

    public string mainMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (P1Life <= 0 && !gameEnding)
        {
            p1.GetComponent<PlayerController>().Die();
            gameEnding = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(mainMenu);
    }

    public void HurtP1()
    {
        P1Life--;
        hurtSound.Play();
        GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerController>().Hit();
        UpdateHealthP1();
    }

    public void HealP1()
    {
        P1Life++;
        UpdateHealthP1();
    }

    private void UpdateHealthP1()
    {
        for (int i = 0; i < p1Health.Length; i++)
        {
            if (P1Life > i)
                p1Health[i].SetActive(true);
            else
                p1Health[i].SetActive(false);
        }
    }

    public void WinScreenP1()
    {
        p1Wins.SetActive(true);
    }

    public bool GameEnding()
    {
        return gameEnding;
    }
}
