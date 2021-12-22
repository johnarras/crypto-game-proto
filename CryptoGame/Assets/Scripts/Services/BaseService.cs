using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class BaseService : IService
{
    public abstract long GetMinBlockId();

    public virtual void Setup(GameState gs, PlayerState ps)
    {

    }
}