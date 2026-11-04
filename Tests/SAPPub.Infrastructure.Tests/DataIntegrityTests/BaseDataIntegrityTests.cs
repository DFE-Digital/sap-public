using System;
using System.Collections.Generic;
using System.Text;

namespace SAPPub.Infrastructure.Tests.DataIntegrityTests
{
    public class BaseDataIntegrityTests
    {
        //to be reviewed and a design agreed upon after go-live
        //protected string _connectionString = "Host=localhost;Port=5432;Database=SAPData;Username=postgres;Password=postgres";
        protected string _connectionString = "Host=127.0.0.1;Port=1;Database=x;Username=x;Password=x;Timeout=1;Command Timeout=1";

        public BaseDataIntegrityTests()
        {
        }
    }
}
