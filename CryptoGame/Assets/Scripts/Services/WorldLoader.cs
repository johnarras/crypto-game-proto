using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WorldLoader
{
    public const string WorldFilename = "World";
    public World LoadWorld(GameState gs)
    {
        string worldFilename = WorldFilename + "." + gs.toWallet;
        if (gs.world == null || gs.world.Id != worldFilename)
        {
            gs.world = gs.repo.Load<World>(worldFilename);

            if (gs.world == null)
            {
                gs.world = new World()
                {
                    Id = worldFilename,
                    BlockId = BlockConstants.MinBlock,
                };
                gs.repo.Save(gs.world);
            }
        }
        return gs.world;
    }
}