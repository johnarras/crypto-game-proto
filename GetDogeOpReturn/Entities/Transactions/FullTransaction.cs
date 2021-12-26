using System;
using System.Collections.Generic;
using System.Text;

namespace GetDogeOpReturn.Entities.Transactions
{
    [Serializable]
    public class FullTransaction
    {
        public InnerTransaction data;
    }
}
