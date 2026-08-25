using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;
using Moq;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Core.Interfaces.Services.KS4.Absence;
using SAPPub.Core.Interfaces.Services.KS4.Admissions;
using SAPPub.Core.Interfaces.Services.KS4.Attendance;
using SAPPub.Core.Interfaces.Services.KS4.Destinations;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
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
                services.RemoveAll(typeof(IKS4EstablishmentSubjectEntriesService));
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
                services.RemoveAll(typeof(ILevel3QualificationsService));
                services.RemoveAll(typeof(ILevel2QualificationsService));
                services.RemoveAll(typeof(IKS2ScaledScoreService));
                services.RemoveAll(typeof(IKS2AdditionalMeasuresService));
                services.RemoveAll(typeof(IFeatureManager));


                services.AddSingleton<MockAccessor<IAboutSchoolService>>();
                services.AddSingleton<MockAccessor<IAttainmentAndProgressService>>();
                services.AddSingleton<MockAccessor<IEstablishmentService>>();
                services.AddSingleton<MockAccessor<IEstablishmentPerformanceService>>();
                services.AddSingleton<MockAccessor<IEstablishmentAbsenceService>>();
                services.AddSingleton<MockAccessor<IKS4EstablishmentSubjectEntriesService>>();
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
                services.AddSingleton<MockAccessor<ILevel3QualificationsService>>();
                services.AddSingleton<MockAccessor<ILevel2QualificationsService>>();
                services.AddSingleton<MockAccessor<IEnglishAndMathsQualificationsService>>();
                services.AddSingleton<MockAccessor<IKS2ScaledScoreService>>();
                services.AddSingleton<MockAccessor<IKS2AdditionalMeasuresService>>();
                services.AddSingleton<MockAccessor<IFeatureManager>>();


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
                    return provider.GetRequiredService<MockAccessor<IEstablishmentService>>().Get()?.Object!;
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
                    return provider.GetRequiredService<MockAccessor<IKS4EstablishmentSubjectEntriesService>>().Get()?.Object!;
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
                    return provider.GetRequiredService<MockAccessor<ILevel3QualificationsService>>().Get()?.Object!;
                });
                services.AddTransient(provider =>
                {
                    return provider.GetRequiredService<MockAccessor<ILevel2QualificationsService>>().Get()?.Object!;
                });
                services.AddTransient(provider =>
                {
                    return provider.GetRequiredService<MockAccessor<IEnglishAndMathsQualificationsService>>().Get()?.Object!;
                });
                services.AddTransient(provider =>
                {
                    return provider.GetRequiredService<MockAccessor<IKS2ScaledScoreService>>().Get()?.Object!;
                });
                services.AddTransient(provider =>
                {
                    return provider.GetRequiredService<MockAccessor<IKS2AdditionalMeasuresService>>().Get()?.Object!;
                });
                services.AddTransient<IFeatureManager>(provider =>
                {
                    var accessor = provider.GetRequiredService<MockAccessor<IFeatureManager>>();

                    if (accessor.Get() is null)
                    {
                        // Default: all features enabled unless a test explicitly overrides via UseMock<IFeatureManager>()
                        var defaultMock = new Mock<IFeatureManager>();
                        defaultMock
                            .Setup(f => f.IsEnabledAsync(It.IsAny<string>()))
                            .ReturnsAsync(true);
                        accessor.Set(defaultMock);
                    }

                    return accessor.Object;
                });
            });
    }
}
