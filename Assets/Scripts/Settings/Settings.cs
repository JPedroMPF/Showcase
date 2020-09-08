using UnityEngine;

[System.Serializable]
public class Settings : ASettings
{

    [Header("Main settings")]
    public string defaultLanguage = "EN";
    public int height = 1920;
    public int width = 358;
    public int defaultFrameRate = 30;
    public int qualityLevel = 2;
    public bool fullscreen = true;


    [Header("Gate settings")]
    public string ledIdle = "#FFFFFF";
    public string ledRead = "#FFFFFF";

    [Header("Photo Camera settings")]
    public string cameraLedTakingPhoto = "#FFFFFF";
    public string cameraLedPhotoTaken = "#FFFFFF";

    [Header("Network")]
    public string avatarServerUrl = "localhost:8085";

    [Header("Log")]
    public double LogMaxFileSize = 0.02d;
    public double logMaxFolderSize = 10d;

    [Header("--Enum and Class--")]
    public AvatarType avatarType;


    #region Enums and classes for serialization

    public enum AvatarType
    {
        Male,
        Female,
        Random
    }

    //public enum AvatarQuality
    //{
    //    VeryLow,
    //    Low,
    //    Medium,
    //    High,
    //    VeryHigh,
    //    Ultra
    //}



    #endregion

    private new void Awake()
    {
        base.Awake();
        
    }


    #region  Singelton
    public static Settings _instance;
    public static Settings Instance { get
        {
            if (_instance == null)
                _instance = FindObjectOfType<Settings>();
            return _instance; }
    }
    private void SetupSingelton()
    {
        if (_instance != null)
        {
            Debug.LogError("Error in settings. Multiple singeltons exists: " + _instance.name + " and now " + this.name, this);
        }
        else
        {
            _instance = this;
            
        }
    }
    #endregion



}
