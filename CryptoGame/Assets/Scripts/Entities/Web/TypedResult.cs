using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TypedResult<T>
{
    public bool Success;
    public T Data;

    public bool IsOk()
    {
        return Success && Data != null;
    }
}