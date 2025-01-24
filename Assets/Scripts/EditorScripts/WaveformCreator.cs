using UnityEngine;
using UnityEngine.UI;

public class WaveformCreator : MonoBehaviour
{
    [SerializeField] private Image waveformImage;

    [Header("Values")]
    [SerializeField] private int width = 1024;
    [SerializeField] private int height = 64;
    [SerializeField] private Color background = Color.black;
    [SerializeField] private Color foreground = Color.yellow;
    
    private int _sampleSize;
    private float[] _samples;
    private float[] _waveform;

    public void CreateWaveform(AudioClip clip)
    {
        Texture2D waveform = GetWaveform(clip);
        Rect rect = new Rect(Vector2.zero, new Vector2(width, height));
        Sprite sprite = Sprite.Create(waveform, rect, Vector2.zero);

        waveformImage.sprite = sprite;
    }

    private Texture2D GetWaveform(AudioClip clip)
    {
        int halfHeight = height / 2;
        float heightScale = height * 0.75f;

        // get the sound data
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        _waveform = new float[width];

        _sampleSize = clip.samples * clip.channels;
        _samples = new float[_sampleSize];
        clip.GetData(_samples, 0);

        int packSize = (_sampleSize / width);
        for (int w = 0; w < width; w++)
        {
            _waveform[w] = Mathf.Abs(_samples[w * packSize]);
        }

        // map the sound data to texture
        // 1 - clear
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tex.SetPixel(x, y, background);
                
            }
        }

        // 2 - plot
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < _waveform[x] * heightScale; y++)
            {
                tex.SetPixel(x, halfHeight + y, foreground);
                tex.SetPixel(x, halfHeight - y, foreground);
            }
        }

        tex.Apply();

        return tex;
    }
}
