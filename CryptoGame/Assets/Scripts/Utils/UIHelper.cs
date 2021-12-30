using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

public class UIHelper
{
    public static void SetText(Text text, string msg)
    {
        if (text != null)
        {
            text.text = msg;
        }
    }
}