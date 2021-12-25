using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

// opreturn.net
public class SetupWorldData : IBlockProcessor
{

    protected const string OP_RETURN = "OP_RETURN";
    protected const string blockPrefixURL = "https://chain.so/api/v2/get_block/DOGE/";
    protected const string transPrefixURL = "https://chain.so/api/v2/get_tx/DOGE/";
    public virtual IEnumerator Process(GameState gs, PlayerState ps)
    {
        if (ps.world != null && ps.world.BlockId == gs.processing.BlockId - 1)
        {
            // Ok world from last turn.
            yield break;
        }

        string worldId = WorldData.GenerateWorldId(gs.processing.ToWallet, gs.processing.BlockId - 1);

        WorldData worldData = gs.repo.Load<WorldData>(worldId);

        // If previous block doesn't exist, then restart the sequence.
        if (worldData == null)
        {
            gs.processing.BlockId = BlockIdList.MinBlock;
            worldData = new WorldData()
            {
                ToWallet = gs.processing.ToWallet,
                BlockId = gs.processing.BlockId,
            };
        }
        else
        {
            worldData.BlockId = gs.processing.BlockId;
        }
        ps.world = worldData;
        yield break;
    }
}
