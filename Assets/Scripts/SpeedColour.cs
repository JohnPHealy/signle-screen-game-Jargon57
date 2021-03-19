using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedColour : MonoBehaviour
{
    public Gradient Colours;
    public Slider slider;

    Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        float evealuation = slider.value / slider.maxValue;
        image.color = Colours.Evaluate(evealuation);
    }
}
