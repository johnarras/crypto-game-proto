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
        gs.world.BlockId++;
        gs.repo.Save(gs.world);
        yield break;
    }
}