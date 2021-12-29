using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GameDataService : IGameDataService
{
    public long GetMinBlockId() { return BlockConstants.V1; }

    public void Setup(GameState gs)
    {

    }

    public void SetupGameData(GameState gs)
    {
        gs.data = new GameData();

        gs.data.Currencies.Add(new CurrencyType() { Id = CurrencyType.Gold, Name = "Gold" });
        gs.data.Currencies.Add(new CurrencyType() { Id = CurrencyType.Gems, Name = "Gems" });
        gs.data.Currencies.Add(new CurrencyType() { Id = CurrencyType.Food, Name = "Food" });
        gs.data.Currencies.Add(new CurrencyType() { Id = CurrencyType.Wood, Name = "Wood" });
        gs.data.Currencies.Add(new CurrencyType() { Id = CurrencyType.Stone, Name = "Stone" });
        gs.data.Currencies.Add(new CurrencyType() { Id = CurrencyType.Iron, Name = "Iron" });
        gs.data.Currencies.Add(new CurrencyType() { Id = CurrencyType.Religion, Name = "Religion" });
        gs.data.Currencies.Add(new CurrencyType() { Id = CurrencyType.Entertainment, Name = "Entertainment" });
        gs.data.Currencies.Add(new CurrencyType() { Id = CurrencyType.Science, Name = "Science" });
        gs.data.Currencies.Add(new CurrencyType() { Id = CurrencyType.Exp, Name = "Exp" });

    }
}