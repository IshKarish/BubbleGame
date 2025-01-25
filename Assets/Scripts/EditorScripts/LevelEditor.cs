using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public struct LevelInfo
{
    public string LevelName;
    public string AuthorName;
    public int Bpm;
}

public class LevelEditor : MonoBehaviour
{
    [SerializeField] private Transform cursor;
    [SerializeField] private TextMeshProUGUI levelInfoText;
    
    public SerializedDictionary<float, int> data;
    
    public LevelInfo LevelInfo;
    [HideInInspector] public AudioClip clip;
    
    private Camera _camera;
    private AudioSource _audioSource;

    private void Awake()
    {
        _camera = Camera.main;
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        ShowLevelInfo();

        clip = _audioSource.clip;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && IsMouseInBounds()) ManageCursor();
        
        if (Input.GetMouseButtonUp(0) && !_audioSource.isPlaying) _audioSource.Play();

        if (Input.GetKeyDown(KeyCode.Space)) ManageAudio();
        
        if (Input.GetKeyDown(KeyCode.DownArrow)) AddNoteAtCurrentTime(3);
        
        if (Input.GetKeyDown(KeyCode.UpArrow)) AddNoteAtCurrentTime(4);
        
        if (Input.GetKeyDown(KeyCode.LeftArrow)) AddNoteAtCurrentTime(1);
        
        if (Input.GetKeyDown(KeyCode.RightArrow)) AddNoteAtCurrentTime(2);
        
        if (Input.GetKey(KeyCode.A)) ValidateTimestamps();
    }

    void ShowLevelInfo()
    {
        levelInfoText.text = $"Level name: {LevelInfo.LevelName}\n" +
                             $"Created By: {LevelInfo.AuthorName}\n" +
                             $"BPM: {LevelInfo.Bpm}";
    }

    void AddNoteAtCurrentTime(int lane)
    {
        if (!data.TryAdd(_audioSource.time, lane)) Debug.Log("No");
    }

    void ManageCursor()
    {
        float xPos = _camera.ScreenToWorldPoint(Input.mousePosition).x;

        Vector3 cursorPos = new Vector3(xPos, cursor.position.y, cursor.position.z);
        cursor.position = cursorPos;

        FindSongTimeOnCursor(Input.mousePosition.x);
    }

    void ManageAudio()
    {
        if (_audioSource.isPlaying) _audioSource.Pause();
        else _audioSource.Play();
    }

    bool IsMouseInBounds()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        bool hasHit = Physics.Raycast(ray);
        return hasHit;
    }
    
    void FindSongTimeOnCursor(float cursorXPos, bool play = false)
    {
        float songTime = cursorXPos / Screen.width * _audioSource.clip.length;
        _audioSource.time = songTime;
        
        _audioSource.Pause();
        if (play) _audioSource.Play();
    }
    
    void ValidateTimestamps()
    {
        SerializedDictionary<float, int> newData = new SerializedDictionary<float, int>();
        
        for (int i = 0; i < data.Keys.Count; i++)
        {
            float x = data.Keys.ToArray()[i];
            double x1 = x % 0.588;

            float newX = (float)(x + 0.588 - x1);

            while (!newData.TryAdd(newX, data.Values.ToArray()[i]))
            {
                newX += 0.0001f;
            }
            Debug.Log(newX);
        }

        data = newData;
    }

    public void SaveLevel()
    {
        LevelData levelData = new LevelData(this);
        SaveSystem.SaveLevel(levelData);
    }
}
