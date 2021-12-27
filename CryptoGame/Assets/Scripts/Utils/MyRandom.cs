using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MyRandom
{

    private System.Random _rand = null;

    public MyRandom(int seed)
    {
        _rand = new System.Random(seed);
    }

    public int Next()
    {
        return _rand.Next();
    }

    public double NextDouble()
    {
        return _rand.NextDouble();
    }

    public long NextLong()
    {
        byte[] bytes = new byte[8];

        _rand.NextBytes(bytes);

        long value = BitConverter.ToInt64(bytes, 0);

        return value >= 0 ? value : -value;
    }


    public int Next(int maxValue)
    {
        if (maxValue < 1)
        {
            return 0;
        }

        return Next() % maxValue;
    }

    public long NextLong(long maxValue)
    {
        if (maxValue < 1)
        {
            return 0;
        }

        return NextLong() % maxValue;
    }
        
    /// <summary>
        /// Returns a MyRandom number between minValue and maxValue-1
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
    public int Next(int minValue, int maxValue)
    {
        return minValue + Next(maxValue - minValue);
    }

    public long NextLong(long minValue, long maxValue)
    {
        return minValue + NextLong(maxValue - minValue);
    }
}
