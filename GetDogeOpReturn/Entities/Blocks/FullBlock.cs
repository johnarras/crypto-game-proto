using System;
using System.Collections.Generic;
using System.Text;

namespace GetDogeOpReturn.Entities.Blocks
{
    [Serializable]
    public class FullBlock
    {
        public InnerBlock data { get; set; }
    }
}
