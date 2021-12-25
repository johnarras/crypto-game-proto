using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitClient : BaseBehaviour
{
    public string ToWallet;
    public int StartBlockId;

    private void Awake()
    {
        OnAwake();
    }

    private void OnAwake()
    {
        gs.SetInitObject(gameObject);
        SetupService setupService = new SetupService();

        setupService.SetupGame(gs, ps,ToWallet,StartBlockId);

        ScreenService screenService = gs.fact.Get<ScreenService>();
        screenService.Open(gs, ScreenList.HUDScreen);


        IProcessBlockService blockService = gs.fact.Get<IProcessBlockService>();

        StartCoroutine(blockService.Process(gs, ps));
    }
}
