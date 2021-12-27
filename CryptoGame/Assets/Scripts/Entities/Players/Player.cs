using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Player : INameId
{
    public const long MissingPlayerId = 0;
    public const long StartId = 1;

    public long Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }

    public BuildingData Buildings { get; set; }

    public CurrencyData Currencies { get; set; }

    public Player()
    {
        Buildings = new BuildingData();
        Currencies = new CurrencyData();
    }


}
