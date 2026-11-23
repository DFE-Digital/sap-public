using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAPPub.Infrastructure.EES.NonEES.DTOs
{
    public class CSCPProgKS2
    {
        public string? URN { get; set; }
        public string? READPROG_DESCR { get; set; }
        public string? WRITPROG_DESCR { get; set; }
        public string? MATPROG_DESCR { get; set; }


    }
}
