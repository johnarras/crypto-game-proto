
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScreenLayer
{
    public int Layer { get; set; }
    public string Name;
    public string ScreenNameInfix;
    public ActiveScreen CurrentScreen;
    public List<ActiveScreen> ScreenQueue;
    public bool SkipInAllScreensList;

    public GameObject LayerParent { get; set; }

}