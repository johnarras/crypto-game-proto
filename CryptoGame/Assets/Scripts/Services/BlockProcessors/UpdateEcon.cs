using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UpdateEcon : IBlockProcessor
{
    public IEnumerator Process(GameState gs, PlayerState ps)
    {
        IUpdateEconService econService = gs.fact.Get<IUpdateEconService>();
        econService.Process(gs, ps);

        yield break;
    }
}