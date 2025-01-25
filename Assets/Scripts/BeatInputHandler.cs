using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct TimingThresholds
{
    public float perfectThreshold;
    public float goodThreshold;
}

public class BeatInputHandler : MonoBehaviour
{
    [SerializeField] private Transform[] spawners = new Transform[4];
    [SerializeField] private GameObject bubblePrefab;
    
    [SerializeField] private string mapName;
    private Dictionary<float, int> _mapData;
    
    private float _nextTimestamp;
    private int _nextLane;
    
    private int _nextNoteIndex;

    private int _playerScore;
    private int _currentCombo;
    private int _highestCombo;

    [Header("Timing Settings")] public TimingThresholds timingThresholds;

    [Header("Input Settings")]
    public KeyCode[] laneKeys = { KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.UpArrow, KeyCode.RightArrow };

    void Start()
    {
        LevelData data = SaveSystem.LoadLevel(Application.persistentDataPath + "/" + mapName + ".roy");
        _mapData = data.Data;

        GetComponent<AudioSource>().clip = data.Clip;
        GetComponent<AudioSource>().Play();

        _nextTimestamp = _mapData.Keys.ToArray()[0];
        _nextLane = _mapData.Values.ToArray()[0];
    }

    void Update()
    {
        if (Time.time >= _nextTimestamp) SpawnNextNote();
    }

    void SpawnNextNote()
    {
        Instantiate(bubblePrefab, spawners[_nextLane - 1].position, Quaternion.identity);
        Debug.Log(_nextLane);
            
        _nextNoteIndex++;
            
        _nextTimestamp = _mapData.Keys.ToArray()[_nextNoteIndex];
        _nextLane = _mapData.Values.ToArray()[_nextNoteIndex];
    }

    void RegisterHit(int scoreValue, float timeStamp)
    {
        _playerScore += scoreValue;
        _currentCombo++;

        if (_currentCombo > _highestCombo) _highestCombo = _currentCombo;
    }

    void RegisterMiss()
    {
        _currentCombo = 0;
    }

    public int CalculateFinalScore(int totalBeatsInSong)
    {
        float comboPercentage = _highestCombo / totalBeatsInSong;
        int finalScore = Mathf.RoundToInt(totalBeatsInSong * comboPercentage);  
        
        Debug.Log($"Final Score: {finalScore} | Total Score: {_playerScore} | Highest Combo: {_highestCombo} | Total Beats: {totalBeatsInSong}");
        return finalScore;
    }
}


/*
 
//i added above the logic for applying scores and keeping the player's combo streak and final score calculation.
//i also added below the logic for assigning the scores based on the thresholds, though we still need to change variables to match our code
//and place the logic in the input handler. basically it takes the input's timestamp and calculates it's distance from the nearest timestamp,
//after that it takes the time difference between the input and the timestamp and places it within one of the 2 thresholds and assigns score
//accordingly, if it can't the time difference within one of the two thresholds it registers a miss whether it's too late or there is no valid 
//beat in either threshold's range.
 
float closestBeatTime = -1f;
        float closestTimeDifference = float.MaxValue;

        foreach (float beatTime in activeBeats)
        {
            if (beatMap[beatTime] == lane)
            {
                float timeDifference = Mathf.Abs(gameTime - beatTime);

                if (timeDifference < closestTimeDifference)
                {
                    closestTimeDifference = timeDifference;
                    closestBeatTime = beatTime;
                }
            }
        }

        if (closestBeatTime >= 0)
        {
            if (closestTimeDifference <= timingThresholds.perfectThreshold)
            {
                RegisterHit(2, closestBeatTime); // Perfect
            }
            else if (closestTimeDifference <= timingThresholds.goodThreshold)
            {
                RegisterHit(1, closestBeatTime); // Good
            }
            else
            {
                RegisterMiss(); // Miss (too late)
            }
        }
        else
        {
            RegisterMiss(); // Miss (no valid beat in range)
        }
*/