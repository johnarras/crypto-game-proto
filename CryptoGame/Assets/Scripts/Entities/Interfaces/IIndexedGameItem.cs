using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IIndexedGameItem : INameId
{
    public string Desc { get; set; }
    public string Icon { get; set; }
    public string Art { get; set; }
}