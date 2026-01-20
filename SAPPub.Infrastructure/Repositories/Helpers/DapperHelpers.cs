using SAPPub.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Infrastructure.Repositories.Helpers
{
    public class DapperHelpers
    {
        public static string GetQuery(Type entityName)
        {

            //return $"select * from v_{entityName.Name.ToLower()} Where \"PhaseOfEducationId\" = 4 LIMIT 100;";
            return $"select * from v_establishment Where \"PhaseOfEducationId\" = 4 LIMIT 100;";
        }
    }
}
