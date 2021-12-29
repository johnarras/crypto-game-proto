using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BuyCommandHandler : ICommandHandler
{
    public long GetMinBlockId() { return BlockConstants.V1; }

    public string GetKey() { return CommandList.Buy; }
   
    public IEnumerator Process (GameState gs,  Command comm)
    {
        IPlayerService playerService = gs.fact.Get<IPlayerService>();

        long playerId = playerService.GetPlayerIdFromWallet(gs, comm.FromWallet);

        Player player = gs.world.Players.FirstOrDefault(x => x.Id == playerId);

        if (comm.Args == "Land")
        {
            ILandService landService = gs.fact.Get<ILandService>();

            landService.AddLand(gs, player.Id);
        }
        yield break;
    }
}