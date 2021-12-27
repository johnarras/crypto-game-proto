using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LoadCurrentCommands : IBlockProcessor
{
    public virtual IEnumerator Process(GameState gs)
    {
        BlockData bdata = gs.repo.Load<BlockData>(gs.world.BlockId.ToString());

        if (bdata == null)
        {
            yield break;
        }

        List<Command> validCommands = bdata.Commands.Where(x => x.ToWallet == gs.toWallet).ToList();


        foreach (Command comm in validCommands)
        {
            string[] words = comm.DecodedCommand.Split(' ');
            if (words.Length < 1)
            {
                continue;
            }

            comm.CommandId = words[0];
            if (words.Length > 1)
            {
                comm.Args = comm.DecodedCommand.Substring(words[0].Length).Trim();
            }

            gs.world.PendingCommands.Add(comm);
        }

        yield break;
    }
}