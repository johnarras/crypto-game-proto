using GetDogeOpReturn.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GetDogeOpReturn.Repos
{
    public interface IRepository<T> where T : IStringId
    { 
        Task<T> Load(String id);
        Task<bool> Save(T t, string id = "");
        Task<bool> Delete(T t);
    }
}
