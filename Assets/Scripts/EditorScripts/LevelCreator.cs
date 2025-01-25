using System.Collections;
using System.IO;
using UnityEngine;
using SimpleFileBrowser;
using TMPro;
using UnityEngine.Networking;

public class LevelCreator : MonoBehaviour
{
    [Header("Input Fields")]
    [SerializeField] private TMP_InputField levelNameInput;
    [SerializeField] private TMP_InputField authorInput;
    [SerializeField] private TMP_InputField bpmInput;
    
    private AudioSource _audioSource;
    private WaveformCreator _waveformCreator;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _waveformCreator = GetComponent<WaveformCreator>();
    }

    public void UploadSong(bool newSong)
    {
        FileBrowser.Filter wavFilter = new FileBrowser.Filter("Wav file", ".wav");
        FileBrowser.Filter royFilter = new FileBrowser.Filter("Roy file", ".roy");
        
        FileBrowser.SetFilters(false, newSong ? wavFilter : royFilter);
        FileBrowser.AddQuickLink("BubbleGame", Application.persistentDataPath);
        
        StartCoroutine(newSong ? LoadAudioDialog() : LoadRoyDialog());
    }

    IEnumerator LoadAudioDialog()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders);

        if (FileBrowser.Success)
        {
            string res = FileBrowser.Result[0];
            string path = Path.Combine(FileBrowserHelpers.GetDirectoryName(res), FileBrowserHelpers.GetFilename(res));

            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file:///" + path, AudioType.WAV);

            yield return www.SendWebRequest();

            _audioSource.clip = ((DownloadHandlerAudioClip)www.downloadHandler).audioClip;
            
            EditorSetup();
        }
    }
    
    IEnumerator LoadRoyDialog()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders);

        if (FileBrowser.Success)
        {
            string res = FileBrowser.Result[0];
            string path = Path.Combine(FileBrowserHelpers.GetDirectoryName(res), FileBrowserHelpers.GetFilename(res));
            
            EditorSetup(SaveSystem.LoadLevel(path));
        }
    }

    void EditorSetup(LevelData levelData)
    {
        _audioSource.clip = levelData.Clip;
        _waveformCreator.CreateWaveform(_audioSource.clip);
        
        LevelEditor editor = GetComponent<LevelEditor>();
        editor.LevelInfo = levelData.LevelInfo;

        editor.data = levelData.Data;

        editor.enabled = true;
    }

    void EditorSetup()
    {
        _waveformCreator.CreateWaveform(_audioSource.clip);
        
        LevelEditor editor = GetComponent<LevelEditor>();
        editor.LevelInfo = CreateLevelInfo();

        editor.enabled = true;
    }

    LevelInfo CreateLevelInfo()
    {
        LevelInfo info;
        
        info.LevelName = levelNameInput.text;
        info.AuthorName = authorInput.text;
        info.Bpm = int.Parse(bpmInput.text);

        return info;
    }
}
