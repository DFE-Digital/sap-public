using Dapper;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.Absence;
using SAPPub.Core.Interfaces.Repositories.KS4.Destinations;
using SAPPub.Core.Interfaces.Repositories.KS4.Performance;
using SAPPub.Core.Interfaces.Repositories.KS4.SubjectEntries;
using SAPPub.Core.Interfaces.Repositories.KS4.Workforce;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4;
using SAPPub.Core.Interfaces.Services.KS4.Absence;
using SAPPub.Core.Interfaces.Services.KS4.Admissions;
using SAPPub.Core.Interfaces.Services.KS4.Destinations;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.Interfaces.Services.KS4.SubjectEntries;
using SAPPub.Core.Interfaces.Services.KS4.Workforce;
using SAPPub.Core.Services;
using SAPPub.Core.Services.KS4;
using SAPPub.Core.Services.KS4.Absence;
using SAPPub.Core.Services.KS4.Admissions;
using SAPPub.Core.Services.KS4.Destinations;
using SAPPub.Core.Services.KS4.Performance;
using SAPPub.Core.Services.KS4.SubjectEntries;
using SAPPub.Core.Services.KS4.Workforce;
using SAPPub.Infrastructure.Mapping.ValueCodes;
using SAPPub.Infrastructure.Repositories;
using SAPPub.Infrastructure.Repositories.Generic;
using SAPPub.Infrastructure.Repositories.KS4.Absence;
using SAPPub.Infrastructure.Repositories.KS4.Destinations;
using SAPPub.Infrastructure.Repositories.KS4.Performance;
using SAPPub.Infrastructure.Repositories.KS4.SubjectEntries;
using SAPPub.Infrastructure.Repositories.KS4.Workforce;

namespace SAPPub.Web.Middleware
{
    public static class DependenciesExtensions
    {
        private static int _dapperConfigured;

        public static void AddDependencies(this IServiceCollection services)
        {
            // Generic repository
            services.AddTransient(typeof(IGenericRepository<>), typeof(DapperRepository<>));

            // Core repos/services
            services.AddTransient<IEstablishmentRepository, EstablishmentRepository>();
            services.AddTransient<IEstablishmentService, EstablishmentService>();

            services.AddTransient<IEstablishmentPerformanceRepository, EstablishmentPerformanceRepository>();
            services.AddTransient<IEstablishmentPerformanceService, EstablishmentPerformanceService>();

            services.AddTransient<IEstablishmentDestinationsRepository, EstablishmentDestinationsRepository>();
            services.AddTransient<IEstablishmentDestinationsService, EstablishmentDestinationsService>();

            services.AddTransient<IEstablishmentAbsenceRepository, EstablishmentAbsenceRepository>();
            services.AddTransient<IEstablishmentAbsenceService, EstablishmentAbsenceService>();

            services.AddTransient<IEstablishmentWorkforceRepository, EstablishmentWorkforceRepository>();
            services.AddTransient<IEstablishmentWorkforceService, EstablishmentWorkforceService>();

            services.AddTransient<ILAPerformanceRepository, LAPerformanceRepository>();
            services.AddTransient<ILAPerformanceService, LAPerformanceService>();

            services.AddTransient<ILADestinationsRepository, LADestinationsRepository>();
            services.AddTransient<ILADestinationsService, LADestinationsService>();

            services.AddTransient<ILAAbsenceRepository, LAAbsenceRepository>();
            services.AddTransient<ILAAbsenceService, LAAbsenceService>();

            services.AddTransient<IEnglandPerformanceRepository, EnglandPerformanceRepository>();
            services.AddTransient<IEnglandPerformanceService, EnglandPerformanceService>();

            services.AddTransient<IEnglandDestinationsRepository, EnglandDestinationsRepository>();
            services.AddTransient<IEnglandDestinationsService, EnglandDestinationsService>();

            services.AddTransient<IEnglandAbsenceRepository, EnglandAbsenceRepository>();
            services.AddTransient<IEnglandAbsenceService, EnglandAbsenceService>();

            services.AddTransient<ILookupRepository, LookupRepository>();
            services.AddTransient<ILookupService, LookupService>();

            services.AddTransient<IEstablishmentSubjectEntriesRepository, EstablishmentSubjectEntriesRepository>();
            services.AddTransient<IEstablishmentSubjectEntriesService, EstablishmentSubjectEntriesService>();

            services.AddTransient<IAcademicPerformanceEnglishAndMathsResultsService, Core.Services.KS4.Performance.EnglishAndMathsResultsService>();

            services.AddTransient<IDestinationsService, DestinationsService>();
            services.AddTransient<IAdmissionsService, EstablishmentAdmissionsService>();
            services.AddTransient<ILaUrlsRepository, LaUrlsRepository>();

            // Mapper
            services.AddSingleton<ICodedValueMapper, ReflectionCodedValueMapper>();

            // Reason lookup (singleton)
            var reasonLookup = new ReasonCodeLookup(new Dictionary<string, string>
            {
                ["z"] = "Not applicable",
                ["c"] = "Redacted for confidentiality",
                ["x"] = "Not available",
                ["low"] = "positive % less than 0.5"
            });
            services.AddSingleton<IReasonCodeLookup>(reasonLookup);

            // Configure Dapper handlers once per process
            if (Interlocked.Exchange(ref _dapperConfigured, 1) == 0)
            {
                SqlMapper.AddTypeHandler(new CodedDoubleTypeHandler(reasonLookup));
            }
        }
    }
}
