using System;
using System.Collections.Generic;
using System.Text;

namespace GetDogeOpReturn.Entities.Transactions
{
    [Serializable]
    public class TransactionIO
    {
        public long output_no;
        public double value;
        public string address;
        public string script;
    }
}
