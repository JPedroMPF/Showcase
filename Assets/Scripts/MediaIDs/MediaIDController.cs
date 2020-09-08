using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class MediaIDController : MonoBehaviour
{

    #region Instance

    private static MediaIDController _instance;

    public static MediaIDController Instance { get { return _instance; } }


    private void Awake()
    {       
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    public static bool MEDIAIDLOADED = false;
        
    public List<MediaIDObject> supportedMediaIDsList = new List<MediaIDObject>();

    MediaID mediaIDS = new MediaID();
    string rawMediaIDSstring;
    static string FileName = "/MediaID.json";    
    string filePath = Path.Combine(Application.streamingAssetsPath + FileName);

    private void Start()
    {
#if UNITY_EDITOR
        SetScriptAwakeOrder<MediaIDController>(short.MinValue);
#endif

        LoadMediaIDsJson();
        FilterSupportedMediaIDs();
    }

    public List<MediaIDObject> FilterSupportedMediaIDs()
    {     
        foreach (var item in mediaIDS.mediaIdList)
        {
            if(item.active == true)
            {
                supportedMediaIDsList.Add(item);
            }
        }
        return supportedMediaIDsList;
    }

    public List<MediaIDObject> GetSupportedMediaIDsList()
    {
        if(supportedMediaIDsList.Count == 0)
        {
            Debug.Log("No supported media IDS, check MEDIAIDS.json file");
            return null;
        }

        return supportedMediaIDsList;
    }

    public string GetSupportedMediaIDsString()
    {        
        string supportedMediaIDs = JsonConvert.SerializeObject(supportedMediaIDsList);
        return supportedMediaIDs;
    }

    public string GetSupportedMediaIDsFiltered()
    {        
        var supportedMediaIDs = JsonConvert.SerializeObject(supportedMediaIDsList.Select(x => new { x.path, x.active }));
        return supportedMediaIDs;
    }

    public bool GetMediaIDActiveValue(string mediaID)
    {
        foreach (var item in supportedMediaIDsList)
        {
            if(item.avatarEvent == mediaID)
            {
                return true;
            }
        }
        return false;
    }

    #region Load
    public void LoadMediaIDsJson()
    {
        if (!File.Exists(filePath))
            return;

        rawMediaIDSstring = File.ReadAllText(filePath);
        mediaIDS = JsonConvert.DeserializeObject<MediaID>(rawMediaIDSstring);
        Debug.Log("MediaIDS json loaded" + rawMediaIDSstring);

        MEDIAIDLOADED = true;
    }

    #endregion

    #region Save

    public void AddMediaIDInfo(string avatarEventIN, string mediaIDName, bool activeState, bool subtitleState, bool audioState)
    {
        MediaIDObject newMediaID = new MediaIDObject();
        newMediaID.avatarEvent = avatarEventIN;
        newMediaID.path = mediaIDName;
        newMediaID.active = activeState;
        newMediaID.subtitle = subtitleState;
        newMediaID.audio = audioState;
        mediaIDS.mediaIdList.Add(newMediaID);

    }
    public void SaveSupportedMediaID()
    {
        var json = JsonConvert.SerializeObject(mediaIDS, Formatting.Indented);
        File.WriteAllText(filePath, json);
        Debug.Log(json);

    }

    #endregion

    #region Editor Methods

#if UNITY_EDITOR
    public static void SetScriptAwakeOrder<T>(short num)
    {
        string scriptName = typeof(T).Name;
        SetScriptAwakeOrder(scriptName, num);
    }
    public static void SetScriptAwakeOrder(string scriptName, short num)
    {
        foreach (var monoScript in UnityEditor.MonoImporter.GetAllRuntimeMonoScripts())
        {
            if (monoScript.name == scriptName)
            {
                var exeOrder = UnityEditor.MonoImporter.GetExecutionOrder(monoScript);
                if (exeOrder != num)
                {
                    //Debug.Log("Change script " + monoScript.name + " old " + exeOrder + " new " + num);
                    UnityEditor.MonoImporter.SetExecutionOrder(monoScript, num);
                }
                break;
            }
        }
    }
#endif


    #endregion
   
}
