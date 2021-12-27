using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public struct WorldStruct
{
    string ToWallet;
    long BlockId;

    public WorldStruct(string toWallet, long blockId)
    {
        ToWallet = toWallet;
        BlockId = blockId;
    }
}

public class World : IStringId
{

    public string Id { get; set; }
    public string ToWallet { get; set; }
    public long BlockId { get; set; }
    public long NumberVal { get; set; }

    public List<LandData> Lands { get; set; }

    public List<Player> Players { get; set; }

    public World()
    {
        Lands = new List<LandData>();
        Players = new List<Player>();
    }

    public int GetSeed()
    {
        WorldStruct ws = new WorldStruct(ToWallet, BlockId);
      
        Hash128 hash = new Hash128();
        HashUtilities.ComputeHash128(ref ws, ref hash);

        string firstStr = hash.ToString().Substring(0, 8);

        int value = Convert.ToInt32(firstStr, 16);

        return value;
    }

}