using System;
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





    }
    public void SetupGame(GameState gs, PlayerState ps, string toWallet, long gameId, long blockId)
    {
        if (blockId < BlockIdList.MinBlock)
        {
            blockId = BlockIdList.MinBlock;
        }

        gs.processing = new ProcessingData()
        {
            ToWallet = toWallet,
            GameId = gameId,
            BlockId = blockId,
        };

        SetupSettings(gs, ps);
        SetupRepo(gs, ps);
        SetupObjectFactory(gs, ps);
        RunServiceSetups(gs, ps);
        SetupGameData(gs, ps);
    }

    protected void SetupSettings(GameState gs, PlayerState ps)
    {
        gs.settings = new GameSettings();
    }

    protected void SetupRepo(GameState gs, PlayerState ps)
    {
        gs.repo = new Repository();
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
}
