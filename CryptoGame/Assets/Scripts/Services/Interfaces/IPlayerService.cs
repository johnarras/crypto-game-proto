using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IPlayerService : IService
{
    Player AddPlayer(GameState gs, string walletAddres, string name);
    long GetPlayerIdFromWallet(GameState gs, string wallet);
    string GetWalletFromPlayerId(GameState gs, long playerId);
    string PrintOverview(GameState gs, Player player);
    Player GetPlayerFromId(GameState gs, long playerId);
}
