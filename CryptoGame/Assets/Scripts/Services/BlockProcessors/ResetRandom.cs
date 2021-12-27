using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ResetRandom : IBlockProcessor
{
    public IEnumerator Process(GameState gs)
    {
        gs.rand = new MyRandom(gs.world.GetSeed());
        yield break;
    }
}