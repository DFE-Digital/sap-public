using SAPPub.Core.Entities;
using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Entities.KS4.SubjectEntries;
using SAPPub.Core.Entities.KS4.Workforce;
using SAPPub.Infrastructure.Repositories.Helpers;
using System;
using Xunit;

namespace SAPPub.Infrastructure.Tests.Repositories.Helpers
{
    public class DapperHelpersTests
    {
        [Fact]
        public void GetReadMultiple_for_mapped_types_returns_sql()
        {
            Assert.NotEmpty(DapperHelpers.GetReadMultiple(typeof(Establishment)));
            Assert.NotEmpty(DapperHelpers.GetReadMultiple(typeof(EstablishmentAbsence)));
            Assert.NotEmpty(DapperHelpers.GetReadMultiple(typeof(EstablishmentDestinations)));
            Assert.NotEmpty(DapperHelpers.GetReadMultiple(typeof(EstablishmentPerformance)));
            Assert.NotEmpty(DapperHelpers.GetReadMultiple(typeof(EstablishmentWorkforce)));
            Assert.NotEmpty(DapperHelpers.GetReadMultiple(typeof(LADestinations)));
            Assert.NotEmpty(DapperHelpers.GetReadMultiple(typeof(LAPerformance)));
            Assert.NotEmpty(DapperHelpers.GetReadMultiple(typeof(EnglandDestinations)));
            Assert.NotEmpty(DapperHelpers.GetReadMultiple(typeof(EnglandPerformance)));
            Assert.NotEmpty(DapperHelpers.GetReadMultiple(typeof(LaUrls)));
        }

        [Fact]
        public void GetReadSingle_for_mapped_types_returns_sql()
        {
            Assert.NotEmpty(DapperHelpers.GetReadSingle(typeof(Establishment)));
            Assert.NotEmpty(DapperHelpers.GetReadSingle(typeof(EstablishmentAbsence)));
            Assert.NotEmpty(DapperHelpers.GetReadSingle(typeof(EstablishmentDestinations)));
            Assert.NotEmpty(DapperHelpers.GetReadSingle(typeof(EstablishmentPerformance)));
            Assert.NotEmpty(DapperHelpers.GetReadSingle(typeof(EstablishmentWorkforce)));
            Assert.NotEmpty(DapperHelpers.GetReadSingle(typeof(LADestinations)));
            Assert.NotEmpty(DapperHelpers.GetReadSingle(typeof(LAPerformance)));
            Assert.NotEmpty(DapperHelpers.GetReadSingle(typeof(EnglandDestinations)));
            Assert.NotEmpty(DapperHelpers.GetReadSingle(typeof(EnglandPerformance)));
            Assert.NotEmpty(DapperHelpers.GetReadSingle(typeof(LaUrls)));
        }

        [Fact]
        public void GetReadMany_for_subject_entries_row_returns_expected_sql_shape()
        {
            var sql = DapperHelpers.GetReadMany(typeof(EstablishmentSubjectEntryRow));

            Assert.NotEmpty(sql);
            Assert.Contains("from public.v_establishment_subject_entries", sql, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("where school_urn = @Urn", sql, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Establishment_ReadSingle_uses_URN_column_and_Id_parameter()
        {
            var sql = DapperHelpers.GetReadSingle(typeof(Establishment));

            Assert.Contains("from public.v_establishment", sql, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("where \"URN\" = @Id", sql, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Most_ReadSingle_queries_filter_on_Id_parameter()
        {
            // a couple of representative ones
            Assert.Contains("where \"Id\" = @Id", DapperHelpers.GetReadSingle(typeof(EstablishmentAbsence)), StringComparison.OrdinalIgnoreCase);
            Assert.Contains("where \"Id\" = @Id", DapperHelpers.GetReadSingle(typeof(EstablishmentDestinations)), StringComparison.OrdinalIgnoreCase);
            Assert.Contains("where \"Id\" = @Id", DapperHelpers.GetReadSingle(typeof(LAPerformance)), StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void England_ReadSingle_queries_filter_on_National_id()
        {
            var destSql = DapperHelpers.GetReadSingle(typeof(EnglandDestinations));
            var perfSql = DapperHelpers.GetReadSingle(typeof(EnglandPerformance));

            Assert.Contains("\"Id\" = 'National'", destSql, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("\"Id\" = 'National'", perfSql, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Unmapped_type_returns_empty_sql()
        {
            Assert.Equal(string.Empty, DapperHelpers.GetReadMultiple(typeof(Unmapped)));
            Assert.Equal(string.Empty, DapperHelpers.GetReadSingle(typeof(Unmapped)));
            Assert.Equal(string.Empty, DapperHelpers.GetReadMany(typeof(Unmapped)));
        }

        [Fact]
        public void SubjectEntries_ReadMany_selects_expected_columns_for_row_mapping()
        {
            var sql = DapperHelpers.GetReadMany(typeof(EstablishmentSubjectEntryRow));

            Assert.NotEmpty(sql);

            // Core mapping fields that MUST exist for Dapper binding
            Assert.Contains("school_urn", sql, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("pupil_count", sql, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("subject", sql, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("qualification_type", sql, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("qualification_detailed", sql, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("grade", sql, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("number_achieving", sql, StringComparison.OrdinalIgnoreCase);

            // View and parameter contract
            Assert.Contains("from public.v_establishment_subject_entries", sql, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("where school_urn = @Urn", sql, StringComparison.OrdinalIgnoreCase);
        }

        public sealed class Unmapped { }
    }
}
