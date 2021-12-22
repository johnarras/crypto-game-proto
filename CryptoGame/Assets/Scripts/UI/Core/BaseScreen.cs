using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class BaseScreen : AnimatorBehaviour
{

    public string Name { get; set; }
    public float IntroTime;
    public float OutroTime;

    protected object _openData;

    private static List<GraphicRaycaster> _raycasters = new List<GraphicRaycaster>();

    void Awake()
    {
    }

    virtual protected void OnEnable()
    {
        GraphicRaycaster gr = GetComponent<GraphicRaycaster>();
        if (gr != null)
        {
            _raycasters.Add(gr);
        }
    }

    virtual protected void OnDisable()
    {
        GraphicRaycaster gr = GetComponent<GraphicRaycaster>();
        if (gr != null)
        {
            _raycasters.Remove(gr);
        }
    }

    protected List<GraphicRaycaster> GetAllRaycasters()
    {
        return _raycasters;
    }


    virtual public void StartOpen(object data)
    {
        _openData = data;
        OnStartOpen(data);

        if (IntroTime > 0)
        {
            TriggerAnimation(AnimParams.Intro, IntroTime, OnFinishOpen);
        }
        else
        {
            OnFinishOpen();
        }
    }

    // Called when screen first opens.
    virtual protected void OnStartOpen(object data)
    {
    }

    // Called as the screen finishes opening.
    virtual protected void OnFinishOpen()
    {
    }

    virtual protected void ScreenUpdate()
    {

    }

    virtual public void OnReset()
    {

    }

    virtual public bool BlockMouse()
    {
        return true;
    }

    virtual public void OnInfoChanged()
    {

    }
    virtual public void ErrorClose(string txt)
    {
        if (!string.IsNullOrEmpty(txt))
        {
            Debug.LogError("Error on minigame: " + txt);
        }

        StartClose();
    }


    virtual public void StartClose()
    {
        OnStartClose();
        if (OutroTime > 0)
        {
            TriggerAnimation(AnimParams.Outro, OutroTime, OnFinishClose);
        }
        else
        {
            OnFinishClose();
        }
    }

    // Called immediately on start close.
    virtual protected void OnStartClose()
    {

    }

    // Called after close animation ends.
    virtual protected void OnFinishClose()
    {
        ScreenService ss = gs.fact.Get<ScreenService>();
        ss.FinishClose(gs, Name);
    }





}



