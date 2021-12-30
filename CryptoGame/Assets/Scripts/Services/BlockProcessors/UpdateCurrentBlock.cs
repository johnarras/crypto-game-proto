using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UpdateCurrentBlock : IBlockProcessor
{
    public IEnumerator Process(GameState gs)
    {
        if (!string.IsNullOrEmpty(gs.processMessage))
        {
            yield break;
        }
        gs.world.BlockId++;
        gs.repo.Save(gs.world);
        gs.dispatcher.DispatchEvent(gs,new ShowOverview());
        if (gs.world.BlockId > gs.maxProcessBlock)
        {
            gs.processMessage = "End of Blocks";
        }
        yield break;
    }
}