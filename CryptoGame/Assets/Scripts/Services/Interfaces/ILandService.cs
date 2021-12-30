using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ILandService : IService
{
    LandData AddLand(GameState gs, long playerId);

    long GetFoodToGrowPopulation(long currentPopulation);

    string PrintOverview(GameState gs, LandData land);
}
