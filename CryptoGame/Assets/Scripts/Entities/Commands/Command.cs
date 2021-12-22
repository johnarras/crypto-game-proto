using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Command
{
    public string ToWallet { get; set; }
    public string FromWallet { get; set; }
    public double Quantity { get; set; }
    public string CommandId { get; set; }
    public string Data { get; set; }
}
