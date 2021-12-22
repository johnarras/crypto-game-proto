using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WorldData : IStringId
{
    public static string GenerateWorldId(string toWallet, long gameId, long blockId)
    {
        return toWallet + "g" + gameId + "b" + blockId;
    }
    public string Id
    {
        get
        {
            return GenerateWorldId(ToWallet, GameId, BlockId);
        }

        set
        {

        }
    }
    public string ToWallet { get; set; }
    public long GameId { get; set; }
    public long BlockId { get; set; }


    public List<LandData> Lands { get; set; }

    public List<Player> Players { get; set; }


    public WorldData()
    {
        Lands = new List<LandData>();
        Players = new List<Player>();
    }

}