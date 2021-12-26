using GetDogeOpReturn.Entities.Commands;
using GetDogeOpReturn.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GetDogeOpReturn.Entities.Blocks
{
    [Serializable]
    public class BlockData : IStringId
    {
        public string Id { get; set; }
        public long BlockId { get; set; }
        public List<Command> Commands { get; set; }

        public BlockData()
        {
            Commands = new List<Command>();
        }


        public static string GetFileNameFromBlockId(long blockId)
        {
            return blockId.ToString();
        }
    }

    
}
