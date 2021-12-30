using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public class IndexedDataItemLookup
{

    protected object parentObject = null;
    private Dictionary<Type, Dictionary<long, IIndexedGameItem>> _cache = null;
    private Dictionary<Type, object> _listCache = null;
    private Object _cacheLock = new object();
    public IndexedDataItemLookup(object parent)
    {
        parentObject = parent;
    }

    public void Clear()
    {
        _cache = null;
        _listCache = null;
    }

    virtual public object Get(Type type, long id)
    {
        if (type == null)
        {
            return null;
        }

        if (_cache == null)
        {
            SetupCache();
        }

        if (!_cache.ContainsKey(type))
        {
            return null;
        }
        Dictionary<long, IIndexedGameItem> dict = _cache[type];
        if (!dict.ContainsKey(id))
        {
            return null;
        }
        return dict[id];

    }



    protected void SetupCache()
    {
        if (_cache != null)
        {
            return;
        }

        lock (_cacheLock)
        {
            if (_cache != null)
            {
                return;
            }

            Dictionary<Type, Dictionary<long, IIndexedGameItem>> tempCache = new Dictionary<Type, Dictionary<long, IIndexedGameItem>>();
            Dictionary<Type, object> tempListCache = new Dictionary<Type, object>();
            string indexInterfacename = typeof(IIndexedGameItem).Name;
            PropertyInfo[] props = parentObject.GetType().GetProperties();
            for (int p = 0; p < props.Length; p++)
            {
                PropertyInfo prop = props[p];

                Type ptype = prop.PropertyType;


                if (!ptype.IsGenericType ||
                    (ptype.FullName.IndexOf("System.Collections.Generic.List`1") != 0 &&
                    ptype.FullName.IndexOf("System.Collections.Generic.List`1") != 0))
                {
                    continue;
                }



                Type[] args = ptype.GetGenericArguments();
                if (args == null || args.Length != 1)
                {
                    continue;
                }

                if (!args[0].IsClass)
                {
                    continue;
                }

                Type inter = args[0].GetInterface(indexInterfacename);
                if (inter == null)
                {
                    continue;
                }


                object listVal = EntityUtils.GetObjectValue(parentObject, prop);
                if (listVal == null)
                {
                    continue;
                }
                tempCache[args[0]] = new Dictionary<long, IIndexedGameItem>();


                object countObj = EntityUtils.GetObjectValue(listVal, "Count");
                if (countObj == null)
                {
                    continue;
                }
                int count = 0;

                Int32.TryParse(countObj.ToString(), out count);
                if (count < 1)
                {
                    continue;
                }

                Type ltype = listVal.GetType();

                tempListCache[args[0]] = listVal;
                MethodInfo arrMethod = ltype.GetMethod("ToArray");
                if (arrMethod == null)
                {
                    continue;
                }

                object arr2 = arrMethod.Invoke(listVal, new object[0]);

                if (arr2 == null)
                {
                    continue;
                }

                Type arrType = arr2.GetType();

                MethodInfo readMethod = arrType.GetMethod("GetValue", new[] { typeof(Int32) });

                if (readMethod == null)
                {
                    continue;
                }


                for (int c = 0; c < count; c++)
                {
                    IIndexedGameItem cval = readMethod.Invoke(arr2, new object[] { c }) as IIndexedGameItem;
                    if (cval != null)
                    {
                        tempCache[args[0]][cval.Id] = cval;
                    }
                }

            }
            _cache = tempCache;
        }
    }



    virtual public T Get<T>(long id) where T : class, IIndexedGameItem
    {
        Type type = typeof(T);
        return Get(type, id) as T;
    }

    virtual public T Get<T>(ulong id) where T : class, IIndexedGameItem
    {
        return Get<T>((int)id);
    }

    public virtual List<T> GetList<T>()
    {
        if (_cache == null)
        {
            SetupCache();
        }
        Type t = typeof(T);
        if (!_listCache.ContainsKey(t))
        {
            return new List<T>();
        }

        List<T> retval = _listCache[t] as List<T>;

        if (retval == null)
        {
            retval = new List<T>();
        }
        return retval;
    }
}