using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class SendWebRequest
{
    public const int MAX_RETRY_TIMES = 4;

    protected const string blobPrefixURL = "https://dogechain.blob.core.windows.net/";

    public static IEnumerator LoadFromBlob<T>(string id, TypedResult<T> result)
    {
        string uri = blobPrefixURL + typeof(T).Name.ToLower() + "/" + id;
        yield return GetObject<T>(uri, result);
    }

    public static IEnumerator GetObject<T>(string uri, TypedResult<T> result)
    {
        WebResult webResult = new WebResult();

        yield return GetRequest(uri, webResult);

        if (webResult.Success && !string.IsNullOrEmpty(webResult.Text))
        {
            try
            {
                result.Data = JsonConvert.DeserializeObject<T>(webResult.Text);
                if (result.Data != null)
                {
                    result.Success = true;
                }
            }
            catch (Exception e)
            {
                Debug.Log("Failed to deserialize type on web request: " + e.Message + " " + typeof(T).Name);
            }
        }
    }

    public static IEnumerator GetRequest(string uri, WebResult result)
    {
        for (int times = 0; times < MAX_RETRY_TIMES; times++)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    result.Text = webRequest.downloadHandler.text;
                    result.Data = webRequest.downloadHandler.data;
                    result.Success = true;
                    yield break;
                }
            }
            
            yield return new WaitForSeconds((times + 1) * 1.0f);
        }
    }
}
