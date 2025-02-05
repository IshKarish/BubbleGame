using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelData
{
    public readonly string LevelName;
    public readonly string AuthorName;
    public readonly int Bpm;
    
    public readonly int Channels;
    public readonly int SamplesLength;
    public readonly int Frequency;
    public readonly float[] Samples;

    public readonly float[] Timestamps;
    public readonly int[] Lanes;

    public readonly LevelInfo LevelInfo;
    public readonly AudioClip Clip;
    public readonly SerializedDictionary<float, int> Data;
    
    public LevelData (LevelInfo levelInfo, AudioClip clip, SerializedDictionary<float, int> data)
    {
        LevelInfo = levelInfo;
        Clip = clip;
        Data = data;
    }

    public LevelData(LevelEditor editor)
    {
        LevelName = editor.LevelInfo.LevelName;
        AuthorName = editor.LevelInfo.AuthorName;
        Bpm = editor.LevelInfo.Bpm;
            
        Channels = editor.clip.channels;
        SamplesLength = editor.clip.samples * Channels;
        Frequency = editor.clip.frequency;
        Samples = GetAudioSamples(editor.clip, SamplesLength);

        Timestamps = editor.data.Keys.ToArray();
        Lanes = editor.data.Values.ToArray();
    }

    float[] GetAudioSamples(AudioClip clip, int sampleLength)
    {
        float[] samples = new float[sampleLength];
        clip.GetData(samples, 0);

        return samples;
    }
}
