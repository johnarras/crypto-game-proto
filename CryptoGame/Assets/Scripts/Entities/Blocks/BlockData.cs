using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class BlockData : IStringId
{
    public string Id
    {
        get
        {
            return BlockId.ToString();
        }
        set { }
    }
    public long BlockId { get; set; }
    public List<Command> Commands {get;set;}

    public BlockData()
    {
        Commands = new List<Command>();
    }
}