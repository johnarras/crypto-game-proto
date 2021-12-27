using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Not interfaced since it should not change.
public class CommandHandlerService : IService
{
    public virtual long GetMinBlockId() { return BlockConstants.V1; }


    private Dictionary<string, List<ICommandHandler>> _handlers = new Dictionary<string, List<ICommandHandler>>();
    public void Setup(GameState gs)
    {
        ReflectionService reflectionService = gs.fact.Get<ReflectionService>();

        _handlers = reflectionService.SetupDictionary<string, ICommandHandler>();

    }

    public ICommandHandler GetHandler(GameState gs, string commandId)
    {
        if (!_handlers.ContainsKey(commandId))
        {
            return null;
        }

        return _handlers[commandId].LastOrDefault(x => x.GetMinBlockId() <= gs.world.BlockId);
    }

    public IEnumerator ProcessCommands (GameState gs, List<Command> commands)
    {

        foreach (Command comm in commands)
        {
            ICommandHandler handler = GetHandler(gs, comm.CommandId);
            if (handler != null)
            {
                yield return handler.Process(gs, comm);
            }
        }


        yield break;
    }
}