using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

public class Repository
{
    protected static string GetPathPrefix<T>() where T : IStringId
    {
        string prefix = Application.persistentDataPath + "/Data/" + typeof(T).Name;
        if (!Directory.Exists(prefix))
        {
            Directory.CreateDirectory(prefix);
        }

        return prefix;
    }

    protected string GetPath<T>(string id) where T : IStringId
    {
        return GetPathPrefix<T>() + "/" + id.Replace("/", "");
    }

    public T Load<T>(string Id) where T : IStringId
    {
        string path = GetPath<T>(Id);
        try
        {
            if (!File.Exists(path))
            {
                return default(T);
            }
            string txt = File.ReadAllText(path, System.Text.Encoding.UTF8);
            return JsonConvert.DeserializeObject<T>(txt);
        }
        catch (Exception e)
        {
            Debug.Log("Failed to read file: " + path + " " + e.Message);
        }
        return default(T);
    }

    public void Save<T> (T t, string id = "") where T : IStringId
    {
        string path = GetPath<T>(!string.IsNullOrEmpty(id)?id:t.Id);
        string txt = JsonConvert.SerializeObject(t);
        File.WriteAllText(path, txt);
    }
}