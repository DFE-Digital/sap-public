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
            return "SELECT table_name as EstablishmentName  FROM information_schema.tables WHERE table_schema='public'  AND table_type='BASE TABLE';";
        }
    }
}
