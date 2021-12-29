using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CurrencyType : IIndexedGameItem
{
    public const long Gold = 1;
    public const long Gems = 2;
    public const long Food = 3;
    public const long Wood = 4;
    public const long Stone = 5;
    public const long Iron = 6;
    public const long Religion = 7;
    public const long Entertainment = 8;
    public const long Science = 9;
    public const long Exp = 10;

    public long Id { get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }
    public string Icon { get; set; }
    public string Art { get; set; }
}