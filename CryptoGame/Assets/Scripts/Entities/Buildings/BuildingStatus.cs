using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BuildingStatus : IQuantityId
{
    public long Id { get; set; }
    public long Quantity { get; set; }
    public long Level { get; set; }
}
