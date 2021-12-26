using GetDogeOpReturn.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GetDogeOpReturn.Entities.Blocks
{
    [Serializable]
    public class BlockList : IStringId
    {
        public string Id { get; set; }
        public List<long> BlockIds { get; set; }

        public BlockList()
        {
            BlockIds = new List<long>();
        }
    }
}
