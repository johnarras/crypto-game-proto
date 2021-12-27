using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LandService : BaseService, ILandService
{
    public override long GetMinBlockId() { return BlockIdList.MinBlock; }

    public LandData BuyLand(GameState gs, long playerId)
    {
        return null;
    }

}
