using Newtonsoft.Json;
using SAPPub.Core.Entities.Performance;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAPPub.Infrastructure.EES.Config
{
    public enum EESMappingType
    {
        KS2PerformanceEstablishment,
        KS2PerformanceLocalAuthority,
        KS2PerformanceEngland,
        KS4PerformanceEstablishment,
        KS4PerformanceLocalAuthority,
        KS4PerformanceEngland   
    }

    public class EESMapping
    {
        public required EESMappingType EESMapId { get; set; }

        public required Guid PublicationId { get; set; }

        public required string Name { get; set; }

        public required string CurrentMajorVersion { get; set; }

        public required string AutoClassName { get; set; }

        public required string AutoClassRoot { get; set; }

        public required string URL { get; set; }

        public required string FilterIdType { get; set; }


        public static List<EESMapping> GetMappings()
        {
            return
            [
                new() {
                    EESMapId = EESMappingType.KS2PerformanceEstablishment,
                    PublicationId = new Guid("019afee4-e5d0-72f9-9a8f-d7a1a56eac1d"),
                    Name = "Key stage 2 institution level - Schools (performance)",
                    CurrentMajorVersion = "1.*",
                    AutoClassName = "KS2PerformanceEstablishment",
                    AutoClassRoot = "KS2Performance",
                    URL = "https://explore-education-statistics.service.gov.uk/data-catalogue/data-set/f6cb50e9-0eca-4b1e-ac1a-6f6bb9d21a07",
                    FilterIdType = EESConstants.FilterSchool,
                },
                new() {
                    EESMapId = EESMappingType.KS2PerformanceLocalAuthority,
                    PublicationId = new Guid("019afee5-4791-7467-a788-c163fd9b57de"),
                    Name = "Key stage 2 institutional level - Local authority (performance)",
                    CurrentMajorVersion = "1.*",
                    AutoClassName = "KS2PerformanceLANAT",
                    AutoClassRoot = "KS2Performance",
                    URL = "https://explore-education-statistics.service.gov.uk/data-catalogue/data-set/d4f0caa6-f0f9-47ae-9174-ebb0f2d52e16",
                    FilterIdType = EESConstants.FilterLocalAuthority,
                },
                new() {
                    EESMapId = EESMappingType.KS2PerformanceEngland,
                    PublicationId = new Guid("019afee5-4791-7467-a788-c163fd9b57de"),
                    Name = "Key stage 2 institutional level - Local authority (performance)",
                    CurrentMajorVersion = "1.*",
                    AutoClassName = "KS2PerformanceLANAT",
                    AutoClassRoot = "KS2Performance",
                    URL = "https://explore-education-statistics.service.gov.uk/data-catalogue/data-set/d4f0caa6-f0f9-47ae-9174-ebb0f2d52e16",
                    FilterIdType = EESConstants.FilterNational,
                },
                new() {
                    EESMapId = EESMappingType.KS4PerformanceEstablishment,
                    PublicationId = new Guid("19e39901-a96c-be76-b9c2-6af54ae076d2"),
                    Name = "Key stage 4 institution level - Performance",
                    CurrentMajorVersion = "1.0.3",
                    AutoClassName = "KS4PerformanceEstablishment",
                    AutoClassRoot = "KS4Performance",
                    URL = "https://explore-education-statistics.service.gov.uk/data-catalogue/data-set/19e39901-a96c-be76-b9c2-6af54ae076d2",
                    FilterIdType = EESConstants.FilterSchool,
                }
            ];
        }

        public static EESMapping? GetMapping(EESMappingType mappingType)
        {
            return GetMappings().FirstOrDefault(m => m.EESMapId == mappingType);
        }

        public static EESMappingType LookupFromModel(Type modelType)
        {
            return modelType.Name switch
            {
                nameof(KS2EstablishmentPerformance) => EESMappingType.KS2PerformanceEstablishment,
                nameof(KS2LAPerformance) => EESMappingType.KS2PerformanceLocalAuthority,
                nameof(KS2EnglandPerformance) => EESMappingType.KS2PerformanceEngland,
                _ => throw new NotSupportedException($"No EESMappingType mapping configured for model type '{modelType.FullName}'."),
            };
        }
    }
}
