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

            // Establishment
            services.AddTransient<IEstablishmentRepository, EstablishmentRepository>();
            services.AddTransient<IEstablishmentService, EstablishmentService>();

            // Establishment Performance
            services.AddTransient<IEstablishmentPerformanceRepository, EstablishmentPerformanceRepository>();
            services.AddTransient<IEstablishmentPerformanceService, EstablishmentPerformanceService>();

            // Establishment Destinations (now can rely on generic repo + coded mapping)
            services.AddTransient<IEstablishmentDestinationsRepository, EstablishmentDestinationsRepository>();
            services.AddTransient<IEstablishmentDestinationsService, EstablishmentDestinationsService>();

            // Absence
            services.AddTransient<IEstablishmentAbsenceRepository, EstablishmentAbsenceRepository>();
            services.AddTransient<IEstablishmentAbsenceService, EstablishmentAbsenceService>();

            // Workforce
            services.AddTransient<IEstablishmentWorkforceRepository, EstablishmentWorkforceRepository>();
            services.AddTransient<IEstablishmentWorkforceService, EstablishmentWorkforceService>();

            // LA Performance
            services.AddTransient<ILAPerformanceRepository, LAPerformanceRepository>();
            services.AddTransient<ILAPerformanceService, LAPerformanceService>();

            // LA Destinations
            services.AddTransient<ILADestinationsRepository, LADestinationsRepository>();
            services.AddTransient<ILADestinationsService, LADestinationsService>();

            // LA Absence
            services.AddTransient<ILAAbsenceRepository, LAAbsenceRepository>();
            services.AddTransient<ILAAbsenceService, LAAbsenceService>();

            // England Performance
            services.AddTransient<IEnglandPerformanceRepository, EnglandPerformanceRepository>();
            services.AddTransient<IEnglandPerformanceService, EnglandPerformanceService>();

            // England Destinations
            services.AddTransient<IEnglandDestinationsRepository, EnglandDestinationsRepository>();
            services.AddTransient<IEnglandDestinationsService, EnglandDestinationsService>();

            // England Absence
            services.AddTransient<IEnglandAbsenceRepository, EnglandAbsenceRepository>();
            services.AddTransient<IEnglandAbsenceService, EnglandAbsenceService>();

            // Lookup
            services.AddTransient<ILookupRepository, LookupRepository>();
            services.AddTransient<ILookupService, LookupService>();

            // Subject entries / misc
            services.AddTransient<IEstablishmentSubjectEntriesService, EstablishmentSubjectEntriesService>();
            services.AddTransient<IEstablishmentSubjectEntriesRepository, EstablishmentSubjectEntriesRepository>();
            services.AddTransient<IAcademicPerformanceEnglishAndMathsResultsService, Core.Services.KS4.Performance.EnglishAndMathsResultsService>();
            services.AddTransient<ISecondarySchoolService, SecondarySchoolService>();

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
            services.AddTransient<ISecondarySchoolService, SecondarySchoolService>();

            services.AddScoped<IAdmissionsService, EstablishmentAdmissionsService>();
            services.AddScoped<IGenericRepository<LaUrls>, JSONRepository<LaUrls>>();
            services.AddScoped<ILaUrlsRepository, LaUrlsRepository>();
            services.AddTransient<IDestinationsService, DestinationsService>();
            services.AddTransient<IAdmissionsService, EstablishmentAdmissionsService>();
            services.AddTransient<IGenericRepository<LaUrls>, JSONRepository<LaUrls>>();
            services.AddTransient<ILaUrlsRepository, LaUrlsRepository>();
        }
    }
}
