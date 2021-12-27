using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ILandService : IService
{
    LandData BuyLand(GameState gs, long playerId);
}
