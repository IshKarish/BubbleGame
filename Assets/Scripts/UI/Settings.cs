using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject options;
    public static bool IsPaused = false;

    public void OnOptionsButton()
    {
        options.SetActive(true);
        Time.timeScale = 0f;
        audioSource.Pause();

    }

    public void OnOptionsBack()
    {
        options.SetActive(false);
        Time.timeScale = 1f;
        audioSource.UnPause();
    }

}
