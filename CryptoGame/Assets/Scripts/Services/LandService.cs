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


            if (gs.rand.NextDouble() < 0.5f)
            {
                land.Production.Add(ctype, 1);
            }


            if (gs.rand.NextDouble() < 0.1f)
            {
                land.Production.Add(ctype, 1);
            }
        }

        return land;
    }

}
