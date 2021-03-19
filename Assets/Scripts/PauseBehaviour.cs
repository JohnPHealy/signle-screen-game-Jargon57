using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseBehaviour : MonoBehaviour
{

    bool isPaused;

    GameObject PausePanel;

    public Text camButtonTxt;
    public bool autoCam;

    public GameObject cameraBoxes;

    public GameObject DynamicCams;
    public GameObject FollowCamera;

    private void Start()
    {
        PausePanel = transform.GetChild(0).gameObject;
        PausePanel.SetActive(false);
    }

    public void switchCam()
    {
        if (autoCam)
        {
            autoCam = false;
            camButtonTxt.text = "Camera: Follow";
           // cameraBoxes.SetActive(false);
            DynamicCams.SetActive(false);
            FollowCamera.SetActive(true);
        }
        else
        {
            camButtonTxt.text = "Camera: Auto";
            autoCam = true;
           // cameraBoxes.SetActive(true);
            DynamicCams.SetActive(true);
            FollowCamera.SetActive(false);
        }
    }

    public void pause()
    {
        if (!isPaused)
        {
            PausePanel.SetActive(true);
            Time.timeScale = 0;
            isPaused = true;            
        }
        else
        {
            PausePanel.SetActive(false);
            Time.timeScale = 1;
            isPaused = false;
        }
    }

    public void exit()
    {
        Application.Quit();
    }
}
