using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class ServiceBehaviour : BaseBehaviour, IService
{
    public abstract long GetMinBlockId();

    public abstract void Setup(GameState gs);
}