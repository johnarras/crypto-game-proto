using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ProcessCommandsOnTick : IBlockProcessor
{
    public IEnumerator Process(GameState gs)
    {
        if (!gs.world.CanProcessTick())
        {
            yield break;
        }

        if (gs.world.PendingCommands.Count < 1)
        {
            yield break;
        }

        CommandHandlerService commandHandlerService = gs.fact.Get<CommandHandlerService>();

        yield return commandHandlerService.ProcessCommands(gs, gs.world.PendingCommands);

        gs.world.PendingCommands = new List<Command>();

        yield break;
    }
}