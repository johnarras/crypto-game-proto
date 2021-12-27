using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public class ReflectionService : IService
{
    virtual public long GetMinBlockId() { return BlockConstants.V1; }


    private List<Assembly> _assemblies = new List<Assembly>();
    private List<Type> _types = new List<Type>();
    public void Setup(GameState gs)
    {
        _assemblies = new List<Assembly>();
        _assemblies.Add(Assembly.GetAssembly(GetType()));


        foreach(Assembly assembly in _assemblies)
        {
            _types.AddRange(assembly.GetExportedTypes());
        }
    }

    public Dictionary<K, List<T>> SetupDictionary<K, T>() where T : IDictItem<K>
    {
        Dictionary<K, List<T>> dict = new Dictionary<K, List<T>>();
        Type ttype = typeof(T);

        foreach (Type t in _types)
        {
            if (!t.IsClass)
            {
                continue;
            }

            if (t.IsAbstract)
            {
                continue;
            }

            Type inter = t.GetInterface(ttype.Name);
            if (inter == null)
            {
                continue;
            }

            T inst = (T)Activator.CreateInstance(t);

            if (inst == null || inst.GetKey() == null)
            {
                continue;
            }

            if (!dict.ContainsKey(inst.GetKey()))
            {
                dict[inst.GetKey()] = new List<T>();
            }

            dict[inst.GetKey()].Add(inst);
        }

        List<K> keyList = new List<K>(dict.Keys);
        foreach (K key in keyList)
        {
            dict[key] = dict[key].OrderBy(x => x.GetMinBlockId()).ToList();
        }

        return dict;
    }


    public List<Type> GetTypesImplementing(Type interfaceType)
    {

        List<Type> retval = new List<Type>();
        if (interfaceType == null || !interfaceType.IsInterface)
        {
            return retval;
        }
        if (!interfaceType.IsInterface)
        {
            return retval;
        }

        foreach (Type t in _types)
        {
            if (!t.IsClass)
            {
                continue;
            }

            if (t.IsAbstract)
            {
                continue;
            }

            if (t.IsGenericType)
            {
                continue;
            }

            Type inter = t.GetInterface(interfaceType.Name);
            if (inter == null)
            {
                continue;
            }
            retval.Add(t);
        }
        return retval;
    }
}