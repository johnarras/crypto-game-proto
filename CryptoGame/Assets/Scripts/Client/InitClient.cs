using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitClient : BaseBehaviour
{
    public string ToWallet;
    public int GameId;
    public int StartBlockId;

    private void Awake()
    {
        OnAwake();
    }

    private void OnAwake()
    {
        SetupService setupService = new SetupService();

        setupService.SetupGame(gs, ps,ToWallet,GameId,StartBlockId);

    }
}
