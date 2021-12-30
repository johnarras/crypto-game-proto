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

    string indent = "    ";
    public string PrintOverview(GameState gs, Player player)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("\nPlayer: " + player.Id + " -- " + player.Name + "\n");
        foreach (CurrencyQuantity cq in player.Currencies.GetData())
        {
            if (cq.Quantity != 0)
            {
                CurrencyType ctype = gs.data.Get<CurrencyType>(cq.Id);

                if (ctype == null)
                {
                    // Warn?
                    continue;
                }
                sb.Append(indent + ctype.Name + ": " + cq.Quantity + "\n");
            }
        }

        return sb.ToString();
    }

    public Player GetPlayerFromId(GameState gs, long playerId)
    {
        return gs.world.Players.FirstOrDefault(x => x.Id == playerId);
    }
}
