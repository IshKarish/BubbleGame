using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public Button[] levelButtons;

    public Image locked;

    public Image done;

    private int highestLevel;

    public void Start()
    {
/*        highestLevel = PlayerPrefs.GetInt("highestLevel", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelNum = i + 1;
            if (levelNum > highestLevel)
            {
                levelButtons[i].interactable = false;
                levelButtons[i].GetComponent<Image>().sprite = locked.sprite;
                levelButtons[i].GetComponentInChildren<TextMeshPro>().text = "";
            }
            else
            {
                levelButtons[i].interactable = true;
                levelButtons[i].GetComponentInChildren<TextMeshPro>().text = "" + levelNum;
                levelButtons[i].GetComponent <Image>().sprite = done.sprite;
            }
        }*/
    }

    public void LoadLevel(int levelNum)
    {
        SceneManager.LoadScene("Level_" +  levelNum);
    }


}
