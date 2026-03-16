namespace SAPData.Filters
{
    public static class SqlViewFilterProvider
    {
        public static List<SqlViewFilter> GetEstablishmentFilters()
        {
            return
            [
                new SqlViewFilter("ExcludeOnlineSchools", tableAlias =>
                    $"clean_int({tableAlias}.\"typeofestablishment__code_\") <> 49"),
                new SqlViewFilter("ExcludeClosed3YrSchools", tableAlias =>
                    $"{tableAlias}.\"closedate\" IS NULL OR {tableAlias}.\"closedate\" = '' OR TO_DATE({tableAlias}.\"closedate\", 'DD/MM/YYYY') >= '{GetAcademicYearCutoffDate()}'"),
                new SqlViewFilter("IncludeKS4", tableAlias =>
                    $"clean_int({tableAlias}.\"phaseofeducation__code_\") IN (4, 5, 7)")
                ];
        }

        public static string GetAcademicYearCutoffDate(DateTime now)
        {
            // Always use previous 12/09 minus 3 years
            var previousSept12 = now.Month < 9 || now.Month == 9 && now.Day < 12
                ? new DateTime(now.Year - 1, 9, 12)
                : new DateTime(now.Year, 9, 12);

            var cutoffDate = previousSept12.AddYears(-3);
            return cutoffDate.ToString("yyyy-MM-dd");
        }
        public static string GetAcademicYearCutoffDate()
        {
            return GetAcademicYearCutoffDate(DateTime.Today);
        }
    }
}
