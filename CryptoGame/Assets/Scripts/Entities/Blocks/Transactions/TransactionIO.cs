using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[Serializable]
public class TransactionIO
{
    public long pos;
    public double value;
    public string address;
    public ScriptPubKey scriptPubKey;
    public ScriptPubKey scriptSig;
}