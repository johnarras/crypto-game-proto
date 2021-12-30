using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GameState
{
    public GameData data;
    public Repository repo;
    public GameSettings settings;
    public Dispatcher dispatcher;
    public BlockData block;
    public ObjectFactory fact;
    public MyRandom rand;
    public World world;
    public string toWallet;
    public long maxProcessBlock;
    public string processMessage;
    public bool blocksAreDownloaded;

    private GameObject initObject = null;
    private InitClient initComponent = null;

    public GameObject GetInitObject()
    {
        return initObject;
    }

    public void StartCoroutine(IEnumerator enumer)
    {
        if (initComponent != null)
        {
            initComponent.StartCoroutine(enumer);
        }
    }
    public void SetInitObject(GameObject go)
    {
        if (go == null)
        {
            return;
        }
        initObject = go;
        initComponent = go.GetComponent<InitClient>();
    }
}