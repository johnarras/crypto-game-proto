using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LandService : BaseService, ILandService
{
    public override long GetMinBlockId() { return BlockConstants.MinBlock; }

    public LandData AddLand(GameState gs, long playerId)
    {
        return null;
    }

}
