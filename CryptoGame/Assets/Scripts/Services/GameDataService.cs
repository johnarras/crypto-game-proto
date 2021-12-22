using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GameDataService : IGameDataService
{
    public long GetMinBlockId() { return BlockIdList.V1; }

    public void Setup(GameState gs, PlayerState ps)
    {

    }

    public void SetupGameData(GameState gs)
    {
        gs.data = new GameData();

        ReflectionService reflectionService = gs.fact.Get<ReflectionService>();

    }
}