using System.IO;
using UnityEngine;

public class DataSerializer<T>
{
    public readonly string myFileName;

    public string mySavePath => $"{Application.persistentDataPath}/{myFileName}";

    public DataSerializer(string aFileName)
    {
        myFileName = aFileName;
    }

    public void Save(T aData)
    {
#if UNITY_EDITOR
        string jsonData = JsonUtility.ToJson(aData, true);
#else
        string jsonData = JsonUtility.ToJson(aData, false);
#endif

        File.WriteAllText(mySavePath, jsonData);
    }

    public T Load()
    {
        if (!File.Exists(mySavePath))
        {
            return default;
        }

        string jsonData = File.ReadAllText(mySavePath);
        return JsonUtility.FromJson<T>(jsonData);
    }

    public void Delete()
    {
        File.Delete(mySavePath);
    }
}
