using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitClient : BaseBehaviour
{
    public string ToWallet;

    private void Awake()
    {
        OnAwake();
    }

    private void OnAwake()
    {
        gs.SetInitObject(gameObject);

        SetupService setupService = new SetupService();
        setupService.SetupGame(gs, ToWallet);

        gs.dispatcher.DispatchEvent(gs, new ShowOverview());

        ScreenService screenService = gs.fact.Get<ScreenService>();
        screenService.Open(gs, ScreenList.HUDScreen);
        screenService.Open(gs, ScreenList.OverviewScreen);


        IProcessBlockService blockService = gs.fact.Get<IProcessBlockService>();

        StartCoroutine(blockService.Process(gs));
    }
}
