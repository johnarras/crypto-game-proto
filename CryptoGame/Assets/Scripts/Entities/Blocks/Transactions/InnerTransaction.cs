using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[Serializable]
public class InnerTransaction
{
    public List<TransactionIO> inputs;
    public List<TransactionIO> outputs;
}