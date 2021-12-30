using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WorldService : BaseService, IWorldService
{
    public override long GetMinBlockId() { return BlockConstants.MinBlock; }

   
    public string PrintOverview(GameState gs)
    {
        StringBuilder sb = new StringBuilder();
        IPlayerService playerService = gs.fact.Get<IPlayerService>();
        ILandService landService = gs.fact.Get<ILandService>();

        sb.Append(" PLAYERS: \n");
        foreach (Player player in gs.world.Players)
        {
            sb.Append(playerService.PrintOverview(gs, player));
        }

        sb.Append("\nLANDS:\n");

        foreach (LandData land in gs.world.Lands)
        {
            sb.Append(landService.PrintOverview(gs, land));
        }

        return sb.ToString();
    }
}