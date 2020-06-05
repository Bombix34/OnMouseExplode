using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JsonParse
{
    public static class JsonParse<T>
    {
        public static string ToJson(T obj)
        {
            return JsonUtility.ToJson(obj);
        }

        public static T FromJson(TextAsset file)
        {
            return JsonUtility.FromJson<T>(file.text);
        }

        public static void SaveIntoJsonFile(string filePath, T obj)
        {
            string objText = JsonUtility.ToJson(obj);
            System.IO.File.WriteAllText(filePath+".json", objText);
            Debug.Log("FILE CREATED:"+ filePath+ ".json");
        }
    }
}

