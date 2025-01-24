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
}

