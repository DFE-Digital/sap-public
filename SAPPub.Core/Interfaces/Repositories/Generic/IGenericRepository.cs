using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Interfaces.Repositories.Generic
{
    public interface IGenericRepository<T>
    {
        //T? Read(string Id);
        IEnumerable<T>? ReadAll();
    }

    public interface IGenericCRUDRepository<T>
    {
        bool Create(T entity);
        T? Read(Guid Id);
        T? Read(string Id);
        IEnumerable<T>? ReadAll();
        IEnumerable<T>? Query(string query, T entity);
        bool Update(T entity);
        //bool Delete(T entity);

    }

}
