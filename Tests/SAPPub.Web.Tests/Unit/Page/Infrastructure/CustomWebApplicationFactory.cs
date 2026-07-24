using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using SAPPub.Core.Interfaces;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Core.Interfaces.Services.KS4.Absence;
using SAPPub.Core.Interfaces.Services.KS4.Admissions;
using SAPPub.Core.Interfaces.Services.KS4.Attendance;
using SAPPub.Core.Interfaces.Services.KS4.Destinations;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.Interfaces.Services.KS4.SubjectEntries;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.Interfaces.Services.Search;

namespace SAPPub.Web.Tests.Unit.Page.Infrastructure;

public class CustomWebApplicationFactory<Program> : WebApplicationFactory<Program>
     where Program : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder
            .ConfigureServices(services =>
            {
                // needed for the background Service that initialises Lucene search
                services.RemoveAll<IHostedService>();
                services.RemoveAll(typeof(IEstablishmentRepository));
                services.AddTransient<IEstablishmentRepository>(provider =>
                {
                    var accessor = provider.GetRequiredService<MockAccessor<IEstablishmentRepository>>();
                    return accessor.GetOrCreate().Object;
                });

                // mock services used by controllers
                services.RemoveAll(typeof(IAboutSchoolService));
                services.RemoveAll(typeof(IAttainmentAndProgressService));
                services.RemoveAll(typeof(IEstablishmentService));
                services.RemoveAll(typeof(IEstablishmentPerformanceService));
                services.RemoveAll(typeof(IEstablishmentAbsenceService));
                services.RemoveAll(typeof(IEstablishmentSubjectEntriesService));
                services.RemoveAll(typeof(IAcademicPerformanceEnglishAndMathsResultsService));
                services.RemoveAll(typeof(IDestinationsService));
                services.RemoveAll(typeof(IAdmissionsService));
                services.RemoveAll(typeof(ISchoolSearchIndexReader));
                services.RemoveAll(typeof(ISchoolSearchService));
                services.RemoveAll(typeof(IAttendanceService));
                services.RemoveAll(typeof(IEnglandPerformanceService));
                services.RemoveAll(typeof(IMySchoolsListService));
                services.RemoveAll(typeof(IEnglishAndMathsComparisionService));
                services.RemoveAll(typeof(IDestinationsComparisonService));
                services.RemoveAll(typeof(IAttainmentAndProgressComparisionService));
                services.RemoveAll(typeof(IAdditionalMeasuresService));
                services.RemoveAll(typeof(IAdvancedLevelQualificationsService));

                services.AddSingleton<MockAccessor<IAboutSchoolService>>();
                services.AddSingleton<MockAccessor<IAttainmentAndProgressService>>();
                services.AddSingleton<MockAccessor<IEstablishmentService>>();
                services.AddSingleton<MockAccessor<IEstablishmentPerformanceService>>();
                services.AddSingleton<MockAccessor<IEstablishmentAbsenceService>>();
                services.AddSingleton<MockAccessor<IEstablishmentSubjectEntriesService>>();
                services.AddSingleton<MockAccessor<IAcademicPerformanceEnglishAndMathsResultsService>>();
                services.AddSingleton<MockAccessor<IDestinationsService>>();
                services.AddSingleton<MockAccessor<IAdmissionsService>>();
                services.AddSingleton<MockAccessor<ISchoolSearchIndexReader>>();
                services.AddSingleton<MockAccessor<ISchoolSearchService>>();
                services.AddSingleton<MockAccessor<IAttendanceService>>();
                services.AddSingleton<MockAccessor<IEnglandPerformanceService>>();
                services.AddSingleton<MockAccessor<IMySchoolsListService>>();
                services.AddSingleton<MockAccessor<IEnglishAndMathsComparisionService>>();
                services.AddSingleton<MockAccessor<IDestinationsComparisonService>>();
                services.AddSingleton<MockAccessor<IAttainmentAndProgressComparisionService>>();
                services.AddSingleton<MockAccessor<IAdditionalMeasuresService>>();
                services.AddSingleton<MockAccessor<IAdvancedLevelQualificationsService>>();

                services.AddTransient(provider =>
                {
                    return provider.GetRequiredService<MockAccessor<IAboutSchoolService>>().Get()?.Object!;
                });
                services.AddTransient(provider =>
                {
                    return provider.GetRequiredService<MockAccessor<IAttainmentAndProgressService>>().Get()?.Object!;
                });
                services.AddTransient<IEstablishmentService>(provider =>
                {
                    var accessor = provider.GetRequiredService<MockAccessor<IEstablishmentService>>();
                    return accessor.GetOrCreate().Object; // provide default for Lucene search initialisation background service
                });
                services.AddTransient(provider =>
                    {
                        return provider.GetRequiredService<MockAccessor<IEstablishmentPerformanceService>>().Get()?.Object!;
                    });
                services.AddTransient(provider =>
                    {
                        return provider.GetRequiredService<MockAccessor<IEstablishmentAbsenceService>>().Get()?.Object!;
                    });
                services.AddTransient(provider =>
                {
                    return provider.GetRequiredService<MockAccessor<IEstablishmentSubjectEntriesService>>().Get()?.Object!;
                });
                services.AddTransient(provider =>
                {
                    return provider.GetRequiredService<MockAccessor<IAcademicPerformanceEnglishAndMathsResultsService>>().Get()?.Object!;
                });
                services.AddTransient(provider =>
                {
                    return provider.GetRequiredService<MockAccessor<IDestinationsService>>().Get()?.Object!;
                });
                services.AddTransient(provider =>
                {
                    return provider.GetRequiredService<MockAccessor<IAdmissionsService>>().Get()?.Object!;
                });
                services.AddTransient(provider =>
                {
                    return provider.GetRequiredService<MockAccessor<IAttendanceService>>().Get()?.Object!;
                });
                services.AddTransient(provider =>
                {
                    return provider.GetRequiredService<MockAccessor<IEnglandPerformanceService>>().Get()?.Object!;
                });
                services.AddTransient(provider =>
                {
                    return provider.GetRequiredService<MockAccessor<ISchoolSearchIndexReader>>().Get()?.Object!;
                });
                services.AddTransient(provider =>
                {
                    return provider.GetRequiredService<MockAccessor<ISchoolSearchService>>().Get()?.Object!;
                });
                services.AddTransient(provider =>
                {
                    return provider.GetRequiredService<MockAccessor<IMySchoolsListService>>().Get()?.Object!;
                });
                services.AddTransient(provider =>
                {
                    return provider.GetRequiredService<MockAccessor<IEnglishAndMathsComparisionService>>().Get()?.Object!;
                });
                services.AddTransient(provider =>
                {
                    return provider.GetRequiredService<MockAccessor<IDestinationsComparisonService>>().Get()?.Object!;
                });
                services.AddTransient(provider =>
                {
                    return provider.GetRequiredService<MockAccessor<IAttainmentAndProgressComparisionService>>().Get()?.Object!;
                });
                services.AddTransient(provider =>
                {
                    return provider.GetRequiredService<MockAccessor<IAdditionalMeasuresService>>().Get()?.Object!;
                });
                services.AddTransient(provider =>
                {
                    return provider.GetRequiredService<MockAccessor<IAdvancedLevelQualificationsService>>().Get()?.Object!;
                });                
            });
    }
}
