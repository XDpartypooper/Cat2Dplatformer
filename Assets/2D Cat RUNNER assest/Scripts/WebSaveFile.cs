using UnityEngine;
using System.IO;
using System;
#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

public class WebSaveFile
{
    // Use Path.Combine to prevent slash direction issues between Windows and WebGL
    public static readonly string SAVE_AREA = Path.Combine(Application.persistentDataPath, "saves");
    public static readonly string EXT = ".json";

#if UNITY_WEBGL && !UNITY_EDITOR
 
    [DllImport("__Internal")]
    private static extern void JS_FileSystem_Sync();
#endif

    public static void Save(string FileName, string datatosave)
    {
        if (!Directory.Exists(SAVE_AREA))
        {
            Directory.CreateDirectory(SAVE_AREA);
        }

        string fileLoc = Path.Combine(SAVE_AREA, FileName + EXT);
        File.WriteAllText(fileLoc, datatosave);

        // CRITICAL FOR WEBGL: Force browser cache synchronization
#if UNITY_WEBGL && !UNITY_EDITOR
        try 
        {
            JS_FileSystem_Sync();
        }
        catch (Exception e) 
        {
            Debug.LogError($"WebGL Sync Failed: {e.Message}");
        }
#endif
    }

    public static string Load(string filename)
    {
        string fileLoc = Path.Combine(SAVE_AREA, filename + EXT);
        if (File.Exists(fileLoc))
        {
            return File.ReadAllText(fileLoc);
        }
        else
        {
            return null;
        }
    }
}