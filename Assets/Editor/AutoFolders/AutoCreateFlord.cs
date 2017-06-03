using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class AutoCreateFolder : Editor {

    //public static string[] autoFolderNameList = {"Plugins", "Plugins/iOS", "Plugins/Android","StreammingAssets","Resources","Scripts","Scripts/Core" ,"Arts"};

    [MenuItem("自动创建目录/创建")]
    static void Create()
    {
        string path = Application.dataPath;
        List<string> autoFolderNameList = new List<string>();

        try
        {
            FileStream fs = new FileStream(path + "/Editor/AutoFolders/AutoCreateFolderNames.ini", FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string strLine = sr.ReadLine();
            while (strLine != null)
            {
                autoFolderNameList.Add(strLine);
                strLine = sr.ReadLine();
            }
            sr.Close();
        }
        catch(IOException IOEX)
        {
            Debug.LogError(IOEX.Message);
            if (EditorUtility.DisplayDialog("File Miss",
             "Can't Find " + path + "/Editor/AutoFolders/AutoCreateFolderNames.ini.", "Exit"))
            {
                return;
            }
        }

        foreach (string folderNames in autoFolderNameList)
        {
            if (!Directory.Exists(path + "/" + folderNames))
            {
                Directory.CreateDirectory(path + "/" + folderNames);
            }
        }

        AssetDatabase.Refresh();
    }
}
