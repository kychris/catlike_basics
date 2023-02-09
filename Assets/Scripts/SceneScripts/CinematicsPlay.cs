using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CinematicsPlay : MonoBehaviour
{
    public InputActionReference RightPrimaryClicked;
    public AudioSource musicPlayer;
    public Material skyboxNight;
    public Light light1;
    public Light light2;

    private bool isMusicStarted = false;
    private bool isMusicPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        RightPrimaryClicked.action.performed += StartMusic;
    }

    void StartMusic(InputAction.CallbackContext action)
    {
        RenderSettings.skybox = skyboxNight;
        light1.enabled = false;
        light2.enabled = true;

        if (!isMusicStarted)
        {
            musicPlayer.Play();
            isMusicStarted = true;
        }
        else if (isMusicPaused)
        {
            musicPlayer.UnPause();
            isMusicPaused = false;
        }
        else
        {
            musicPlayer.Pause();
            isMusicPaused = true;
        }
    }
}
