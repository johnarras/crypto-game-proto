using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IProcessBlockService : IService
{
    IEnumerator Process(GameState gs, PlayerState ps);
}
