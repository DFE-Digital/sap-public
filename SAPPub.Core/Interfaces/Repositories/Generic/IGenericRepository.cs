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

}
