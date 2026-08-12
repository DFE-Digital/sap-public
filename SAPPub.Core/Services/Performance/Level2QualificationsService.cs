using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Core.Interfaces.Repositories.Performance;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Services.Performance;

public class Level2QualificationsService(
    IEstablishmentService establishmentService,
    IKs5PerformanceRepository ks5PerformanceRepository) : ILevel2QualificationsService
{
    public async Task<Level2QualificationModel> GetLevel2QualificationDetailsAsync(
        string urn,
        Level2 level2Qualification,
        CancellationToken ct = default)
    {
        var establishment = await establishmentService.GetEstablishmentAsync(urn, ct);
        var establishmentPerformanceTask = ks5PerformanceRepository.GetEstablishmentPerformanceAsync(urn, ct);
        var englandPerformanceTask = ks5PerformanceRepository.GetEnglandPerformanceAsync(ct);
        var laPerformanceTask = ks5PerformanceRepository.GetLaPerformanceAsync(establishment.LAId, ct);

        await Task.WhenAll(establishmentPerformanceTask, englandPerformanceTask, laPerformanceTask);

        var establishmentPerformance = await establishmentPerformanceTask;
        var englandPerformance = await englandPerformanceTask;
        var laPerformance = await laPerformanceTask;

        return new Level2QualificationModel
        {
            Urn = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            LAName = establishment.LAName,
            IsKS2 = establishment.IsKS2,
            IsKS4 = establishment.IsKS4,
            IsKS5 = establishment.IsKS5,
            QualificationType = level2Qualification,
            TotalNoOfStudentCompletedQualification = GetTotalNoOfStudentsCompletedQualification(level2Qualification, establishmentPerformance),
            ProgressScore = GetProgressScoreModel(level2Qualification, establishmentPerformance, englandPerformance),
            AverageResult = GetAverageResultModel(level2Qualification, establishmentPerformance, englandPerformance, laPerformance),
        };
    }

    private static CodedDouble GetTotalNoOfStudentsCompletedQualification(
        Level2 level2Qualification,
        KS5EstablishmentPerformance establishmentPerformance)
    {
        return level2Qualification switch
        {
            Level2.TechCert => establishmentPerformance.TALLPUP_TECHCERT_Est_Current_Num_Coded,
            _ => CodedDouble.Empty,
        };
    }

    private static ProgressScoreModel GetProgressScoreModel(
        Level2 level2Qualification,
        KS5EstablishmentPerformance establishmentPerformance,
        KS5EnglandPerformance englandPerformance)
    {
        return new ProgressScoreModel
        {
            Score = level2Qualification switch
            {
                Level2.TechCert => establishmentPerformance.VA_INS_TECHCERT_Est_Current_Num_Coded,
                _ => CodedDouble.Empty,
            },
            BandingRating = level2Qualification switch
            {
                Level2.TechCert => establishmentPerformance.PROGRESS_BAND_TECHCERT_Est_Current,
                _ => CodedString.Empty,
            },
            ConfidenceLevelUpper = level2Qualification switch
            {
                Level2.TechCert => establishmentPerformance.UCI_INS_TECHCERT_Est_Current_Num_Coded,
                _ => CodedDouble.Empty,
            },
            ConfidenceLevelLower = level2Qualification switch
            {
                Level2.TechCert => establishmentPerformance.LCI_INS_TECHCERT_Est_Current_Num_Coded,
                _ => CodedDouble.Empty,
            },
            EnglandAverageScore = level2Qualification switch
            {
                Level2.TechCert => englandPerformance.VA_INS_TECHCERT_Eng_Current_Num_Coded,
                _ => CodedDouble.Empty,
            },
        };
    }

    private static AverageResultModel GetAverageResultModel(
        Level2 level2Qualification,
        KS5EstablishmentPerformance establishmentPerformance,
        KS5EnglandPerformance englandPerformance,
        KS5LAPerformance laPerformance)
    {
        return new AverageResultModel
        {
            Establishment = new AverageResult
            {
                Points = level2Qualification switch
                {
                    Level2.TechCert => establishmentPerformance.TALLPPE_TECHCERT_Est_Current_Num_Coded,
                    _ => CodedDouble.Empty,
                },
                Grade = level2Qualification switch
                {
                    Level2.TechCert => establishmentPerformance.TALLPPEGRD_TECHCERT_Est_Current,
                    _ => CodedString.Empty,
                },
            },
            LocalAuthority = new AverageResult
            {
                Points = level2Qualification switch
                {
                    Level2.TechCert => laPerformance.TALLPPE_TECHCERT_LA_Current_Num_Coded,
                    _ => CodedDouble.Empty,
                },
                Grade = level2Qualification switch
                {
                    Level2.TechCert => laPerformance.TALLPPEGRD_TECHCERT_LA_Current,
                    _ => CodedString.Empty,
                },
            },
            England = new AverageResult
            {
                Points = level2Qualification switch
                {
                    Level2.TechCert => englandPerformance.TALLPPE_TECHCERT_Eng_Current_Num_Coded,
                    _ => CodedDouble.Empty,
                },
                Grade = level2Qualification switch
                {
                    Level2.TechCert => englandPerformance.TALLPPEGRD_TECHCERT_Eng_Current,
                    _ => CodedString.Empty,
                },
            }
        };
    }
}
