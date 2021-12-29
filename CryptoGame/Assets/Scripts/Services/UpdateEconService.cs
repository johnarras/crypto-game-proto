using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class UpdateEconService : IUpdateEconService
{
    public long GetMinBlockId() { return BlockConstants.V1; }
    
    public void Setup(GameState gs)
    {

    }

    public void Process(GameState gs)
    {
        UpdatePlayers(gs);
        UpdateLands(gs);
    }

    protected void UpdateLands(GameState gs)
    {

        ILandService landService = gs.fact.Get<ILandService>();
        foreach (LandData landData in gs.world.Lands)
        {
            foreach (CurrencyQuantity prod in landData.Production.GetData())
            {
                landData.Storage.Add(prod.Id, prod.Quantity);
            }

            long foodToGrow = landService.GetFoodToGrowPopulation(landData.Population);

            // Only one growth per turn.
            if (landData.Storage.Get(CurrencyType.Food) >= foodToGrow)
            {
                landData.Population++;
                landData.Storage.Add(CurrencyType.Food, -foodToGrow);
            }
        }
    }

    protected void UpdatePlayers(GameState gs)
    {
        foreach (Player p in gs.world.Players)
        {
            p.Currencies.Add(CurrencyType.Exp, 1);
        }
    }
}