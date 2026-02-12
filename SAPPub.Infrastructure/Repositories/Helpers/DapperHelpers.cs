using SAPPub.Core.Entities;
using SAPPub.Core.Entities.KS4.Destinations;

namespace SAPPub.Infrastructure.Repositories.Helpers
{
    public class DapperHelpers
    {
        public static string GetReadMultiple(Type entityName)
        {
            return entityName.Name switch
            {
                nameof(Establishment) => $"select * from v_establishment Where \"PhaseOfEducationId\" = 4 LIMIT 100;",//this is temporary until the search page is built
                nameof(EstablishmentDestinations) => $"select * from v_establishment_destinations;",
                nameof(LADestinations) => $"select * from v_la_destinations;",
                nameof(EnglandDestinations) => $"select * from v_england_destinations;",
                _ => $"",
            };
        }

        public static string GetReadSingle(Type entityName)
        {
            return entityName.Name switch
            {
                nameof(Establishment) => $"select * from v_establishment Where \"PhaseOfEducationId\" = 4 AND \"URN\" = @Id;",
                nameof(EstablishmentDestinations) => $"select * from v_establishment_destinations where \"Id\" = @Id;",
                nameof(LADestinations) => $"select * from v_la_destinations where \"Id\" = @Id;",
                nameof(EnglandDestinations) => $"select * from v_england_destinations limit 1;",
                _ => $"",
            };
        }
    }
}
