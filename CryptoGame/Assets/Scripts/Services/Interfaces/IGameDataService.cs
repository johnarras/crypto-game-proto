using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IGameDataService : IService
{
    void SetupGameData(GameState gs);
}