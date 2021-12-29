using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ILandService : IService
{
    LandData AddLand(GameState gs, long playerId);

    public long GetFoodToGrowPopulation(long currentPopulation);
}
