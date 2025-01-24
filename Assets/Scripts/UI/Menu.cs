using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    public GameObject settings;
    public void OnQuitButton()
        {
            Application.Quit();
        }

    public void OnLevelSelectionButton()
    {
        SceneManager.LoadScene("LevelSelection");
    }

    public void OnSettingsButton()
    {
        settings.SetActive(true);
    }

}
