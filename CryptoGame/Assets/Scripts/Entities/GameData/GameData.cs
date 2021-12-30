using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameData
{
    public List<BuildingType> Buildings { get; set; }

    public List<CurrencyType> Currencies { get; set; }


    public GameData()
    {
        Buildings = new List<BuildingType>();
        Currencies = new List<CurrencyType>();
        _lookup = new IndexedDataItemLookup(this);
    }




    protected IndexedDataItemLookup _lookup = null;
    virtual public object Get(Type type, int id) { return _lookup.Get(type, id); }
    virtual public T Get<T>(long id) where T : class, IIndexedGameItem { return _lookup.Get<T>(id); }
    virtual public T Get<T>(ulong id) where T : class, IIndexedGameItem { return _lookup.Get<T>(id); }
    virtual public List<T> GetList<T>() where T : class, IIndexedGameItem { return _lookup.GetList<T>(); }
    virtual public void ClearIndex() { _lookup.Clear(); }



}