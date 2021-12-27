using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class Command
{
    public string ToWallet { get; set; }
    public string FromWallet { get; set; }
    public double Quantity { get; set; }
    public string RawCommand { get; set; }
    public string DecodedCommand { get; set; }
    public long BlockId { get; set; }  
    public string CommandId { get; set; }
    public string Args { get; set; }
}
