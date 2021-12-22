using System;
using System.Collections.Generic;
using System.Text;

using System.Reflection;
using System.Configuration;
// I am very aware that you're looking at this wondering why the heck I rolled my own settings rather than 
// using ConfigurationManager. The reason is so the configurations/login strings/passwords/whatnot 
// are all in one spot so when I create the editor/admin tool/realtime server/other servers, I won't need to 
// have that data stored anywhere else to make those things work. I really want to have each of 
// these strings in one spot only.
public class GameSettings
{
    private Dictionary<string, string> _data = new Dictionary<string, string>();

    /// <summary>
    /// Add a new setting
    /// </summary>
    /// <param name="key">Setting key</param>
    /// <param name="val">Setting value</param>
    public void Add(String key, String val)
    {
        InternalAdd(key, val, _data);
    }

    private void InternalAdd(string key, string val, Dictionary<string, string> dict)
    {
        if (dict == null)
        {
            return;
        }
        if (String.IsNullOrEmpty(key))
        {
            return;
        }
        key = key.ToLower();
        if (dict.ContainsKey(key))
        {
            dict.Remove(key);
        }
        if (!string.IsNullOrEmpty(val))
        {
            dict.Add(key, val);
        }
    }

    /// <summary>
    /// Find a setting
    /// </summary>
    /// <param name="key">Lookup key</param>
    /// <returns>value found for key</returns>
    public String Find(String key, string defaultValue = "")
    {

        if (String.IsNullOrEmpty(key) || _data == null)
        {
            return defaultValue;
        }
        key = key.ToLower();
        if (!_data.ContainsKey(key))
        {
            return defaultValue;
        }
        return _data[key];
    }

    public int FindInt(String key, int defaultValue = 0)
    {
        string str = Find(key);
        if (string.IsNullOrEmpty(str))
        {
            return defaultValue;
        }
        int val = 0;
        if (!Int32.TryParse(str, out val))
        {
            return defaultValue;
        }
        return val;
    }

    public long FindLong(String key, int defaultValue = 0)
    {
        string str = Find(key);
        if (string.IsNullOrEmpty(str))
        { 
            return defaultValue;
        }
        long val = 0;
        if (!Int64.TryParse(str, out val))
        {
            return defaultValue;
        }
        return val;
    }

    public float FindFloat(String key, float defaultValue = 0.0f)
    {
        string str = Find(key);

        if (string.IsNullOrEmpty(str))
        {
            return defaultValue;
        }
        float val = 0.0f;
        if (!Single.TryParse(str, out val))
        {
            return defaultValue;
        }
        return val;

    }


    public bool FindBool(String key, bool defaultValue = false)
    {
        string str = Find(key);

        if (string.IsNullOrEmpty(str))
        {
            return defaultValue;
        }
        bool val = false;
        if (!Boolean.TryParse(str, out val))
        {
            return defaultValue;
        }
        return val;

    }

    /// <summary>
    /// Clear and reset the settings
    /// </summary>
    public void Clear()
    {
        _data = new Dictionary<string, string>();
    }
}


