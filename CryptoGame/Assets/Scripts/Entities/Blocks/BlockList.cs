using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BlockList: IStringId
{
    public string Id { get; set; }
    public List<long> BlockIds { get; set; }


    public BlockList()
    {
        BlockIds = new List<long>();
    }
}