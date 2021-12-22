using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine.UI;

public class ScreenService : ServiceBehaviour
{

    public override long GetMinBlockId() { return BlockIdList.V1; }


    public override void Setup (GameState gs, PlayerState ps)
    {

    }

    const string DragParentName = "DragParent";

    public List<ScreenLayer> Layers;


    public List<string> AllowMultiQueueScreens;


    public GameObject DragParent;

    private void Awake()
    {
        GameObjectUtils.DestroyAllChildren(gameObject);
        SetupLayers();
    }

    private bool _haveSetupLayers = false;
    private void SetupLayers()
    {
        if (_haveSetupLayers || Layers == null)
        {
            return;
        }
        _haveSetupLayers = true;
        for (int i = 0; i < Layers.Count; i++)
        {
            Layers[i].CurrentScreen = null;
            Layers[i].ScreenQueue = new List<ActiveScreen>();
            Layers[i].Layer = i + 1;
            if (Layers[i].ScreenNameInfix == null)
            {
                Layers[i].ScreenNameInfix = "";
            }

            Layers[i].ScreenNameInfix = Layers[i].ScreenNameInfix.ToLower();
        }

        GameObjectUtils.DestroyAllChildren(gameObject);
        for (int i = 0; i < Layers.Count; i++)
        {
            Layers[i].LayerParent = new GameObject();
            Layers[i].LayerParent.name = Layers[i].Name + "Layer";
            GameObjectUtils.AddToParent(Layers[i].LayerParent, gameObject);
            if (Layers[i].Name == DragParentName)
            {
                DragParent = Layers[i].LayerParent;
                Canvas canvas = DragParent.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = 10000;
            }
        }
    }

    virtual public object GetDragParent()
    {
        return DragParent;
    }

    private void LateUpdate()
    {
        foreach (ScreenLayer layer in Layers)
        {
            if (layer.CurrentScreen != null)
            {
                continue;
            }
            if (layer.ScreenQueue == null || layer.ScreenQueue.Count < 1)
            {
                continue;
            }

            ActiveScreen nextItem = layer.ScreenQueue[0];
            layer.ScreenQueue.RemoveAt(0);


            GameObject screen = Resources.Load<GameObject>("Screens/" + nextItem.Name);
          
            if (screen != null)
            {
                screen = Instantiate(screen);
                GameObjectUtils.AddToParent(screen, layer.LayerParent);
            }

            OnLoadScreen(gs, nextItem.Name, screen, nextItem);
        }

    }

    private void OnLoadScreen(GameState gs, string url, object obj, object data)
    {
        GameObject screen = obj as GameObject;
        ActiveScreen nextItem = data as ActiveScreen;


        if (screen == null)
        {
            Debug.Log("Couldn't load screen " + url);
            return;
        }

        if (nextItem == null)
        {
            Debug.Log("Couldn't find active screen object for new screen");
            GameObject.Destroy(screen);
            return;
        }

        ScreenLayer layer = nextItem.LayerObject as ScreenLayer;

        if (layer == null)
        {
            Debug.Log("Couldn't find active screen layer for new screen");
            GameObject.Destroy(screen);
            return;
        }


        BaseScreen bs = screen.GetComponent<BaseScreen>();

        if (bs == null)
        {
            GameObject.Destroy(screen);
            Debug.Log("Screen had no BaseScreen on it");
            return;
        }
        bs.Name = nextItem.Name;

        Canvas canvas = screen.GetComponent<Canvas>();

        if (canvas != null)
        {
            canvas.sortingOrder = layer.Layer * 10;
        }

        nextItem.Screen = bs;

        layer.CurrentScreen = nextItem;


        nextItem.Screen.StartOpen(nextItem.Data);


        ClearAllScreensList();

    }


    public void Open(GameState gs, string screenName, object data = null)
    {
        if (gs == null || string.IsNullOrEmpty(screenName))
        {
            Debug.Log("Opening empty screen name");
            return;
        }
        ScreenLayer currLayer = GetLayer(screenName);
        if (currLayer == null)
        {
            Debug.Log("Couldn't find layer for the screen " + screenName);
            return;
        }

        bool allowMultiScreens = false;
        if (AllowMultiQueueScreens != null)
        {
            allowMultiScreens = AllowMultiQueueScreens.Contains(screenName);
        }
        if (!allowMultiScreens)
        {

            List<ActiveScreen> currScreen = GetScreensNamed(gs, screenName);

            if (currScreen != null && currScreen.Count > 0)
            {
                Debug.Log("Screen is already open: " + screenName);
                return;
            };
            foreach (ActiveScreen screen in currLayer.ScreenQueue)
            {
                if (screen.Name == screenName)
                {
                    //Debug.Log("Screen is queued: " + screenName);
                    return;
                }
            }

        }

        ActiveScreen act = new ActiveScreen();
        act.Data = data;
        act.Screen = null;
        act.Layer = currLayer.Layer;
        act.Name = screenName;
        act.LayerObject = currLayer;

        currLayer.ScreenQueue.Add(act);
    }

    public ScreenLayer GetLayer(string name)
    {
        if (Layers == null)
        {
            return null;
        }

        name = name.ToLower();

        ScreenLayer defaultLayer = null;

        foreach (ScreenLayer layer in Layers)
        {
            if (string.IsNullOrEmpty(layer.ScreenNameInfix))
            {
                if (defaultLayer == null)
                {
                    defaultLayer = layer;
                }
            }
            else
            {
                if (name.Contains(layer.ScreenNameInfix))
                {
                    return layer;
                }
            }
        }
        return defaultLayer;
    }


    public void Close(GameState gs, string screenName)
    {
        foreach (ScreenLayer layer in Layers)
        {
            if (layer.CurrentScreen != null && layer.CurrentScreen.Name == screenName)
            {

                BaseScreen baseScreen = layer.CurrentScreen.Screen as BaseScreen;
                if (baseScreen != null)
                {
                    baseScreen.StartClose();
                }
                else
                {
                    layer.CurrentScreen = null;
                }
                break;
            }
        }
    }

    public void FinishClose(GameState gs, string screenName)
    {
        foreach (ScreenLayer layer in Layers)
        {
            if (layer.CurrentScreen != null && layer.CurrentScreen.Name == screenName)
            {

                BaseScreen baseScreen = layer.CurrentScreen.Screen as BaseScreen;
                if (baseScreen != null)
                {
                    GameObject.Destroy(baseScreen.gameObject);
                }
                layer.CurrentScreen = null;
                ClearAllScreensList();
                break;
            }
        }

    }

    public ActiveScreen GetScreen(GameState gs, string screenName)
    {
        foreach (ScreenLayer layer in Layers)
        {
            if (layer.CurrentScreen == null)
            {
                continue;
            }
            if (layer.CurrentScreen.Name != screenName)
            {
                continue;
            }
            return layer.CurrentScreen;
        }
        return null;
    }

    public List<ActiveScreen> GetScreensNamed(GameState gs, string screenName)
    {
        List<ActiveScreen> retval = new List<ActiveScreen>();


        if (string.IsNullOrEmpty(screenName))
        {
            return retval;
        }

        foreach (ScreenLayer layer in Layers)
        {
            if (layer.CurrentScreen == null)
            {
                continue;
            }
            if (layer.CurrentScreen.Name != null && layer.CurrentScreen.Name.Contains(screenName))
            {
                retval.Add(layer.CurrentScreen);
            }
        }
        return retval;
    }

    protected void ClearAllScreensList()
    {
        _allScreens = null;
    }

    private List<ActiveScreen> _allScreens = null;
    public List<ActiveScreen> GetAllScreens(GameState gs)
    {

        if (_allScreens != null)
        {
            return _allScreens;
        }

        _allScreens = new List<ActiveScreen>();

        foreach (ScreenLayer layer in Layers)
        {
            if (layer.CurrentScreen == null || layer.SkipInAllScreensList)
            {
                continue;
            }
            _allScreens.Add(layer.CurrentScreen);
        }
        return _allScreens;
    }


    public void CloseAll(GameState gs)
    {
        foreach (ScreenLayer layer in Layers)
        {
            if (layer.CurrentScreen == null || layer.SkipInAllScreensList)
            {
                continue;
            }
            Close(gs, layer.CurrentScreen.Name);
        }
    }

}