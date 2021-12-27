using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IPlayerService : IService
{
    Player AddPlayer(GameState gs, string name);
}
