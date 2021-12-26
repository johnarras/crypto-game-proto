using System;
using System.Collections.Generic;
using System.Text;

namespace GetDogeOpReturn.Utils
{
    public class ErrorUtils
    {
        public static void LogException(Exception e)
        {
            Console.WriteLine("Exception: " + e.Message + "\n" + e.StackTrace);
        }
    }
}
