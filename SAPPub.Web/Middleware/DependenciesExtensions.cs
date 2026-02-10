using SAPPub.Core.Entities;
using SAPPub.Core.Entities.Gateway;
using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Entities.KS4.Workforce;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Repositories.Gateway;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.Absence;
using SAPPub.Core.Interfaces.Repositories.KS4.Destinations;
using SAPPub.Core.Interfaces.Repositories.KS4.Performance;
using SAPPub.Core.Interfaces.Repositories.KS4.SubjectEntries;
using SAPPub.Core.Interfaces.Repositories.KS4.Workforce;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Gateway;
using SAPPub.Core.Interfaces.Services.KS4;
using SAPPub.Core.Interfaces.Services.KS4.Absence;
using SAPPub.Core.Interfaces.Services.KS4.Destinations;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.Interfaces.Services.KS4.SubjectEntries;
using SAPPub.Core.Interfaces.Services.KS4.Workforce;
using SAPPub.Core.Services;
using SAPPub.Core.Services.Gateway;
using SAPPub.Core.Services.KS4;
using SAPPub.Core.Services.KS4.Absence;
using SAPPub.Core.Services.KS4.Destinations;
using SAPPub.Core.Services.KS4.Performance;
using SAPPub.Core.Services.KS4.SubjectEntries;
using SAPPub.Core.Services.KS4.Workforce;
using SAPPub.Infrastructure.Repositories;
using SAPPub.Infrastructure.Repositories.Gateway;
using SAPPub.Infrastructure.Repositories.Generic;
using SAPPub.Infrastructure.Repositories.KS4.Absence;
using SAPPub.Infrastructure.Repositories.KS4.Destinations;
using SAPPub.Infrastructure.Repositories.KS4.Performance;
using SAPPub.Infrastructure.Repositories.KS4.SubjectEntries;
using SAPPub.Infrastructure.Repositories.KS4.Workforce;
using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Web.Middleware
{
    [ExcludeFromCodeCoverage]
    public static class DependenciesExtensions
    {
        public static void AddDependencies(this IServiceCollection services)
        {
            services.AddTransient<IGenericRepository<Establishment>, JSONRepository<Establishment>>();
            services.AddTransient<IEstablishmentRepository, EstablishmentRepository>();
            services.AddTransient<IEstablishmentService, EstablishmentService>();

            services.AddTransient<IGenericRepository<EstablishmentPerformance>, JSONRepository<EstablishmentPerformance>>();
            services.AddTransient<IEstablishmentPerformanceRepository, EstablishmentPerformanceRepository>();
            services.AddTransient<IEstablishmentPerformanceService, EstablishmentPerformanceService>();

            services.AddTransient<IGenericRepository<EstablishmentDestinations>, JSONRepository<EstablishmentDestinations>>();
            services.AddTransient<IEstablishmentDestinationsRepository, EstablishmentDestinationsRepository>();
            services.AddTransient<IEstablishmentDestinationsService, EstablishmentDestinationsService>();

            services.AddTransient<IGenericRepository<EstablishmentAbsence>, JSONRepository<EstablishmentAbsence>>();
            services.AddTransient<IEstablishmentAbsenceRepository, EstablishmentAbsenceRepository>();
            services.AddTransient<IEstablishmentAbsenceService, EstablishmentAbsenceService>();

            services.AddTransient<IGenericRepository<EstablishmentWorkforce>, JSONRepository<EstablishmentWorkforce>>();
            services.AddTransient<IEstablishmentWorkforceRepository, EstablishmentWorkforceRepository>();
            services.AddTransient<IEstablishmentWorkforceService, EstablishmentWorkforceService>();

            services.AddTransient<IGenericRepository<LAPerformance>, JSONRepository<LAPerformance>>();
            services.AddTransient<ILAPerformanceRepository, LAPerformanceRepository>();
            services.AddTransient<ILAPerformanceService, LAPerformanceService>();

            services.AddTransient<IGenericRepository<LADestinations>, JSONRepository<LADestinations>>();
            services.AddTransient<ILADestinationsRepository, LADestinationsRepository>();
            services.AddTransient<ILADestinationsService, LADestinationsService>();

            services.AddTransient<IGenericRepository<LAAbsence>, JSONRepository<LAAbsence>>();
            services.AddTransient<ILAAbsenceRepository, LAAbsenceRepository>();
            services.AddTransient<ILAAbsenceService, LAAbsenceService>();

            services.AddTransient<IGenericRepository<EnglandPerformance>, JSONRepository<EnglandPerformance>>();
            services.AddTransient<IEnglandPerformanceRepository, EnglandPerformanceRepository>();
            services.AddTransient<IEnglandPerformanceService, EnglandPerformanceService>();

            services.AddTransient<IGenericRepository<EnglandDestinations>, JSONRepository<EnglandDestinations>>();
            services.AddTransient<IEnglandDestinationsRepository, EnglandDestinationsRepository>();
            services.AddTransient<IEnglandDestinationsService, EnglandDestinationsService>();

            services.AddTransient<IGenericRepository<EnglandAbsence>, JSONRepository<EnglandAbsence>>();
            services.AddTransient<IEnglandAbsenceRepository, EnglandAbsenceRepository>();
            services.AddTransient<IEnglandAbsenceService, EnglandAbsenceService>();

            services.AddTransient<IGenericRepository<Lookup>, JSONRepository<Lookup>>();
            services.AddTransient<ILookupRepository, LookupRepository>();
            services.AddTransient<ILookupService, LookupService>();

            services.AddTransient<IEstablishmentSubjectEntriesService, EstablishmentSubjectEntriesService>();
            services.AddTransient<IEstablishmentSubjectEntriesRepository, EstablishmentSubjectEntriesRepository>();
            services.AddTransient<IAcademicPerformanceEnglishAndMathsResultsService, Core.Services.KS4.Performance.EnglishAndMathsResultsService>();
            services.AddTransient<ISecondarySchoolService, SecondarySchoolService>();


            services.AddTransient<IGenericCRUDRepository<GatewaySettings>, DapperRepository<GatewaySettings>>();
            //services.AddTransient<IGatewaySettingsRepository, DapperRepository<GatewaySettings>>();
            services.AddTransient<IGatewaySettingsService, GatewaySettingsService>();

            services.AddTransient<IGenericCRUDRepository<GatewayUser>, DapperRepository<GatewayUser>>();
            services.AddTransient<IGatewayUserRepository, GatewayUserRepository>();
            services.AddTransient<IGatewayUserService, GatewayUserService>();

            services.AddTransient<IGenericCRUDRepository<GatewayLocalAuthority>, DapperRepository<GatewayLocalAuthority>>();
            services.AddTransient<IGatewayLocalAuthorityRepository, GatewayLocalAuthorityRepository>();
            services.AddTransient<IGatewayLocalAuthorityService, GatewayLocalAuthorityService>();

            services.AddTransient<IGenericCRUDRepository<GatewayUserAudit>, DapperRepository<GatewayUserAudit>>();
            services.AddTransient<IGatewayUserAuditRepository, GatewayUserAuditRepository>();
            services.AddTransient<IGatewayUserAuditService, GatewayUserAuditService>();

            services.AddTransient<IGenericCRUDRepository<GatewayPageViewAudit>, DapperRepository<GatewayPageViewAudit>>();
            services.AddTransient<IGatewayPageViewAuditRepository, GatewayPageViewAuditRepository>();
            services.AddTransient<IGatewayPageViewAuditService, GatewayPageViewAuditService>();

            services.AddTransient<IGatewayUserLAService, GateWayUserLAService>();
        }
    }
}
