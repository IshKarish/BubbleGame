using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    public GameObject options;
    public void OnQuitButton()
        {
            Application.Quit();
        }

    public void OnLevelSelectionButton()
    {
        SceneManager.LoadScene("LevelSelection");
    }

    public void OnOptionsButton()
    {
        options.SetActive(true);
    }

}
