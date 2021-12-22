using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Repository
{
    protected static string GetPathPrefix()
    {
        string prefix = Application.persistentDataPath + "/Data";
        if (!Directory.Exists(prefix))
        {
            Directory.CreateDirectory(prefix);
        }

        return prefix;
    }

    protected string GetPath(string id)
    {
        return GetPathPrefix() + "/" + id.Replace("/", "");
    }

    public T Load<T>(string Id) where T : IStringId
    {
        string path = GetPath(typeof(T).Name + Id);
        try
        {
            if (!File.Exists(path))
            {
                return default(T);
            }
            string txt = File.ReadAllText(path, System.Text.Encoding.UTF8);
            return JsonUtility.FromJson<T>(txt);
        }
        catch (Exception e)
        {
            Debug.Log("Failed to read file: " + path + " " + e.Message);
        }
        return default(T);
    }

    public void Save<T> (T t, string id = "") where T : IStringId
    {
        string path = GetPath(typeof(T).Name + (!string.IsNullOrEmpty(id)?id:t.Id));
        File.WriteAllText(path, JsonUtility.ToJson(t));
    }
}