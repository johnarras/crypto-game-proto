using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ProcessBlockService : BaseService, IProcessBlockService
{
    public override long GetMinBlockId()
    {
        return BlockIdList.MinBlock;
    }

    public override void Setup(GameState gs, PlayerState ps)
    {
        base.Setup(gs, ps);
        // Refine here.
    }


    protected List<IBlockProcessor> blockProcessors = new List<IBlockProcessor>()
    {
        new DownloadNewBlock(),
    };


    public virtual IEnumerator Process(GameState gs, PlayerState ps)
    {

        foreach (IBlockProcessor processor in blockProcessors)
        {
            yield return processor.Process(gs, ps);
        }


        yield break;
    }
}
