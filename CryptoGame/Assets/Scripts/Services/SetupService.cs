using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SetupService
{
    protected void SetupObjectFactory(GameState gs)
    {
        gs.fact = new ObjectFactory(gs);

        gs.fact.Set<ReflectionService>(new ReflectionService());
        gs.fact.Set<IProcessBlockService>(new ProcessBlockService());
        gs.fact.Set<CommandHandlerService>(new CommandHandlerService());
        gs.fact.Set<IGameDataService>(new GameDataService());
        gs.fact.Set<IUpdateEconService>(new UpdateEconService());
        gs.fact.Set<IPlayerService>(new PlayerService());





    }
    public void SetupGame(GameState gs, string toWallet)
    {
        SetupWallet(gs, toWallet);
        SetupRepo(gs);
        SetupWorld(gs);
        SetupSettings(gs);
        SetupRand(gs);
        SetupObjectFactory(gs);
        RunServiceSetups(gs);
        SetupUnityServices(gs);
        SetupGameData(gs);
    }

    protected void SetupWallet(GameState gs, string toWallet)
    {
        gs.toWallet = toWallet;
    }

    protected void SetupSettings(GameState gs)
    {
        gs.settings = new GameSettings();
    }

    protected void SetupRepo(GameState gs)
    {
        gs.repo = new Repository();
    }

    protected void SetupWorld(GameState gs)
    {
        WorldLoader loader = new WorldLoader();
        loader.LoadWorld(gs);
    }

    protected void SetupRand(GameState gs)
    {
        gs.rand = new MyRandom(gs.world.GetSeed());
    }

    protected void RunServiceSetups(GameState gs)
    { 
        List<IService> allServices = gs.fact.GetAll();

        foreach (IService service in allServices)
        {
            service.Setup(gs);
        }
    }

    protected void SetupGameData(GameState gs)
    {
        IGameDataService gameService = gs.fact.Get<IGameDataService>();

        gs.data = new GameData();

        gameService.SetupGameData(gs);
    }

    protected void SetupUnityServices(GameState gs)
    {
        AddUnityService<ScreenService>(gs);
    }

    protected void AddUnityService<T>(GameState gs) where T :ServiceBehaviour
    {
        T service = GameObjectUtils.GetComponent<T>(gs.GetInitObject());
        if (service != null)
        {
            gs.fact.Set(service);
        }
    }

}
