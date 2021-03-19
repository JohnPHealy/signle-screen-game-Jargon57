using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerScore : MonoBehaviour
{
    public Text TimeTxt;
    float Time;

    public Text attemptstxt;
    int attempts = 1;

    public void go()
    {
        InvokeRepeating("addTime", 0, 0.05f);
    }

    private void addTime()
    {
        Time += 0.05f;
    }

    public void modTime(float Value)
    {
        Time += Value;
    }

    private void FixedUpdate()
    {
        if (Time < 0)
        {
            Time = 0;
        }
        TimeTxt.text = "Time: " + Time.ToString("0.00");
    }

    public void Reset()
    {
        attempts++;
        attemptstxt.text = "Attempts: " + attempts;
        CancelInvoke();
        Time = 0;
    }
}
