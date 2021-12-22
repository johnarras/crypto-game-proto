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
    public BlockData block;
    public ProcessingData processing;
    public ObjectFactory fact;
    
    
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