using System;
using System.Collections.Generic;
using System.Text;

namespace GetDogeOpReturn.Entities.Network
{
    [Serializable]
    public class FullNetwork
    {
        public InnerNetwork data { get; set; }
    }
}
