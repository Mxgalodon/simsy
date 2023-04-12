using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public Slider HungrySlider;
    public Slider FunSlider;
    public Slider StaminaSlider;
    public Slider CleanSlider;

    private void Update()
    {
        HungrySlider.value = PlayerScript.Instance.hungry;
        FunSlider.value = PlayerScript.Instance.bored;
        StaminaSlider.value = PlayerScript.Instance.tired;
        CleanSlider.value = PlayerScript.Instance.cleany;
    }
}
