using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBar : MonoBehaviour
{
    [SerializeField] private Slider slider1;
    [SerializeField] private Slider slider2;

    [SerializeField] private float number1 = 100f;
    [SerializeField] private float number2 = 100f;

    void Start()
    {
        // Set the slider values to 0 initially
        slider1.value = 0;
        slider2.value = 0;
    }

    //void Update()
    //{
    //    UpdateSliderValues();
    //}
    #region TEST FUNCTIONS
    public void IncreaseLeft()
    {
        number1 += 5;
        UpdateSliderValues();
    }

    public void IncreaseRight()
    {
        number2 += 5;
        UpdateSliderValues();
    }

    public void DecreaseLeft()
    {
        number1 -= 5;
        UpdateSliderValues();
    }

    public void DecreaseRight()
    {
        number2 -= 5;
        UpdateSliderValues();
    }
    #endregion

    public void UpdateLeft(float updateAmount)
    {
        number1 += updateAmount;
        UpdateSliderValues();
    }

    public void UpdateRight(float updateAmount)
    {
        number2 += updateAmount;
        UpdateSliderValues();
    }

    private void UpdateSliderValues()
    {
        if (number1 == number2)
        {
            slider1.value = 0;
            slider2.value = 0;
        }
        else if (number1 > number2)
        {
            slider1.value = (number1 - number2) / number1;
            slider2.value = 0;
        }
        else
        {
            slider1.value = 0;
            slider2.value = (number2 - number1) / number2;
        }
    }
}
