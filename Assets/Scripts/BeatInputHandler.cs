using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BeatInputHandler : MonoBehaviour
{
    [SerializeField] private string mapName;
    
    [Header("Spawners")]
    [SerializeField] private Transform[] arrowSpawners = new Transform[4];
    [SerializeField] private Transform[] bubbleSpawners = new Transform[4];

    [Header("Arrow Values")] [SerializeField]
    private GameObject[] windPrefabs = new GameObject[4];
    [SerializeField] private float arrowVelocity = 2;
    
    [SerializeField] private GameObject bubblePrefab;

    [Header("Input Settings")]
    [SerializeField] private KeyCode[] laneKeys = { KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.UpArrow, KeyCode.RightArrow };
    
    private Dictionary<float, int> _mapData;
    
    private float _nextTimestamp;
    private int _nextLane;
    
    private float[] _activeTimeStamps = new float[4];
    
    private int _nextNoteIndex;

    private int _playerScore;
    private int _currentCombo;
    private int _highestCombo;
    
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
        if (_nextNoteIndex != -1 && Time.time >= _nextTimestamp - 9.515f) SpawnNextNote();
    }

    void SpawnNextNote()
    {
        _activeTimeStamps[_nextLane - 1] = _nextTimestamp;
        
        GameObject newArrow = Instantiate(windPrefabs[_nextLane - 1], arrowSpawners[_nextLane - 1].position, Quaternion.identity);
        newArrow.GetComponent<Rigidbody2D>().linearVelocityX = arrowVelocity;

        if (_nextNoteIndex < _mapData.Keys.Count - 1)
        {
            _nextNoteIndex++;

            _nextTimestamp = _mapData.Keys.ToArray()[_nextNoteIndex];
            _nextLane = _mapData.Values.ToArray()[_nextNoteIndex];
        }
        else _nextNoteIndex = -1;
    }
    
    void RegisterHit(int scoreValue)
    {
        Debug.Log("Hit");
        _playerScore += scoreValue;
        _currentCombo++;

        if (_currentCombo > _highestCombo) _highestCombo = _currentCombo;
        
        // -8 / 1.5
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