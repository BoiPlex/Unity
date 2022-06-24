using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteManager : MonoBehaviour
{
    private bool isMuted;

    // Start is called before the first frame update
    void Start()
    {
        isMuted = PlayerPrefs.GetInt("MUTED") == 1;
        AudioListener.pause = isMuted;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            MutePress();
    }

    public void MutePress()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0 : 1;
        PlayerPrefs.SetInt("MUTED", isMuted ? 1 : 0);
    }
}
