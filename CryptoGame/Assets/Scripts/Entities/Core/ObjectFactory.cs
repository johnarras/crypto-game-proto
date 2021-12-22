using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Linq;

/// <summary>
/// This is a DI/IOC object that I implemented myself to 
/// be able to have more than one of these within a given program for 
/// different contexts (Such as in an editor with multiple games or
/// multiple environments open)
/// </summary>
public class ObjectFactory
{

    private GameState _gs = null;
    public ObjectFactory (GameState gs)
    {
        _gs = gs;
    }

    /// <summary>
    /// Internal storage indexed by type
    /// </summary>
    private Dictionary<Type, List<IService>> dict = new Dictionary<Type, List<IService>>();

    /// <summary>
    /// Returns an instance of type T
    /// </summary>
    /// <typeparam name="T">The type to be returned. An IFoo can return a FooImpl as long as FooImpl is IFoo</typeparam>
    /// <returns>An object of Type T</returns>
    public T Get<T>() where T : IService
    {
        if (!dict.ContainsKey(typeof(T)))
        {
            return default(T);
        }
        return (T)dict[typeof(T)].LastOrDefault(x => x.GetMinBlockId() <= _gs.processing.BlockId);
    }

    public List<IService> GetAll()
    {
        List<IService> retval = new List<IService>();

        foreach (Type t in dict.Keys)
        {
            retval.AddRange(dict[t]);
        }

        return retval;
    }

    /// <summary>
    /// Reset the internal dictionaries
    /// </summary>
    public void Clear()
    {
        dict = new Dictionary<Type, List<IService>>();
    }
    /// <summary>
    /// Add an object to the dictionaries
    /// </summary>
    /// <typeparam name="T">The type to add</typeparam>
    /// <param name="obj">The object of type T to add</param>
    public void Set<T>(T obj) where T : IService
    {
        if (obj == null)
        {
            return;
        }
        if (!dict.ContainsKey(typeof(T)))
        {
            dict[typeof(T)] = new List<IService>();
        }
        dict[typeof(T)].Add(obj);

        dict[typeof(T)] = dict[typeof(T)].OrderBy(x => x.GetMinBlockId()).ToList();
    }
}
