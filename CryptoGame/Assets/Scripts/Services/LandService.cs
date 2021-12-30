using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LandService : BaseService, ILandService
{
    public override long GetMinBlockId() { return BlockConstants.MinBlock; }


    public long GetFoodToGrowPopulation(long currentPopulation)
    {
        if (currentPopulation < 1)
        {
            currentPopulation = 1;
        }
        return LandConstants.BaseFoodPerPopMult * currentPopulation +
            LandConstants.FoodPerPopSquareVal * currentPopulation * currentPopulation;
    }

    public LandData AddLand(GameState gs, long playerId)
    {
        long landId = 1;

        if (gs.world.Lands.Count > 0)
        {
            landId = gs.world.Lands.Max(x => x.Id) + 1;
        }

        LandData land = new LandData()
        {
            Id = landId,
            Seed = gs.rand.NextLong(),
            OwnerId = playerId,
        };

        gs.world.Lands.Add(land);

        List<long> baseCurrencies = new List<long>()
        {
            CurrencyType.Food,
            CurrencyType.Stone,
            CurrencyType.Wood,
            CurrencyType.Gold
        };

        foreach (long ctype in baseCurrencies)
        {
            land.Production.Add(ctype, 1);


            if (gs.rand.Next(0,10) < 5)
            {
                land.Production.Add(ctype, 1);
            }


            if (gs.rand.Next(0,10) < 1)
            {
                land.Production.Add(ctype, 1);
            }
        }

        return land;
    }

    public string PrintOverview(GameState gs, LandData land)
    {
        StringBuilder sb = new StringBuilder();


        IPlayerService playerService = gs.fact.Get<IPlayerService>();


        Player owner = playerService.GetPlayerFromId(gs, land.OwnerId);

        if (owner == null)
        {
            sb.Append("\nLand: [#" + land.Id + "] (Unowned)\n");
        }
        else
        {
            sb.Append("\nLand: [#" + land.Id + "] Owner: " + owner.Name + "[#" + owner.Id + "]\n");
        }

        List<long> productionIds = land.Production.GetData().Select(x=>x.Id).Distinct().ToList();

        List<long> storageIds = land.Storage.GetData().Select(x => x.Id).Distinct().ToList();

        List<long> allIds = productionIds.Concat(storageIds).Select(x => x).Distinct().OrderBy(x => x).ToList();

        foreach (long cid in allIds)
        {
            CurrencyType ctype = gs.data.Get<CurrencyType>(cid);
            if (ctype == null)
            {
                // Warn?
                continue;
            }

            long prodVal = land.Production.Get(cid);
            long storeVal = land.Storage.Get(cid);

            sb.Append(ctype.Name + "[#" + ctype.Id + "] -- Stored(Prod): " + storeVal + "(" + prodVal + ")\n");
        }

        return sb.ToString();
    }

}
