using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LastBlockCompleted : IStringId
{
    public string Id { get; set; }
    public long LastBlockId { get; set; }
}