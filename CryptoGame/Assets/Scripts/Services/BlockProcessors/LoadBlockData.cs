using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

// opreturn.net
public class LoadBlockData : IBlockProcessor
{

    protected const string blobPrefixURL = "https://dogechain.blob.core.windows.net/";

    protected const string lastBlockSavedContainer = "lastblocksaved";
    protected const string blockDataContainer = "blockdata";
    protected const string listDataContainer = "blocklist";

    protected const string listContentsURL = "?restype=container&&comp=list";



    public static readonly List<string> BadAddresses = new List<string>()
        {
            "coinbase",
            "nonstandard",
        };
    public virtual IEnumerator Process(GameState gs, PlayerState ps)
    {
        string fullListURL = blobPrefixURL + blockDataContainer + listContentsURL;

        WebResult result = new WebResult();

        yield return SendWebRequest.GetRequest(fullListURL, result);


        if (!result.Success)
        {
            Debug.Log("Error: " + result.Text);
            yield break;
        }

        string[] words = result.Text.Split("<Name>");

        List<long> Ids = new List<long>();

        for (int i = 0; i < words.Length; i++)
        {
            int leftIndex = words[i].IndexOf("<");
            if (leftIndex <= 1)
            {
                continue;
            }

            string numberStr = words[i].Substring(0, leftIndex);

            if (Int64.TryParse(numberStr, out long numberVal))
            {
                Ids.Add(numberVal);
            }
        }

        foreach (long id in Ids)
        {
            Debug.Log("ID: " + id);
        }
    }
}
