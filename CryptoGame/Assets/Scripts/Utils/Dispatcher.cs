using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Should probably use 2 generic params, but that's more complicated for now.
public delegate object GameAction<T>(GameState gs, T t);


public class Dispatcher
{
    private Dictionary<Type, object> _dict = new Dictionary<Type, object>();


    public void AddEvent<T>(GameAction<T> action) where T : class
    {
        if (!_dict.ContainsKey(typeof(T)))
        {
            _dict[typeof(T)] = new List<GameAction<T>>();
        }

        List<GameAction<T>> list = (List<GameAction<T>>)_dict[typeof(T)];
        list.Add(action);
    }

    public void RemoveEvent<T>(GameAction<T> action) where T : class
    {
        if (!_dict.ContainsKey(typeof(T)))
        {
            return;
        }
        List<GameAction<T>> list = (List<GameAction<T>>)_dict[typeof(T)];
        if (list.Contains(action))
        {
            list.Remove(action);
        }
    }

    public object DispatchEvent<T>(GameState gs, T actionParam) where T : class
    {
        if (!_dict.ContainsKey(typeof(T)))
        {
            return null;
        }

        object retval = null;


        List<GameAction<T>> list = (List<GameAction<T>>)_dict[typeof(T)];

        foreach (GameAction<T> gameAction in list)
        {
            object tempVal = gameAction(gs, actionParam);
            if (tempVal != null && retval == null)
            {
                retval = tempVal;
            }
        }

        return retval;
    }

}