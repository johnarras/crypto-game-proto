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

        while (string.IsNullOrEmpty(gs.processError))
        {

            foreach (IBlockProcessor processor in blockProcessors)
            {
                yield return processor.Process(gs);
                if (!string.IsNullOrEmpty(gs.processError))
                {
                    yield break;
                }
            }

            Debug.Log("Block: " + gs.world.BlockId + " " + gs.processError);
            yield return null;
        }


        yield break;
    }
}
