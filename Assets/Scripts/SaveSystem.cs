using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

public static class SaveSystem
{
    private const string Format = ".roy";

    public static void SaveLevel(LevelData levelData)
    {
        string path = Application.persistentDataPath + "/" + levelData.LevelName + Format;

        FileStream stream = new FileStream(path, FileMode.Create);
        BinaryWriter writer = new BinaryWriter(stream);
        
        writer.Write(levelData.LevelName);
        writer.Write(levelData.AuthorName);
        writer.Write(levelData.Bpm);

        writer.Write(levelData.Channels);
        writer.Write(levelData.SamplesLength);
        writer.Write(levelData.Frequency);

        foreach (var s in levelData.Samples)
        {
            writer.Write(s);
        }
        
        writer.Write(levelData.Timestamps.Length);
        foreach (var t in levelData.Timestamps)
        {
            writer.Write(t);
        }

        foreach (var l in levelData.Lanes)
        {
            writer.Write(l);
        }
        
        stream.Close();
    }

    public static LevelData LoadLevel(string path)
    {
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);
            BinaryReader reader = new BinaryReader(stream);

            string levelName = reader.ReadString();
            string authorName = reader.ReadString();
            int bpm = reader.ReadInt32();

            int channels = reader.ReadInt32();
            int samplesLength = reader.ReadInt32();
            int frequency = reader.ReadInt32();

            float[] samples = new float[samplesLength];
            for (int i = 0; i < samplesLength; i++)
            {
                samples[i] = reader.ReadSingle();
            }

            float[] timestamps = new float[reader.ReadInt32()];
            for (int i = 0; i < timestamps.Length; i++)
            {
                timestamps[i] = reader.ReadSingle();
            }

            int[] lanes = new int[timestamps.Length];
            for (int i = 0; i < lanes.Length; i++)
            {
                lanes[i] = reader.ReadInt32();
            }
            
            stream.Close();

            LevelInfo levelInfo = new LevelInfo
            {
                LevelName = levelName,
                AuthorName = authorName,
                Bpm = bpm
            };
            
            AudioClip clip = AudioClip.Create("Clip", samplesLength / 2, channels, frequency, false);
            clip.SetData(samples, 0);

            SerializedDictionary<float, int> data = new SerializedDictionary<float, int>();
            for (int i = 0; i < timestamps.Length; i++)
            {
                data.Add(timestamps[i], lanes[i]);
            }
            
            LevelData levelData = new LevelData(levelInfo, clip, data);
            return levelData;
        }

        return null;
    }
}
