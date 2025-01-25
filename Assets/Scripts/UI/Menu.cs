using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    public void OnQuitButton()
        {
            Application.Quit();
        }

    public void OnLevelSelectionButton()
    {
        SceneManager.LoadScene("LevelScene");
    }

    public void OnLevelEditorButton()
    {
        SceneManager.LoadScene("LevelEditor");
    }

    public void OnBackToStart()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void OnLeaderBoardScene()
    {
        SceneManager.LoadScene("LeaderBoard");
    }

    public void OnGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

}
