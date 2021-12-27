using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerService : BaseService, IPlayerService
{
    public override long GetMinBlockId() { return BlockIdList.MinBlock; }

    public virtual Player AddPlayer(GameState gs, string name)
    {
        long maxId = 0;

        if (gs.world.Players.Count > 0)
        {
            maxId = gs.world.Players.Max(x => x.Id);
        }

            

        Player player = new Player()
        {
            Id = maxId + 1,
            Name = name,
        };

        if (string.IsNullOrEmpty(name))
        {
            player.Name = "Player" + player.Id;
        }
        return player;
    }
}
