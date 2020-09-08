using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugConsole : MonoBehaviour
{
    public GameObject debugView;
    public GameObject defaultPosition;

    [SerializeField]
    private Text animationLock;
    public Text AnimationLock
    {
        get { return animationLock; }
        set { animationLock = value; }
    }   

    [SerializeField]
    private Text stopedBecause;

    public Text StopedBecause
    {
        get { return stopedBecause; }
        set { stopedBecause = value; }
    }


    [SerializeField]
    private Text lastRequestState;

    public Text LastRequestState
    {
        get { return lastRequestState; }
        set { lastRequestState = value; }
    }

    private string currentStateStringValue;

    public string CurrentStateStringValue
    {
        get { return currentStateStringValue; }
        set {
            
            if (value == "Reset")
            {
                currentStateStringValue = "Idle";
                CurrentStateText.text = "Idle";
            }
            else
            {
                currentStateStringValue = value;
                currentState.text = value;
            }
        }
    }


    [SerializeField]
    private Text currentState;
   
    public Text CurrentStateText
    {
        get { return currentState; }
        set { currentState = value; }
    }

    [SerializeField]
    private Text fps;

    public Text FPS
    {
        get { return fps; }
        set { fps = value; }
    }

    [SerializeField]
    private Text languageCode;

    public Text LanguageCode
    {
        get { return languageCode; }
        set { languageCode = value; }
    }

    [SerializeField]
    private Text soundsNmbr;

    public Text SoundsNmbr
    {
        get { return soundsNmbr; }
        set { soundsNmbr = value; }
    }

    [SerializeField]
    private Dropdown qualitySettings;
    public Dropdown QualitySettings
    {
        get { return qualitySettings; }
        set { qualitySettings = value; }
    }

    [SerializeField]
    private Text versionNmbr;

    public Text Version
    {
        get { return versionNmbr; }
        set { versionNmbr = value; }
    }

    [SerializeField]
    private Text coreVersionNmbr;

    public Text CoreVersion
    {
        get { return coreVersionNmbr; }
        set { coreVersionNmbr = value; }
    }


    [SerializeField]
    private Text languages;

    public Text Languages
    {
        get { return languages; }
        set { languages = value; }
    }

    [SerializeField]
    private Text urlWebSocket;

    public Text SocketIOUrl
    {
        get { return urlWebSocket; }
        set { urlWebSocket = value; }
    }

    [SerializeField]
    private Text urlNodeServer;

    public Text NodeUrl
    {
        get { return urlNodeServer; }
        set { urlNodeServer = value; }
    }


    [SerializeField]
    private Text actuatorValue;

    public Text ActuatorValue
    {
        get { return actuatorValue; }
        set { actuatorValue = value; }
    }


    private static DebugConsole s_Instance = null;   // Our instance to allow this script to be called without a direct connection.
    public static DebugConsole instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(DebugConsole)) as DebugConsole;
                if (s_Instance == null)
                {
                    Debug.LogError("Can't find Debug console Object");
                }

            }

            return s_Instance;
        }
    }

    void Awake()
    {
        s_Instance = this;
        debugView.transform.position = defaultPosition.transform.position;
        ToggleDebugView();
    }


    public void ToggleDebugView()
    {
        debugView.SetActive(!debugView.activeSelf);       

    }
   

}