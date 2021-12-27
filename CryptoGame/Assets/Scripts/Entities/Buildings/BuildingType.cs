using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BuildingType : IIndexedGameItem
{

    public const long TownHall = 1;
    public const long Farm = 2;
    public const long Blacksmith = 3;
    public const long Temple = 4;
    public const long Theatre = 5;
    public const long Library = 6;

    public long Id { get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }
    public string Icon { get; set; }
    public string Art { get; set; }

    public BuildingSet Prerequisites { get; set; }

    public CurrencySet BuildCosts { get; set; }
    public CurrencySet UpkeepCosts { get; set; }
    public CurrencySet Production { get; set; }
}