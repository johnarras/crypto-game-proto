﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class HUDScreen : BaseScreen
{

    [SerializeField]
    private Text _title = null;

    override protected void OnStartOpen(object data)
    {
        gs.dispatcher.AddEvent<ShowOverview>(OnShowOverview);
        base.OnStartOpen(data);
    }


    protected object OnShowOverview(GameState gs, ShowOverview after)
    {

        UIHelper.SetText(_title, "BlockId " + gs.world.BlockId);
        return null;
    }

    protected override void OnFinishClose()
    {
        gs.dispatcher.RemoveEvent<ShowOverview>(OnShowOverview);
        base.OnFinishClose();
    }
}
