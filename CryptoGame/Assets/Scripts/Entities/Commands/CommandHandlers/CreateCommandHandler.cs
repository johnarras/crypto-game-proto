using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CreateCommandHandler : ICommandHandler
{
    public long GetMinBlockId() { return BlockIdList.V1; }

    public string GetKey() { return CommandList.Create; }
   
    public IEnumerator Process (GameState gs,  Command comm)
    {
        IPlayerService playerService = gs.fact.Get<IPlayerService>();

        playerService.AddPlayer(gs, comm.Args);

        yield break;
    }
}