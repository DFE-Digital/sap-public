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

            //return $"select * from sappub_test.v_{entityName.Name.ToLower()} Where \"PhaseOfEducationId\" = 4 LIMIT 100;";
            //return "SELECT * FROM public.raw_edubasealldata2025_4db2d83a LIMIT 100;";
            return "SELECT *  FROM information_schema.tables where table_type='BASE TABLE' and table_schema != 'pg_catalog';";
        }
    }
}
