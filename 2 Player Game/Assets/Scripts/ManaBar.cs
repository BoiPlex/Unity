using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Slider slider;

    public void SetHealth(float mana)
    {
        slider.value = mana;
    }
}
