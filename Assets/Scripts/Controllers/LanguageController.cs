using Honeti;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LanguageController : MonoBehaviour {

    private static LanguageController _languageController = null;

    public static LanguageController Instance
    {
        get {
            if (_languageController == null)
                _languageController = FindObjectOfType<LanguageController>();

            return _languageController;
        }
    }
   
   /* private List<string> availableLanguages;

    public List<string> Languages
    {
        get { return availableLanguages; }
        set { availableLanguages = value; }
    }
    */

    private Hashtable languagesTable;

    public Hashtable LanguagesTable
    {
        get { return languagesTable; }
        set { languagesTable = value; }
    }

    public static bool LANGUAGESREADY = false;
    private void Start()
    {
        StartCoroutine(FetchAndReadLanguageJson());
    }
       
    private IEnumerator FetchAndReadLanguageJson()
    {
        string languagesFilePath = Application.streamingAssetsPath + "/LanguageFiles";
        DirectoryInfo directoryInfo = new DirectoryInfo(languagesFilePath);

        FileInfo[] allFiles = directoryInfo.GetFiles("*.json");

        LanguagesTable = new Hashtable();

        foreach (var item in allFiles)
        {  
            using (UnityWebRequest languageFileReader = UnityWebRequest.Get(item.FullName))
            {
                yield return languageFileReader.SendWebRequest();

                if(languageFileReader.isNetworkError || languageFileReader.isHttpError)
                {
                    Debug.LogWarning("Couldn't found JSON file for: " + item.FullName);
                }
                else
                {
                    JObject languageJson = JObject.Parse(languageFileReader.downloadHandler.text);

                    string lang = languageJson["Language"].ToString();

                    LanguageCode thisLangCode = ReturnLanguageCodeFromString(lang);
                    LanguagesTable[thisLangCode] = new Hashtable();

                    Hashtable subs = languageJson["Subtitles"].ToObject<Hashtable>();

                    foreach (DictionaryEntry entry in subs)
                    {
                        if (!(LanguagesTable[thisLangCode] as Hashtable).ContainsKey(entry.Key))
                        {
                            (LanguagesTable[thisLangCode] as Hashtable).Add(entry.Key, entry.Value);
                        }
                    }
                    

                    DebugConsole.instance.Languages.text += thisLangCode.ToString() + "\n";
                }
            }
        }


#if UNITY_STANDALONE
        Debug.Log("Translations loaded");
#endif

        LANGUAGESREADY = true;
    }
    public static LanguageCode ReturnLanguageCodeFromString(string langString)
    {
        LanguageCode code = LanguageCode.EN_GB;

        foreach (var lang in Enum.GetValues(typeof(LanguageCode)))
        {
            if (langString.ToUpper() == lang.ToString())
            {
                code = (LanguageCode)lang;
            }
        }

        return code;
    }
}
