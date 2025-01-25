using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Build.Player;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int _score;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField][CanBeNull] private TextMeshProUGUI _leaderBoardText;

    private const int MaxLeaderboardEntries = 10;
    private const string LeaderboardKeyPrefix = "LeaderboardScore";
    void Awake()
    {
        LoadFromLeaderboard();
        UpdateScoreText(0);

    }

    public void UpdateScoreText(int _score)
    {
        _scoreText.text = "Score: " + _score.ToString();
    }

    public void SaveToLeaderboard(int finalScore)
    {
        Debug.Log("Save to leader board");
        int[] scores = new int[MaxLeaderboardEntries];
        for (int i = 0; i < MaxLeaderboardEntries; i++)
        {
            scores[i] = PlayerPrefs.GetInt($"{LeaderboardKeyPrefix}{i}", 0);
        }

        scores[MaxLeaderboardEntries - 1] = finalScore;

        System.Array.Sort(scores);
        System.Array.Reverse(scores);

        for (int i = 0; i < MaxLeaderboardEntries; i++)
        {
            PlayerPrefs.SetInt($"{LeaderboardKeyPrefix}{i}", scores[i]);
        }

        PlayerPrefs.Save();
    }

    public void LoadFromLeaderboard()
    {
        Debug.Log("Loading Leader Booard");
        string leaderboardTextContent = "Leaderboard: \n";

        for (int i = 0; i < MaxLeaderboardEntries; i++)
        {
            int score = PlayerPrefs.GetInt($"{LeaderboardKeyPrefix}{i}", 0);
            if (score > 0)
            {
                leaderboardTextContent += $"{i + 1}. {score}\n";
            }
        }

        _leaderBoardText.text = leaderboardTextContent;
    }

   

    
}
