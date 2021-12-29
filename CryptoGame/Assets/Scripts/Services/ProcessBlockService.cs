using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ProcessBlockService : BaseService, IProcessBlockService
{
    public override long GetMinBlockId()
    {
        return BlockConstants.MinBlock;
    }

    public override void Setup(GameState gs)
    {
        base.Setup(gs);
        // Refine here.
    }


    protected List<IBlockProcessor> blockProcessors = new List<IBlockProcessor>()
    {
        new LoadBlockData(),
        new ResetRandom(),
        new LoadCurrentCommands(),
        new ProcessCommandsOnTick(),
        new UpdateEconOnTick(),
        new UpdateCurrentBlock(),
    };


    public virtual IEnumerator Process(GameState gs)
    {
        long startBlock = gs.world.BlockId;
        while (string.IsNullOrEmpty(gs.processMessage))
        {

            foreach (IBlockProcessor processor in blockProcessors)
            {
                yield return processor.Process(gs);
                if (!string.IsNullOrEmpty(gs.processMessage))
                {
                    break;
                }
            }

            if (gs.world.BlockId % 100 == 0)
            {
                Debug.Log("Processed block " + gs.world.BlockId);
            }
            yield return null;
        }
        long totalBlocks = gs.world.BlockId - startBlock;
        Debug.Log("EndBlock: " + gs.world.BlockId + " TotalBlocks: " + totalBlocks +
            " Msg: " + gs.processMessage);


        yield break;
    }
}
