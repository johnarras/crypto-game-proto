using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ProcessingData
{
    public string ToWallet { get; set; }
    public long BlockId { get; set; }
    public string BlockError { get; set; }
    public bool DidDownloadBlocks { get; set; }
}