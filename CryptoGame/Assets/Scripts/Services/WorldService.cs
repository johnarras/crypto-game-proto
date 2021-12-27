using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WorldService : BaseService, IWorldService
{
    public override long GetMinBlockId() { return BlockConstants.MinBlock; }

   
}