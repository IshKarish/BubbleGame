using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider audioslider;
    float currentVolume;

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        currentVolume = volume;
    } 
    
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("VolumePreference", currentVolume);
    }
}
