using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameData
{
    public List<BuildingType> Buildings { get; set; }

    public List<CurrencyType> Currencies { get; set; }


    public GameData()
    {
        Buildings = new List<BuildingType>();
        Currencies = new List<CurrencyType>();
    }
}