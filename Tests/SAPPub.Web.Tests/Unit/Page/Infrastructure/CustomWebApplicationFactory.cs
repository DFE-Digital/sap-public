using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Core.Interfaces.Services.KS4.Absence;
using SAPPub.Core.Interfaces.Services.KS4.Admissions;
using SAPPub.Core.Interfaces.Services.KS4.Destinations;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.Interfaces.Services.KS4.SubjectEntries;
using SAPPub.Core.Interfaces.Services.KS4.Workforce;
using SAPPub.Core.Interfaces.Services.Search;

namespace SAPPub.Web.Tests.Unit.Page.Infrastructure;

public class CustomWebApplicationFactory<Program> : WebApplicationFactory<Program>
     where Program : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .ConfigureServices(services =>
            {
                // use mock services
                services.RemoveAll(typeof(IAboutSchoolService));
                services.RemoveAll(typeof(IAttainmentAndProgressService));
                services.RemoveAll(typeof(IEstablishmentService));
                services.RemoveAll(typeof(IEstablishmentPerformanceService));
                services.RemoveAll(typeof(IEstablishmentDestinationsService));
                services.RemoveAll(typeof(IEstablishmentAbsenceService));
                services.RemoveAll(typeof(IEstablishmentWorkforceService));
                services.RemoveAll(typeof(IEstablishmentSubjectEntriesService));
                services.RemoveAll(typeof(IAcademicPerformanceEnglishAndMathsResultsService));
                services.RemoveAll(typeof(IDestinationsService));
                services.RemoveAll(typeof(IAdmissionsService));
                services.RemoveAll(typeof(ISchoolSearchIndexReader));
                services.RemoveAll(typeof(ISchoolSearchService));
                services.AddSingleton<MockAccessor<IAboutSchoolService>>();
                services.AddSingleton<MockAccessor<IAttainmentAndProgressService>>();
                services.AddSingleton<MockAccessor<IEstablishmentService>>();
                services.AddSingleton<MockAccessor<IEstablishmentPerformanceService>>();
                services.AddSingleton<MockAccessor<IEstablishmentDestinationsService>>();
                services.AddSingleton<MockAccessor<IEstablishmentAbsenceService>>();
                services.AddSingleton<MockAccessor<IEstablishmentWorkforceService>>();
                services.AddSingleton<MockAccessor<IEstablishmentSubjectEntriesService>>();
                services.AddSingleton<MockAccessor<IAcademicPerformanceEnglishAndMathsResultsService>>();
                services.AddSingleton<MockAccessor<IDestinationsService>>();
                services.AddSingleton<MockAccessor<IAdmissionsService>>();
                services.AddSingleton<MockAccessor<ISchoolSearchIndexReader>>();
                services.AddSingleton<MockAccessor<ISchoolSearchService>>();

                services.AddTransient(provider =>
                {
                    return provider.GetRequiredService<MockAccessor<IAboutSchoolService>>().Get()?.Object!;
                });
                services.AddTransient(provider =>
                {
                    return provider.GetRequiredService<MockAccessor<IAttainmentAndProgressService>>().Get()?.Object!;
                });
                services.AddTransient(provider =>
                {
                    return provider.GetRequiredService<MockAccessor<IEstablishmentService>>().Get()?.Object!;
                });
                services.AddTransient(provider =>
                    {
                        return provider.GetRequiredService<MockAccessor<IEstablishmentPerformanceService>>().Get()?.Object!;
                    });
                services.AddTransient(provider =>
                    {
                        return provider.GetRequiredService<MockAccessor<IEstablishmentDestinationsService>>().Get()?.Object!;
                    });
                services.AddTransient(provider =>
                    {
                        return provider.GetRequiredService<MockAccessor<IEstablishmentAbsenceService>>().Get()?.Object!;
                    });
                services.AddTransient(provider =>
                {
                    return provider.GetRequiredService<MockAccessor<IEstablishmentWorkforceService>>().Get()?.Object!;
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
                services.AddSingleton(provider =>
                {
                    return provider.GetRequiredService<MockAccessor<ISchoolSearchIndexReader>>().Get()?.Object!;
                });
                services.AddSingleton(provider =>
                {
                    return provider.GetRequiredService<MockAccessor<ISchoolSearchService>>().Get()?.Object!;
                });
            });
    }
}
