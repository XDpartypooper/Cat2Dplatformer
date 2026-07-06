using UnityEngine;
using System.IO;
using System;

public class SaveFile
{
    public static readonly string SAVE_FOLDER = Application.persistentDataPath + "/saves/";
    public static readonly string FILE_EXT = ".json";

    public static void Save(String FileName, string datatosave)
    {
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }

        File.WriteAllText(SAVE_FOLDER + FileName + FILE_EXT, datatosave);
    }


    public static String Load(String filename)
    {
        String fileLoc = SAVE_FOLDER + filename + FILE_EXT;
        if (File.Exists(fileLoc))
        {
            string LoadedData = File.ReadAllText(fileLoc);

            return LoadedData;
        }
        else
        {
            return null;
        }
    }
}
