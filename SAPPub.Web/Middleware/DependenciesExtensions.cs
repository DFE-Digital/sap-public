using SAPPub.Core.Entities;
using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Entities.KS4.Workforce;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.Absence;
using SAPPub.Core.Interfaces.Repositories.KS4.Destinations;
using SAPPub.Core.Interfaces.Repositories.KS4.Performance;
using SAPPub.Core.Interfaces.Repositories.KS4.Workforce;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Absence;
using SAPPub.Core.Interfaces.Services.KS4.Destinations;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.Interfaces.Services.KS4.Workforce;
using SAPPub.Core.Services;
using SAPPub.Core.Services.KS4.Absence;
using SAPPub.Core.Services.KS4.Destinations;
using SAPPub.Core.Services.KS4.Performance;
using SAPPub.Core.Services.KS4.Workforce;
using SAPPub.Infrastructure.Repositories;
using SAPPub.Infrastructure.Repositories.Generic;
using SAPPub.Infrastructure.Repositories.KS4.Absence;
using SAPPub.Infrastructure.Repositories.KS4.Destinations;
using SAPPub.Infrastructure.Repositories.KS4.Performance;
using SAPPub.Infrastructure.Repositories.KS4.Workforce;
using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Web.Middleware
{
    [ExcludeFromCodeCoverage]
    public static class DependenciesExtensions
    {
        public static void AddDependencies(this IServiceCollection services)
        {


            services.AddSingleton<IGenericRepository<Establishment>, JSONRepository<Establishment>>();
            services.AddSingleton<IEstablishmentRepository, EstablishmentRepository>();
            services.AddSingleton<IEstablishmentService, EstablishmentService>();

            services.AddSingleton<IGenericRepository<EstablishmentPerformance>, JSONRepository<EstablishmentPerformance>>();
            services.AddSingleton<IEstablishmentPerformanceRepository, EstablishmentPerformanceRepository>();
            services.AddSingleton<IEstablishmentPerformanceService, EstablishmentPerformanceService>();

            services.AddSingleton<IGenericRepository<EstablishmentDestinations>, JSONRepository<EstablishmentDestinations>>();
            services.AddSingleton<IEstablishmentDestinationsRepository, EstablishmentDestinationsRepository>();
            services.AddSingleton<IEstablishmentDestinationsService, EstablishmentDestinationsService>();

            services.AddSingleton<IGenericRepository<EstablishmentAbsence>, JSONRepository<EstablishmentAbsence>>();
            services.AddSingleton<IEstablishmentAbsenceRepository, EstablishmentAbsenceRepository>();
            services.AddSingleton<IEstablishmentAbsenceService, EstablishmentAbsenceService>();

            services.AddSingleton<IGenericRepository<EstablishmentWorkforce>, JSONRepository<EstablishmentWorkforce>>();
            services.AddSingleton<IEstablishmentWorkforceRepository, EstablishmentWorkforceRepository>();
            services.AddSingleton<IEstablishmentWorkforceService, EstablishmentWorkforceService>();

            services.AddSingleton<IGenericRepository<LAPerformance>, JSONRepository<LAPerformance>>();
            services.AddSingleton<ILAPerformanceRepository, LAPerformanceRepository>();
            services.AddSingleton<ILAPerformanceService, LAPerformanceService>();

            services.AddSingleton<IGenericRepository<LADestinations>, JSONRepository<LADestinations>>();
            services.AddSingleton<ILADestinationsRepository, LADestinationsRepository>();
            services.AddSingleton<ILADestinationsService, LADestinationsService>();

            services.AddSingleton<IGenericRepository<EnglandPerformance>, JSONRepository<EnglandPerformance>>();
            services.AddSingleton<IEnglandPerformanceRepository, EnglandPerformanceRepository>();
            services.AddSingleton<IEnglandPerformanceService, EnglandPerformanceService>();

            services.AddSingleton<IGenericRepository<EnglandDestinations>, JSONRepository<EnglandDestinations>>();
            services.AddSingleton<IEnglandDestinationsRepository, EnglandDestinationsRepository>();
            services.AddSingleton<IEnglandDestinationsService, EnglandDestinationsService>();

            services.AddSingleton<IGenericRepository<Lookup>, JSONRepository<Lookup>>();
            services.AddSingleton<ILookupRepository, LookupRepository>();
            services.AddSingleton<ILookupService, LookupService>();
        }
    }
}
