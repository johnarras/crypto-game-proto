using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LandData : IId
{
    public long Id { get; set; }
    public long OwnerId { get; set; }
    public long Seed { get; set; }
    public long Population { get; set; }

    public CurrencySet Production { get; set; }
    public CurrencySet Storage { get; set; }

    public BuildingSet Buildings { get; set; }

    public LandData()
    {
        Production = new CurrencySet();
        Storage = new CurrencySet();
        Buildings = new BuildingSet();
        Population = 1;
    }
}
