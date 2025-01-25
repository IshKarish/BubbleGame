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
    [SerializeField] private float arrowVelocity = 2;
    
    [SerializeField] private Transform[] spawners = new Transform[4];
    [SerializeField] private GameObject arrowPrefab;
    
    [SerializeField] private string mapName;
    private Dictionary<float, int> _mapData;
    
    private float _nextTimestamp;
    private int _nextLane;
    
    private float[] _activeTimeStamps = new float[4];
    
    private int _nextNoteIndex;

    private int _playerScore;
    private int _currentCombo;
    private int _highestCombo;

    [Header("Timing Settings")] 
    [SerializeField] private TimingThresholds timingThresholds;

    [Header("Input Settings")]
    [SerializeField] private KeyCode[] laneKeys = { KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.UpArrow, KeyCode.RightArrow };

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

        foreach (var key in laneKeys)
        {
            if (Input.GetKeyDown(key))
            {
                float currentTime = Time.time;
                int lane = GetLaneFromInput(key);

                CompareTiming(currentTime, lane);
            }
        }
    }

    int GetLaneFromInput(KeyCode key)
    {
        return key switch
        {
            KeyCode.DownArrow => 1,
            KeyCode.UpArrow => 3,
            KeyCode.LeftArrow => 0,
            KeyCode.RightArrow => 1,
            _ => 0
        };
    }

    void SpawnNextNote()
    {
        _activeTimeStamps[_nextLane - 1] = _nextTimestamp;
        
        GameObject newArrow = Instantiate(arrowPrefab, spawners[_nextLane - 1].position, Quaternion.identity);
        newArrow.GetComponent<Rigidbody2D>().linearVelocityX = arrowVelocity;
            
        _nextNoteIndex++;
            
        _nextTimestamp = _mapData.Keys.ToArray()[_nextNoteIndex];
        _nextLane = _mapData.Values.ToArray()[_nextNoteIndex];
        
        Destroy(newArrow, 1.5f);
    }

    void CompareTiming(float inputTime, int lane)
    {
        float timeDifference = Mathf.Abs(inputTime - _activeTimeStamps[lane]);
        
        Debug.Log(timeDifference);
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

//I added above the logic for applying scores and keeping the player's combo streak and final score calculation.
//I also added below the logic for assigning the scores based on the thresholds, though we still need to change variables to match our code
//and place the logic in the input handler. basically it takes the input's timestamp and calculates it's distance from the nearest timestamp,
//after that it takes the time difference between the input and the timestamp and places it within one of the 2 thresholds and assigns score
//accordingly, if it can't the time difference within one of the two thresholds it registers a miss whether it's too late or there is no valid 
//beat in either threshold's range.