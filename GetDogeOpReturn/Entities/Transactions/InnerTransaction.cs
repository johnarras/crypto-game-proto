using System;
using System.Collections.Generic;
using System.Text;

namespace GetDogeOpReturn.Entities.Transactions
{
    [Serializable]
    public class InnerTransaction
    {
        public List<TransactionIO> inputs;
        public List<TransactionIO> outputs;
    }
}
