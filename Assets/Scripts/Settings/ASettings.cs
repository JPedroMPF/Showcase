using System.IO;
using UnityEngine;

[System.Serializable]
public abstract class ASettings : MonoBehaviour
{

    public const string FileName = "/Settings.json";
    public static string SettingFolderPath = Application.streamingAssetsPath;

    public static string SettingPath = SettingFolderPath + FileName;//FilesToCopy/Settings.txt";

    private bool _loadSettingInEditorPlay = true;
    public bool LoadSettingInEditorPlay { get { return _loadSettingInEditorPlay; } set { _loadSettingInEditorPlay = value; } }

    private bool _autoSave = true;
    public bool AutoSave { get { return _autoSave; } set { _autoSave = value; } }


    public System.Action onValidateFunc;

    protected void Reset()
    {
        if (GameObject.FindObjectsOfType<ASettings>().Length != 1)
        {
            Debug.Log("Too many settings singelton in scene. Destroy one", DLogType.Error);
            if (GameObject.FindObjectsOfType<ASettings>().Length != 1)
            {
                Debug.LogError("Too many settings singelton in scene. Destroy one", this, DLogType.Error);


            }
            else
            {
                name = "SettingsSingleton";
                Debug.Log("Create settings GameObject", DLogType.Log);
            }

            LoadEditorSetting();
#if UNITY_EDITOR
            SetScriptAwakeOrder<ASettings>(short.MinValue);
#endif
        }
    }
    protected void OnValidate()
    {
#if UNITY_EDITOR
        if (AutoSave && Application.isPlaying == false)
        {
            //Debug.Log("Auto save " + Application.isPlaying + " " );
            //Debug.Log("Create settings GameObject", DLogType.Log);
            //     SaveToFile(false);
            // SaveToFile(false);
        }
#endif
    }


    // Use this for initialization
    protected void Awake()
    {

        try
        {        
            LoadSetting(SettingPath, this);
        }
        catch (System.Exception e)
        {
            //Debug.Log(name + " Error with loading scripts " + e);
            Debug.Log("Error with loading settings: " + e, DLogType.Error);
        }
    }


    #region Inside Unity

    private void LoadEditorSetting()
    {
        if (LoadSettingInEditorPlay)
        {
            var filePath = System.IO.Path.Combine(Application.dataPath, SettingFolderPath + FileName);

            LoadSetting(filePath);

        }
    }
    private string GetJsonFromFile(string path)
    {
        if (File.Exists(path))
        {
            return File.ReadAllText(path);
        }
        return null;
    }
    private void LoadSetting(string path, bool log = false)
    {
        var json = GetJsonFromFile(path);

        if (json != null)
        {
            if (log) Debug.Log("Loaded Settings" + json, DLogType.Content);

            try
            {
                JsonUtility.FromJsonOverwrite(json, this);
                
            }
            catch (System.Exception e)
            {
                Debug.Log("Error with overwriting settings file" + e, DLogType.Error);
            }
        }
        else
        {
            Debug.Log("Failed loading settings of " + name + " from " + path, DLogType.Warning);
        }
    }
    #endregion
    #region Unity editor

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
    public void LoadToScript()
    {
        var filePath = System.IO.Path.Combine(Application.dataPath, SettingFolderPath + FileName);
        LoadSetting(filePath, true);
    }

    public void SaveToFile(bool forceWrite = true)
    {
        var filePath = Path.Combine(Application.dataPath, SettingFolderPath + FileName);
        var loaded = GetJsonFromFile(filePath);
        string json = null;
        try
        {
            json = JsonUtility.ToJson(this,true);

        }
        catch (System.Exception e)
        {
            Debug.Log("Error with loading settings file: " + e, DLogType.Error);
        }
        if (forceWrite || loaded != json || json == null) WriteToFile(json);
    }
    private static void WriteToFile(string json)
    {
        Debug.Log("Save settings file at " + ASettings.SettingPath + " with data: " + json, DLogType.Content);
        if (Directory.Exists(SettingFolderPath) == false)
        {
            Directory.CreateDirectory(SettingFolderPath);
        }

        //string json = JsonConvert.SerializeObject(_data.ToArray());

        //using (var fs = File.CreateText(SettingPath))
        //{
        //    fs.Write(' ');
        //}
        File.WriteAllText(SettingPath, json);
    }
    #endregion
}

