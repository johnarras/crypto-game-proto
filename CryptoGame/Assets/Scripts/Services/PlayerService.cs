using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerService : BaseService, IPlayerService
{
    public override long GetMinBlockId() { return BlockConstants.MinBlock; }

    public long GetPlayerIdFromWallet(GameState gs, string wallet)
    {
        Player player = gs.world.Players.FirstOrDefault(x => x.Address == wallet);

        if (player != null)
        {
            return player.Id;
        }
        return PlayerConstants.MissingPlayerId;
    }

    public string GetWalletFromPlayerId(GameState gs, long playerId)
    {
        Player player = gs.world.Players.FirstOrDefault(x => x.Id == playerId);
        if (player != null)
        {
            return player.Address;
        }
        return "";
    }

    public virtual Player AddPlayer(GameState gs, string walletAddress, string name)
    {
        long maxId = 0;


        Player oldPlayer = gs.world.Players.FirstOrDefault(x => x.Address == walletAddress);

        if (oldPlayer != null)
        {
            return oldPlayer;
        }
        if (gs.world.Players.Count > 0)
        {
            maxId = gs.world.Players.Max(x => x.Id);
        }

            

        Player player = new Player()
        {
            Id = maxId + 1,
            Name = name,
            Address = walletAddress,
        };

        if (string.IsNullOrEmpty(name))
        {
            player.Name = "Player" + player.Id;
        }
        gs.world.Players.Add(player);

        return player;
    }
}
