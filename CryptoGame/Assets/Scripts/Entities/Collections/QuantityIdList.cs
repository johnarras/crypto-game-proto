using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class QuantityIdList<T> where T : class, IQuantityId, new()
{
    public List<T> _data { get; set; }

    public T GetItem(long id)
    {

        if (_data == null) _data = new List<T>();

        T item = _data.FirstOrDefault(x => x.Id == id);
        if (item == null)
        {
            item = new T()
            {
                Id = id,
            };
            _data.Add(item);
        }
        return item;
    }

    public void Add(long id, long quantity)
    {
        GetItem(id).Quantity += quantity;
    }

    public void Set(long Id, long quantity)
    {
        GetItem(Id).Quantity = quantity;
    }

    public long Get(long Id)
    {
        return GetItem(Id).Quantity;
    }

    public List<T> GetData()
    {
        return _data;
    }
}