using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface ICommandHandler : IDictItem<string>
{
    public IEnumerator Process(GameState gs,  Command data);
}