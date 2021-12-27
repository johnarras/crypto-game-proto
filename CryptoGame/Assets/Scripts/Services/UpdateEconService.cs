using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class UpdateEconService : IUpdateEconService
{
    public long GetMinBlockId() { return BlockConstants.V1; }
    
    public void Setup(GameState gs)
    {

    }

    public void Process(GameState gs)
    {
        gs.world.NumberVal++;
    }
}