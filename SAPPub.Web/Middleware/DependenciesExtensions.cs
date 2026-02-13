using Dapper;
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
using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Web.Middleware
{
    [ExcludeFromCodeCoverage]
    public static class DependenciesExtensions
    {
        public interface IDapperBootstrapper { }
        public sealed class DapperBootstrapper : IDapperBootstrapper { }

        public static void AddDependencies(this IServiceCollection services)
        {
            // Register generic repository for ALL entities
            services.AddTransient(typeof(IGenericRepository<>), typeof(DapperRepository<>));

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

            services.AddTransient<ILookupRepository, LookupRepository>();
            services.AddTransient<ILookupService, LookupService>();

            services.AddTransient<IEstablishmentSubjectEntriesRepository, EstablishmentSubjectEntriesRepository>();
            services.AddTransient<IEstablishmentSubjectEntriesService, EstablishmentSubjectEntriesService>();

            services.AddTransient<IAcademicPerformanceEnglishAndMathsResultsService, Core.Services.KS4.Performance.EnglishAndMathsResultsService>();

            // Mapper (reflection-based coded mapping)
            services.AddSingleton<ICodedValueMapper, ReflectionCodedValueMapper>();

            // Reason lookup
            services.AddSingleton<IReasonCodeLookup>(_ =>
                new ReasonCodeLookup(new Dictionary<string, string>
                {
                    ["z"] = "Not applicable",
                    ["c"] = "Redacted for confidentiality",
                    ["x"] = "Not available",
                    ["low"] = "positive % less than 0.5"
                }));

            // Dapper type handler bootstrapper 
            services.AddSingleton<IDapperBootstrapper>(sp =>
            {
                var lookup = sp.GetRequiredService<IReasonCodeLookup>();
                SqlMapper.AddTypeHandler(new CodedDoubleTypeHandler(lookup));
                return new DapperBootstrapper();
            });
            services.AddSingleton(sp => sp.GetRequiredService<IDapperBootstrapper>());

            services.AddTransient<IDestinationsService, DestinationsService>();
            services.AddTransient<IAdmissionsService, EstablishmentAdmissionsService>();
            services.AddTransient<ILaUrlsRepository, LaUrlsRepository>();
        }
    }
}
