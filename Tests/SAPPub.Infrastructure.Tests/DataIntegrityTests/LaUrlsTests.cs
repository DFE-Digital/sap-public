using Npgsql;
using SAPPub.Core.Entities;
using System;

namespace SAPPub.Infrastructure.Tests.DataIntegrityTests
{
    public class LaUrlsTests : BaseDataIntegrityTests
    {
        public readonly List<LaUrls> _laUrls = new List<LaUrls>();

        public LaUrlsTests()
        {
            string query = @"SELECT ""Id"", ""GSSLACode"", ""LAMainUrl"", ""Name"" FROM v_la_urls ORDER BY ""Id""";

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var laUrls = new LaUrls
                        {
                            Id = reader.IsDBNull(0) ? null : reader.GetString(0),
                            GSSLACode = reader.IsDBNull(1) ? null : reader.GetString(1),
                            LAMainUrl = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Name = reader.IsDBNull(3) ? null : reader.GetString(3)
                        };
                        _laUrls.Add(laUrls);
                    }
                }
            }
        }

        //[Fact]
        //public void LA_URLS_x_DataIntegrityTest()
        //{
        //    // Arrange

        //    // Act

        //    // Assert
        //}
    }
}
