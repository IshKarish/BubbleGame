using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioSource audioSource;
    public Slider audioslider;
    private float currentVolume;
    public GameObject options;
    public static bool IsPaused = false;
    


    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        currentVolume = volume;
    } 
    
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("VolumePreference", currentVolume);
    }

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
