using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UpdateCurrentBlock : IBlockProcessor
{
    public IEnumerator Process(GameState gs, PlayerState ps)
    {

        if (string.IsNullOrEmpty(gs.processing.BlockError))
        {
            gs.repo.Save(ps.world, "World" + gs.processing.BlockId);
            gs.processing.BlockId++;
        }

        yield break;
    }
}