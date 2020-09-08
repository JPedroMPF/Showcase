
using System.IO;
using System.Linq;
using UnityEngine;


[System.Serializable]

[ExecuteInEditMode]
public class AvatarFileDebug : MonoBehaviour {
    public bool logTime = true;
    public bool useAbsolutePath = false;
    public string fileName = "Avatar";

    public string absolutePath = "/home/yourUsername/AvatarLogs";

    public string filePath;
    public string filePathFull;
    public int count = 0;

    System.IO.StreamWriter fileWriter;
    FileInfo currentFile;

    void OnEnable()
    {
        UpdateFilePath();    

        if (Application.isPlaying)
        {
            StartLogFile();
            Application.logMessageReceivedThreaded += HandleLog;
        }
    }
    void OnDisable()
    {      
        if (Application.isPlaying)
        {
            Application.logMessageReceivedThreaded -= HandleLog;
            EndLogFile();
        }
    }

    void StartLogFile()
    {
        count = 0;
        fileWriter = new System.IO.StreamWriter(filePathFull, false);
        fileWriter.AutoFlush = true;
        fileWriter.WriteLine("[");

        
    }

    void EndLogFile()
    {
        fileWriter.WriteLine("\n]");
        fileWriter.Close();
    }
    public void UpdateFilePath()
    {
        var outputDir = AvatarUtils.GetArg("-workspacedirectory");

        if (outputDir == null)
            filePath = Application.persistentDataPath + "/logs";
        else
            filePath = outputDir + "/logs";

        System.IO.Directory.CreateDirectory(filePath);
        filePathFull = System.IO.Path.Combine(filePath, fileName + "_" +
            System.DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss") + ".json");
        
    }


    void HandleLog(string logString, string stackTrace, LogType type)
    {
        try
        {
            LogJSON j = new LogJSON();
            if (type == LogType.Assert)
            {
                j.t = "Assert";
                j.l = logString;
            }
            else if (type == LogType.Exception)
            {
                j.t = "Exception";
                j.l = logString;
            }
            else
            {
                int end = logString.IndexOf("]");
                j.t = logString.Substring(1, end - 1);
                j.l = logString.Substring(end + 2);
            }

            j.s = stackTrace;
            j.tm = string.Format("Day {0} | Time: {1}", System.DateTime.Now.ToString("yyyy.MM.dd"), System.DateTime.Now.ToString("HH.mm.ss"));

            fileWriter.Write((count == 0 ? "" : ",\n") + JsonUtility.ToJson(j));
            count++;

            if (HasFileReachMaxSize())
            {
                WriteLastLogOnFile();
                EndLogFile();
                CheckFolderSize();
                UpdateFilePath();
                StartLogFile();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }

        
        
    }

    private void WriteLastLogOnFile()
    {
        LogJSON jF = new LogJSON();
        jF.l = string.Format("[Warning] File: {0} reach max size", currentFile.Name);
        fileWriter.Write((count == 0 ? "" : ",\n") + JsonUtility.ToJson(jF));

    }

    bool HasFileReachMaxSize()
    {
        currentFile = new FileInfo(Path.GetFullPath(filePathFull));
        var fileSize = AvatarUtils.ConvertToMegabytes(currentFile.Length);
        var maxSize = Settings.Instance.LogMaxFileSize;

        return (fileSize > maxSize) ? true : false;
    }    

    private void CheckFolderSize()
    {
        System.IO.DirectoryInfo di = new DirectoryInfo(filePath);

        FileInfo[] files = di.GetFiles().OrderBy(p => p.CreationTime).ToArray();

        long totalSize = 0;

        for (int i = 0; i < files.Length; i++)
        {
            totalSize += files[i].Length;
        }

        print(AvatarUtils.ConvertToMegabytes(totalSize) + " " + Settings.Instance.logMaxFolderSize);

        if (AvatarUtils.ConvertToMegabytes(totalSize) > Settings.Instance.logMaxFolderSize)
        {
            files[0].Delete();
        }
    }
}
[System.Serializable]
public class LogJSON
{
    public string t;  //type
    public string tm; //time
    public string l;  //log
    public string s;  //stack
}

public static class Debug
{
    #region Assert
    public static void Assert(bool condition) { UnityEngine.Debug.Assert(condition); }
    public static void Assert(bool condition, Object context) { UnityEngine.Debug.Assert(condition, context); }
    public static void Assert(bool condition, object message) { UnityEngine.Debug.Assert(condition, message); }
    public static void Assert(bool condition, object message, Object context) { UnityEngine.Debug.Assert(condition, message, context); }
    public static void AssertFormat(bool condition, string format, params object[] args) { UnityEngine.Debug.AssertFormat(condition, format, args); }
    public static void AssertFormat(bool condition, Object context, string format, params object[] args) { UnityEngine.Debug.AssertFormat(condition, context, format, args); }
    public static void LogAssertion(object message) { UnityEngine.Debug.LogAssertion(message); }
    public static void LogAssertion(object message, Object context) { UnityEngine.Debug.LogAssertion(message, context); }
    public static void LogAssertionFormat(string format, params object[] args) { UnityEngine.Debug.LogAssertionFormat(format, args); }
    public static void LogAssertionFormat(Object context, string format, params object[] args) { UnityEngine.Debug.LogAssertionFormat(context, format, args); }
    #endregion

    #region Helper
    public static void Break() { UnityEngine.Debug.Break(); }
    public static void ClearDeveloperConsole() { UnityEngine.Debug.ClearDeveloperConsole(); }
    #endregion

    #region Draw
    public static void DrawRay(Vector3 start, Vector3 dir) { UnityEngine.Debug.DrawRay(start, dir); }
    public static void DrawRay(Vector3 start, Vector3 dir, Color color) { UnityEngine.Debug.DrawRay(start, dir, color); }
    public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration) { UnityEngine.Debug.DrawRay(start, dir, color, duration); }
    public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration, bool depthTest) { UnityEngine.Debug.DrawRay(start, dir, color, duration, depthTest); }
    public static void DrawLine(Vector3 start, Vector3 end) { UnityEngine.Debug.DrawLine(start, end); }
    public static void DrawLine(Vector3 start, Vector3 end, Color color) { UnityEngine.Debug.DrawLine(start, end, color); }
    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration) { UnityEngine.Debug.DrawLine(start, end, color, duration); }
    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration, bool depthTest) { UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest); }
    #endregion

    #region Log
    public static void Log(object message, DLogType type = DLogType.Log) { UnityEngine.Debug.Log("[" + type + "] " + message); }
    public static void Log(object message, Object context, DLogType type = DLogType.Log) { UnityEngine.Debug.Log("[" + type + "] " + message, context); }
    public static void LogFormat(string format, DLogType type = DLogType.Log, params object[] args) { UnityEngine.Debug.LogFormat("[" + type + "] " + format, args); }
    public static void LogFormat(Object context, string format, DLogType type = DLogType.Log, params object[] args) { UnityEngine.Debug.LogFormat(context, "[" + type + "] " + format, args); }
    #endregion

    #region Error
    public static void LogError(object message, DLogType type = DLogType.Error) { UnityEngine.Debug.LogError("[" + type + "] " + message); }
    public static void LogError(object message, Object context, DLogType type = DLogType.Error) { UnityEngine.Debug.Log("[" + type + "] " + message, context); }
    public static void LogErrorFormat(string format, params object[] args) { UnityEngine.Debug.LogErrorFormat(format, args); }
    public static void LogErrorFormat(Object context, string format, params object[] args) { UnityEngine.Debug.LogErrorFormat(context, format, args); }
    #endregion

    #region Exception
    public static void LogException(System.Exception exception) { UnityEngine.Debug.LogException(exception); }
    public static void LogException(System.Exception exception, Object context) { UnityEngine.Debug.LogException(exception, context); }
    #endregion

    #region Warning
    public static void LogWarning(object message, DLogType type = DLogType.Warning) { UnityEngine.Debug.LogWarning("[" + type + "] " + message); }
    public static void LogWarning(object message, Object context, DLogType type = DLogType.Warning) { UnityEngine.Debug.LogWarning("[" + type + "] " + message, context); }
    #endregion
}

public enum DLogType
{
    Assert,
    Error,
    Exception,
    Warning,
    System,
    Log,
    AI,
    Audio,
    Content,
    Logic,
    GUI,
    Input,
    Network,
    Physics
}