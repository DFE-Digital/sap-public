using Newtonsoft.Json;
using SAPPub.Core.Entities.Performance;
using SAPPub.Infrastructure.EES.Entities.DTOs.KS2;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAPPub.Infrastructure.EES.Config
{
    public enum EESMappingType
    {
        KS2PerformanceEstablishment,
        KS2InformationEstablishment,
        KS2PerformanceLocalAuthority,
        KS2InformationLocalAuthority,
        KS2PerformanceEngland,
        KS2InformationEngland,
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
                    EESMapId = EESMappingType.KS2InformationEstablishment,
                    PublicationId = new Guid("\t019afee4-ba17-73cb-85e0-f88c101bb734"),
                    Name = "Key stage 2 institution level - Schools (School information)",
                    CurrentMajorVersion = "1.*",
                    AutoClassName = "KS2InformationEstablishment",
                    AutoClassRoot = "KS2Information",
                    URL = "https://explore-education-statistics.service.gov.uk/data-catalogue/data-set/f6cb50e9-0eca-4b1e-ac1a-6f6bb9d21a07",
                    FilterIdType = EESConstants.FilterSchool,
                },
                new() {
                    EESMapId = EESMappingType.KS2PerformanceLocalAuthority,
                    PublicationId = new Guid("019afee5-4791-7467-a788-c163fd9b57de"),
                    Name = "Key stage 2 institutional level - Local authority (performance)",
                    CurrentMajorVersion = "1.*",
                    AutoClassName = "KS2PerformanceLocalAuthorityNational",
                    AutoClassRoot = "KS2Performance",
                    URL = "https://explore-education-statistics.service.gov.uk/data-catalogue/data-set/d4f0caa6-f0f9-47ae-9174-ebb0f2d52e16",
                    FilterIdType = EESConstants.FilterLocalAuthority,
                },
                new() {
                    EESMapId = EESMappingType.KS2InformationLocalAuthority,
                    PublicationId = new Guid("019afee5-21ef-7739-b322-011e3ca98089"),
                    Name = "Key stage 2 institutional level - Local authority (LA information)",
                    CurrentMajorVersion = "1.*",
                    AutoClassName = "KS2InformationLocalAuthorityNational",
                    AutoClassRoot = "KS2Information",   
                    URL = "https://explore-education-statistics.service.gov.uk/data-catalogue/data-set/13d572ea-98c3-4968-a64b-d28bc0b53088",
                    FilterIdType = EESConstants.FilterLocalAuthority,
                },
                new() {
                    EESMapId = EESMappingType.KS2PerformanceEngland,
                    PublicationId = new Guid("019afee5-4791-7467-a788-c163fd9b57de"),
                    Name = "Key stage 2 institutional level - Local authority (performance)",
                    CurrentMajorVersion = "1.*",
                    AutoClassName = "KS2PerformanceLocalAuthorityNational",
                    AutoClassRoot = "KS2Performance",
                    URL = "https://explore-education-statistics.service.gov.uk/data-catalogue/data-set/d4f0caa6-f0f9-47ae-9174-ebb0f2d52e16",
                    FilterIdType = EESConstants.FilterNational,
                },
                new() {
                    EESMapId = EESMappingType.KS2InformationEngland,
                    PublicationId = new Guid("019afee5-21ef-7739-b322-011e3ca98089"),
                    Name = "Key stage 2 institutional level - Local authority (LA information)",
                    CurrentMajorVersion = "1.*",
                    AutoClassName = "KS2InformationLocalAuthorityNational",
                    AutoClassRoot = "KS2Information",
                    URL = "https://explore-education-statistics.service.gov.uk/data-catalogue/data-set/13d572ea-98c3-4968-a64b-d28bc0b53088",
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
                nameof(KS2EstablishmentPerformanceDTO) => EESMappingType.KS2PerformanceEstablishment,
                nameof(KS2LAPerformanceDTO) => EESMappingType.KS2PerformanceLocalAuthority,
                nameof(KS2EnglandPerformanceDTO) => EESMappingType.KS2PerformanceEngland,
                nameof(KS2EstablishmentInformationDTO) => EESMappingType.KS2InformationEstablishment,
                nameof(KS2LAInformationDTO) => EESMappingType.KS2InformationLocalAuthority,
                nameof(KS2EnglandInformationDTO) => EESMappingType.KS2InformationEngland,
                _ => throw new NotSupportedException($"No EESMappingType mapping configured for model type '{modelType.FullName}'."),
            };
        }
    }
}
