using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class CurrentBlockStatus : IStringId
{
    public string Id { get; set; }
    public long CurrDownloadBlock { get; set; }
    public long CurrProcessBlock { get; set; }
    public long MaxProcessBlock { get; set; }
}