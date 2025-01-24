using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TimingThresholds
{
    public float perfectThreshold = 0.1f; //threshold for perfect hit
    public float goodThreshold = 0.25f;   //thershold for good hit
}

public class BeatInputHandler : MonoBehaviour
{
    [Header("Beat Map Settings")]
    public TextAsset beatMapFile; //binary file (.roy) containing the beat map
    private Dictionary<float, int> beatMap; //key: timestamp, Value: lane

    [Header("Timing Settings")]
    public TimingThresholds timingThresholds;

    [Header("Input Settings")]
    public KeyCode[] laneKeys = { KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.UpArrow, KeyCode.RightArrow };

    private int currentCombo = 0;
    private int highestCombo = 0;
    private int totalScore = 0;

    private float gameTime = 0f;
    private List<float> activeBeats = new List<float>();

    void Start()
    {
        beatMap = LoadBeatMap(beatMapFile);
    }

    void Update()
    {
        gameTime += Time.deltaTime;
        CheckForBeats();
        CheckInput();
    }

    private Dictionary<float, int> LoadBeatMap(TextAsset file)
    {
        Dictionary<float, int> map = new Dictionary<float, int>();

        if (file == null)
        {
            Debug.LogError("Beat map file is not assigned!");
            return map;
        }

        using (System.IO.BinaryReader reader = new System.IO.BinaryReader(new System.IO.MemoryStream(file.bytes)))
        {
            int beatCount = reader.ReadInt32(); //number of beats in the map
            for (int i = 0; i < beatCount; i++)
            {
                float timestamp = reader.ReadSingle();
                int lane = reader.ReadInt32();
                map[timestamp] = lane;
            }
        }

        return map;
    }
        private void CheckForBeats()
    {
        //remove beats that are too far in the past
        activeBeats.Clear();
        foreach (var beat in beatMap)
        {
            if (gameTime <= beat.Key + timingThresholds.goodThreshold)
            {
                activeBeats.Add(beat.Key);
            }
        }
    }

    private void CheckInput()
    {
        for (int lane = 0; lane < laneKeys.Length; lane++)
        {
            if (Input.GetKeyDown(laneKeys[lane]))
            {
                HandleInput(lane);
            }
        }
    }

    private void HandleInput(int lane)
    {
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
                RegisterHit(2, closestBeatTime); //perfect score
            }
            else if (closestTimeDifference <= timingThresholds.goodThreshold)
            {
                RegisterHit(1, closestBeatTime); //good score
            }
            else
            {
                RegisterMiss(); //miss (too late)
            }
        }
        else
        {
            RegisterMiss(); //miss (no valid beat in range)
        }
    }

    private void RegisterHit(int scoreValue, float beatTime)
    {
        totalScore += scoreValue;
        currentCombo++;

        if (currentCombo > highestCombo)
        {
            highestCombo = currentCombo;
        }

        beatMap.Remove(beatTime); //remove the beat from the map to avoid duplicate scoring
    }

    private void RegisterMiss()
    {
        currentCombo = 0; //reset combo on a miss
    }

    public int CalculateFinalScore(Dictionary<float, int> beatMap)
    {
        if (beatMap == null || beatMap.Count == 0)
        {
            Debug.LogError("Beat map is null or empty.");
            return 0;
        }
        
        int totalBeatsInSong = beatMap.Count;
        
        float comboPercentage = (float)highestCombo / totalBeatsInSong;

        int finalScore = Mathf.RoundToInt(totalScore * comboPercentage);

        Debug.Log($"Final Score: {finalScore} | Total Score: {totalScore} | Highest Combo: {highestCombo} | Total Beats: {totalBeatsInSong}");
        return finalScore;
    }


    private void OnDrawGizmos()
    {
        // Optional: Visualize beats for debugging
        Gizmos.color = Color.green;

        if (beatMap != null)
        {
            foreach (var beat in beatMap)
            {
                if (Mathf.Abs(gameTime - beat.Key) <= timingThresholds.goodThreshold)
                {
                    Gizmos.DrawSphere(new Vector3(beat.Value, 0, 0), 0.2f);
                }
            }
        }
    }
}

