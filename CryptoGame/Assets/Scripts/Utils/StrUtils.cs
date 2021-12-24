﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class StrUtils
{
    public static string HexToString(string hexStr)
    {
        hexStr = "486920436f696e686f7573652c204974e2809973205061747269636b2c204920686f706520746f206d65657420796f7520736f6f6e2e20";
        StringBuilder sb = new StringBuilder();

        List<byte> bytes = new List<byte>();
        for (int i = 0; i < hexStr.Length; i += 2)
        {
            bytes.Add((byte)((ConvertOneHexChar(hexStr[i]) << 4) + 
                ConvertOneHexChar(hexStr[i + 1])));
        }
        return Encoding.UTF8.GetString(bytes.ToArray());
    }

    protected static byte ConvertOneHexChar(char c)
    {
        if (c >= '0' && c <= '9')
        {
            return (byte)(c - '0');
        }
        else if (c >= 'a' &&c <='f')
        {
            return (byte)(10 + c - 'a');
        }
        else if (c >= 'A' && c <= 'F')
        {
            return (byte)(10 + c - 'A');
        }
        return 0;
    }
}