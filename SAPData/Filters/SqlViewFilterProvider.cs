namespace SAPData.Filters
{
    public static class SqlViewFilterProvider
    {
        public static List<SqlViewFilter> GetEstablishmentFilters()
        {
            return
            [
                new SqlViewFilter("ExcludeNurseries", tableAlias =>
                    $"clean_int({tableAlias}.\"phaseofeducation__code_\") <> 1"),
                new SqlViewFilter("IncludeOnlyInScopeSchoolTypes", tableAlias =>
                    $"clean_int({tableAlias}.\"typeofestablishment__code_\") IN (1, 2, 3, 5, 6, 7, 8, 10, 11, 12, 18, 26, 28, 31, 33, 34, 35, 36, 39, 40, 41, 44, 45, 46, 56)"),
                new SqlViewFilter("ExcludeClosed3YrSchools", tableAlias =>
                    $"({tableAlias}.\"closedate\" IS NULL OR {tableAlias}.\"closedate\" = '' OR TO_DATE({tableAlias}.\"closedate\", 'DD/MM/YYYY') >= '{GetAcademicYearCutoffDate()}')"),
                new SqlViewFilter("IncludeKS4", tableAlias =>
                    $"(clean_int({tableAlias}.\"phaseofeducation__code_\") IN (0, 3, 4, 5, 6, 7) " +
                    $"AND clean_int({tableAlias}.\"statutorylowage\") < 16 " +
                    $"AND clean_int({tableAlias}.\"statutoryhighage\") > 12) " +
                    $"OR (clean_int({tableAlias}.\"phaseofeducation__code_\") IN (4, 7) " +
                    $"AND clean_int({tableAlias}.\"statutoryhighage\") = 0)"
                ),
                new SqlViewFilter("ExcludeProposedToOpen", tableAlias =>
                    $"clean_int({tableAlias}.\"establishmentstatus__code_\") <> 4")
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
