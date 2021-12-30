using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class OverviewScreen : BaseScreen
{

    [SerializeField]
    private Text _overview = null;
    override protected void OnStartOpen(object data)
    {
        gs.dispatcher.AddEvent<ShowOverview>(OnShowOverview);
        base.OnStartOpen(data);
    }

    protected object OnShowOverview(GameState gs, ShowOverview after)
    {
        StringBuilder sb = new StringBuilder();

        IWorldService worldService = gs.fact.Get<IWorldService>();

        sb.Append(worldService.PrintOverview(gs));



        UIHelper.SetText(_overview, sb.ToString());


        return null;
    }

    protected override void OnFinishClose()
    {
        gs.dispatcher.RemoveEvent<ShowOverview>(OnShowOverview);
        base.OnFinishClose();
    }

}
