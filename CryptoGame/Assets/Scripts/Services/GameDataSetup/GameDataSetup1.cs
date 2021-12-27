using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameDataSetup1 : IGameDataSetup
{
    public long GetMinBlockId() { return BlockConstants.V1; }

    public void Setup (GameState gs)
    {
    }
}