using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;

public class DownloadNewBlock : IBlockProcessor
{

    public virtual IEnumerator Process(GameState gs, PlayerState ps)
    {
        long blockId = gs.processing.BlockId;


        BlockData currentData = gs.repo.Load<BlockData>(blockId.ToString());

        if (currentData == null)
        {
            // Download from web.

            currentData = new BlockData()
            {
                BlockId = blockId,
            };

            for (int i = 0; i < 3; i++)
            {
                currentData.Commands.Add(new Command()
                {
                    ToWallet = "123",
                    CommandId = CommandList.Create,
                    Data="Char" + DateTime.UtcNow.Ticks,
                    FromWallet = "345" + i,
                    Quantity = 1+i,
                }); 
            }
        }


        yield break;
    }
}