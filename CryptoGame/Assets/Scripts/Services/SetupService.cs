﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SetupService
{
    protected void SetupObjectFactory(GameState gs, PlayerState ps)
    {
        gs.fact = new ObjectFactory(gs);

        gs.fact.Set<ReflectionService>(new ReflectionService());
        gs.fact.Set<IProcessBlockService>(new ProcessBlockService());
        gs.fact.Set<CommandHandlerService>(new CommandHandlerService());
        gs.fact.Set<IGameDataService>(new GameDataService());
        gs.fact.Set<IUpdateEconService>(new UpdateEconService());





    }
    public void SetupGame(GameState gs, PlayerState ps, string toWallet)
    {
        SetupRepo(gs, ps, toWallet);
        SetupSettings(gs, ps);
        SetupObjectFactory(gs, ps);
        RunServiceSetups(gs, ps);
        SetupUnityServices(gs, ps);
        SetupGameData(gs, ps);
    }

    protected void SetupSettings(GameState gs, PlayerState ps)
    {
        gs.settings = new GameSettings();
    }

    protected void SetupRepo(GameState gs, PlayerState ps, string toWallet)
    {
        gs.repo = new Repository();

        long startBlockId = BlockIdList.MinBlock;

        LastBlockCompleted completed = gs.repo.Load<LastBlockCompleted>(toWallet);

        if (completed != null && completed.LastBlockId > startBlockId)
        {
            startBlockId = completed.LastBlockId;
        }
        gs.processing = new ProcessingData()
        {
            ToWallet = toWallet,
            BlockId = startBlockId,
        };
    }


    protected void RunServiceSetups(GameState gs, PlayerState ps)
    { 
        List<IService> allServices = gs.fact.GetAll();

        foreach (IService service in allServices)
        {
            service.Setup(gs, ps);
        }
    }

    protected void SetupGameData(GameState gs, PlayerState ps)
    {
        IGameDataService gameService = gs.fact.Get<IGameDataService>();

        gs.data = new GameData();

        gameService.SetupGameData(gs);
    }

    protected void SetupUnityServices(GameState gs, PlayerState ps)
    {
        AddUnityService<ScreenService>(gs, ps);
    }

    protected void AddUnityService<T>(GameState gs, PlayerState ps) where T :ServiceBehaviour
    {
        T service = GameObjectUtils.GetComponent<T>(gs.GetInitObject());
        if (service != null)
        {
            gs.fact.Set(service);
        }
    }

}
