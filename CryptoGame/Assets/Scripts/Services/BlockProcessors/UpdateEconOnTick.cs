using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UpdateEconOnTick : IBlockProcessor
{
    public IEnumerator Process(GameState gs)
    {
        if (!gs.world.CanProcessTick())
        {
            yield break;
        }

        IUpdateEconService econService = gs.fact.Get<IUpdateEconService>();
        econService.Process(gs);

        yield break;
    }
}