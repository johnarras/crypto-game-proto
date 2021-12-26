using System;
using System.Collections.Generic;
using System.Text;

namespace GetDogeOpReturn.Entities.Blocks
{
    [Serializable]
    public class InnerBlock
    {
        public long block_no { get; set; }
        public List<string> txs { get; set; }
    }
}
