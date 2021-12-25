using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class SendWebRequest
{
    public const int MAX_RETRY_TIMES = 4;

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

            Debug.Log("Failed Download Times: " + times);
            yield return new WaitForSeconds((times + 1) * 10.0f);
        }
    }
}
