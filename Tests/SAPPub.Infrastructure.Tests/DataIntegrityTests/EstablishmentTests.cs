using Npgsql;
using SAPPub.Core.Entities;
using System;

namespace SAPPub.Infrastructure.Tests.DataIntegrityTests
{
    public class EstablishmentTests : BaseDataIntegrityTests
    {
        public readonly List<Establishment> _establishments = new List<Establishment>();

        public EstablishmentTests()
        {
            string query = @"SELECT ""URN"", ""EstablishmentName"", ""LAId"", ""LAName"", ""GSSLACode"" FROM v_establishment ORDER BY ""URN""";

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var establishment = new Establishment
                        {
                            URN = reader.GetString(0),
                            EstablishmentName = reader.GetString(1),
                            LAId = reader.GetString(2),
                            LAName = reader.GetString(3),
                            GSSLACode = reader.IsDBNull(4) ? null : reader.GetString(4)
                        };
                        _establishments.Add(establishment);
                    }
                }
            }
        }

        //[Fact]
        //public void Establishment_FK_LA_URLS_DataIntegrityTest()
        //{
        //    // Arrange
        //    var laUrlsTests = new LaUrlsTests();

        //    // Act + Assert
        //    foreach (var establishment in _establishments)
        //    {
        //        var matchingLaUrl = laUrlsTests._laUrls.FirstOrDefault(l => l.GSSLACode == establishment.GSSLACode);
        //        //GSSLACode is set to X999999 if closed and X999999 not in the La_Urls table
        //        if (establishment.GSSLACode == "X999999") 
        //        {
        //            continue;
        //        }
        //        Assert.Equal(establishment.GSSLACode, matchingLaUrl?.GSSLACode);
        //    }
        //}
    }
}
