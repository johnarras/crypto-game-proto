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
        return BlockIdList.MinBlock;
    }

    public override void Setup(GameState gs, PlayerState ps)
    {
        base.Setup(gs, ps);
        // Refine here.
    }


    protected List<IBlockProcessor> blockProcessors = new List<IBlockProcessor>()
    {
        new SetupWorldData(),
        new LoadBlockData(),
        new UpdateEcon(),
        new UpdateCurrentBlock(),
    };


    public virtual IEnumerator Process(GameState gs, PlayerState ps)
    {

        while (string.IsNullOrEmpty(gs.processing.BlockError))
        {

            foreach (IBlockProcessor processor in blockProcessors)
            {
                yield return processor.Process(gs, ps);
            }


            Debug.Log("Block: " + gs.processing.BlockId + " Error: " + 
                gs.processing.BlockError);
            yield return null;
        }


        yield break;
    }
}
