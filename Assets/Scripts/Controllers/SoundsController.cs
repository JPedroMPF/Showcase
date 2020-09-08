using Honeti;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public  class SoundsController:MonoBehaviour  {

    public static SoundsController Instance = null;
    public static bool SOUNDSREADY = false;

    string soundsPath;

    public static bool QuestionsReady = false;
    private Dictionary<string, Dictionary<string, AudioClip>> _questionsAudioClips = new Dictionary<string, Dictionary<string, AudioClip>>();

    public Dictionary<string, Dictionary<string, AudioClip>> QuestionAudioClips
    {
        get{return _questionsAudioClips; }
        set { _questionsAudioClips = value; }
    }

    public static bool SubtitlesReady = false;
    private Dictionary<string, Dictionary<string, AudioClip>> _subtitlesAudioClips = new Dictionary<string, Dictionary<string, AudioClip>>();

    public Dictionary<string, Dictionary<string, AudioClip>> SubtitlesAudioClips
    {
        get { return _subtitlesAudioClips; }
        set { _subtitlesAudioClips = value; }

    }


    public delegate void SoundsLoad();
    public static event SoundsLoad onSoundLoad;


    private void Awake()
    {
        SetAudiosPath();
        GetAudios();
        if (Instance == null)
        {            
            Instance = this;
        }      
        
        
    }
    public IEnumerator Start()
    {
        yield return StartCoroutine(SetAudiosPath());
        yield return StartCoroutine(GetAudios());
    }

    private IEnumerator SetAudiosPath()
    {
        //while(AvatarController?.Instance.ActiveAvatar ==  null)
        //{
        //    yield return false;
        //}        
        soundsPath = Application.streamingAssetsPath + "/Sounds/"+ Settings.Instance.avatarType;
        yield return false;
    }
        

    public IEnumerator GetAudios()
    {
        yield return StartCoroutine(LoadAudios("/Questions", QuestionAudioClips, result => { QuestionsReady = result; }));
        yield return StartCoroutine(LoadAudios("/Subtitles", SubtitlesAudioClips, result => { SubtitlesReady = result; }));
    }

    public AudioClip GetAudioByName(SoundType soundType, LanguageCode lang, string soundName)
    {
        AudioClip fetchAudio = null;

        string langKey = lang.ToString();
        string soundToSearch = soundName + "_" + langKey;

        try
        {
            switch (soundType)
            {
                case SoundType.Questions:

                    fetchAudio = QuestionAudioClips[langKey][soundToSearch];

                    break;
                case SoundType.Subtitles:

                    fetchAudio = SubtitlesAudioClips[langKey][soundToSearch];
                    break;


            }
        }
        catch (Exception)
        {

            fetchAudio = null;

            throw;
        }


        return fetchAudio;
    }
     
    
    private IEnumerator LoadAudios(string path, Dictionary<string, Dictionary<string, AudioClip>> dictionaryToSave, Action<bool> callBack)
    {
        string currentFolderToLook = soundsPath + path;

        
        DirectoryInfo directoryInfo = new DirectoryInfo(currentFolderToLook);

        FileInfo[] allFiles = directoryInfo.GetFiles("*.*");
        DirectoryInfo[] directories = directoryInfo.GetDirectories();
        //LOOP LANGUAGES
        foreach (var item in directories)
        {
            string languageCode = Path.GetFileNameWithoutExtension(item.FullName);

            Dictionary<string, AudioClip> clipsForThisLanguage = new Dictionary<string, AudioClip>();

            //GRAB LANGUAGE DIRECTORY AND GET SOUND FILES
            DirectoryInfo innerDirectory = new DirectoryInfo(currentFolderToLook + "/" + languageCode);
            FileInfo[] innerFiles = innerDirectory.GetFiles("*.wav");

            foreach (var innerItem in innerFiles)
            {
                string key = Path.GetFileNameWithoutExtension(innerItem.FullName);

                AudioClip thisClip = AudioClip.Create(innerItem.FullName,44100,2,44100,true);

                yield return StartCoroutine(ConvertToAudioClip(innerItem.FullName, result => {
                    thisClip = result;
                    clipsForThisLanguage.Add(key, thisClip);
                }));

            }

            dictionaryToSave.Add(languageCode, clipsForThisLanguage);
        }

        callBack(true);

        

        if (_questionsAudioClips.Count == 0 || _subtitlesAudioClips.Count == 0)
            yield break;
        else if (_questionsAudioClips.Count > 0 && _subtitlesAudioClips.Count > 0) 
        {
            foreach (var audioLang in _questionsAudioClips)
            {
                DebugConsole.instance.SoundsNmbr.text += audioLang.Key.ToString() + " Ready! \n";
            }
            /*DebugConsole.instance.SoundsNmbr.text += "Questions: " + _questionsAudioClips.Count + " Subs: " + _subtitlesAudioClips.Count + "\n";*/
            //NOTIFY AUDIOS HAVE BEEN LOADED
            if (onSoundLoad != null)
                onSoundLoad();
        }

#if UNITY_STANDALONE
        Debug.Log("Sounds loaded");
#endif
        SOUNDSREADY = true;

    }
   
    IEnumerator ConvertToAudioClip(string path, Action<AudioClip> mySoundResult)
    {
        using (UnityWebRequest audioClipRequest = UnityWebRequestMultimedia.GetAudioClip(path,AudioType.WAV))
        {
            yield return audioClipRequest.SendWebRequest();

            if (audioClipRequest.isNetworkError || audioClipRequest.isHttpError)
            {
                Debug.LogWarning("Unable to find audio clip for: " + path);
            }
            else
            {
                AudioClip test = DownloadHandlerAudioClip.GetContent(audioClipRequest);
                mySoundResult(test);
            }
        }
    }    
}

public enum SoundType
{
    Questions,
    Subtitles,
    SoundFX
}
