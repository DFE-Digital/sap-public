using System;
using System.Collections.Generic;
using System.Text;

namespace SAPPub.Infrastructure.EES.Config
{
    public static class EESConstants
    {
        public const string BaseAPIUrl = "https://api.education.gov.uk";
        public const string FilterSchool = "SCH";
        public const string FilterLocalAuthority = "LA";
        public const string FilterNational = "NAT";

        public const string NationalLocationCode = "E92000001";
        public const string NationalLocationLevel = "NAT";

        public const string YearIdentifier = "AY";


    }

    public static class EESYearConstants
    {
        public const string Year202425 = "2024/2025";
        public const string Year202324 = "2023/2024";
        public const string Year202223  = "2022/2023";
    }
}
