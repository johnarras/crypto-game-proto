using GetDogeOpReturn.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GetDogeOpReturn.Entities.Blocks
{
    [Serializable]
    public class LastBlockSaved : IStringId
    {
        public string Id { get; set; }
        public long LastBlockId { get; set; }
    }
}
