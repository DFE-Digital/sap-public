using SAPPub.Core.Entities;
using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Enums;
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
            DisadvantagedStudentsData = GetDisadvantagedStudentsData(level2Qualification, establishmentPerformance, laPerformance, englandPerformance),
            NonDisadvantagedStudentsData = GetNonDisadvantagedStudentsData(level2Qualification, laPerformance, englandPerformance)
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
        var bandingRating = level2Qualification switch
        {
            Level2.TechCert => establishmentPerformance.PROGRESS_BAND_TECHCERT_Est_Current,
            _ => CodedString.Empty,
        };
        var bandingDescriptions = level2Qualification switch
        {
            Level2.TechCert => new ProgressBandingDescriptions(
                WellAboveAverage: englandPerformance.ProgBand_Techcert_Band1_Eng_Current_Desc,
                AboveAverage: englandPerformance.ProgBand_Techcert_Band2_Eng_Current_Desc,
                Average: englandPerformance.ProgBand_Techcert_Band3_Eng_Current_Desc,
                BelowAverage: englandPerformance.ProgBand_Techcert_Band4_Eng_Current_Desc,
                WellBelowAverage: englandPerformance.ProgBand_Techcert_Band5_Eng_Current_Desc),
            _ => ProgressBandingDescriptions.Empty,
        };

        return new ProgressScoreModel
        {
            Score = level2Qualification switch
            {
                Level2.TechCert => establishmentPerformance.VA_INS_TECHCERT_Est_Current_Num_Coded,
                _ => CodedDouble.Empty,
            },
            BandingRating = bandingRating,
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
            BandingContextDescription = bandingRating.Value.GetBandingDescription(bandingDescriptions),
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
            NumberOfStudents = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = level2Qualification switch
                {
                    Level2.TechCert => establishmentPerformance.TALLPUP_TECHCERT_Est_Current_Num_Coded,
                    _ => CodedDouble.Empty
                }
            },
            Establishment = new RelativeYearValues<PerformanceResult>
            {
                CurrentYear = level2Qualification switch
                {
                    Level2.TechCert => MapPerformanceResult(establishmentPerformance.TALLPPEGRD_TECHCERT_Est_Current, establishmentPerformance.TALLPPE_TECHCERT_Est_Current_Num_Coded),
                    _ => MapPerformanceResult(CodedString.Empty, CodedDouble.Empty)
                }
            },
            LocalAuthority = new RelativeYearValues<PerformanceResult>
            {
                CurrentYear = level2Qualification switch
                {
                    Level2.TechCert => MapPerformanceResult(laPerformance.TALLPPEGRD_TECHCERT_LA_Current, laPerformance.TALLPPE_TECHCERT_LA_Current_Num_Coded),
                    _ => MapPerformanceResult(CodedString.Empty, CodedDouble.Empty)
                }                
            },
            England = new RelativeYearValues<PerformanceResult>
            {
                CurrentYear = level2Qualification switch
                {
                    Level2.TechCert => MapPerformanceResult(englandPerformance.TALLPPEGRD_TECHCERT_Eng_Current, englandPerformance.TALLPPE_TECHCERT_Eng_Current_Num_Coded),
                    _ => MapPerformanceResult(CodedString.Empty, CodedDouble.Empty)
                }
            }
        };
    }

    private static PerformanceResult MapPerformanceResult(CodedString grade, CodedDouble points)
    {
        return new PerformanceResult
        {
            Grade = grade,
            Points = points
        };
    }

    private static PerformanceSummaryModel GetDisadvantagedStudentsData(
        Level2 level2Qualification,
        KS5EstablishmentPerformance establishmentPerformance,
        KS5LAPerformance laPerformance,
        KS5EnglandPerformance englandPerformance)
    {
        return new PerformanceSummaryModel
        {
            Establishment = new PerformanceData
            {
                NumberOfStudents = level2Qualification switch
                {
                    Level2.TechCert => establishmentPerformance.TALLPUP_TECHCERT_DIS_Est_Current_Num_Coded,
                    _ => CodedDouble.Empty,
                },
                ProgressScore = level2Qualification switch
                {
                    Level2.TechCert => establishmentPerformance.VA_INS_TECHCERT_DIS_Est_Current_Num_Coded,
                    _ => CodedDouble.Empty,
                },
                ConfidenceLevelUpper = level2Qualification switch
                {
                    Level2.TechCert => establishmentPerformance.UCI_INS_TECHCERT_DIS_Est_Current_Num_Coded,
                    _ => CodedDouble.Empty,
                },
                ConfidenceLevelLower = level2Qualification switch
                {
                    Level2.TechCert => establishmentPerformance.LCI_INS_TECHCERT_DIS_Est_Current_Num_Coded,
                    _ => CodedDouble.Empty,
                },
                Result = new PerformanceResult
                {
                    Points = level2Qualification switch
                    {
                        Level2.TechCert => establishmentPerformance.TALLPPE_TECHCERT_DIS_Est_Current_Num_Coded,
                        _ => CodedDouble.Empty,
                    },
                    Grade = level2Qualification switch
                    {
                        Level2.TechCert => establishmentPerformance.TALLPPEGRD_TECHCERT_DIS_Est_Current,
                        _ => CodedString.Empty,
                    }
                }
            },
            LocalAuthority = new PerformanceData
            {
                NumberOfStudents = level2Qualification switch
                {
                    Level2.TechCert => laPerformance.TALLPUP_TECHCERT_DIS_LA_Current_Num_Coded,
                    _ => CodedDouble.Empty,
                },
                ProgressScore = level2Qualification switch
                {
                    Level2.TechCert => laPerformance.VA_INS_TECHCERT_DIS_LA_Current_Num_Coded,
                    _ => CodedDouble.Empty,
                },
                ConfidenceLevelUpper = level2Qualification switch
                {
                    Level2.TechCert => laPerformance.UCI_INS_TECHCERT_DIS_LA_Current_Num_Coded,
                    _ => CodedDouble.Empty,
                },
                ConfidenceLevelLower = level2Qualification switch
                {
                    Level2.TechCert => laPerformance.LCI_INS_TECHCERT_DIS_LA_Current_Num_Coded,
                    _ => CodedDouble.Empty,
                },
                Result = new PerformanceResult
                {
                    Points = level2Qualification switch
                    {
                        Level2.TechCert => laPerformance.TALLPPE_TECHCERT_DIS_LA_Current_Num_Coded,
                        _ => CodedDouble.Empty,
                    },
                    Grade = level2Qualification switch
                    {
                        Level2.TechCert => laPerformance.TALLPPEGRD_TECHCERT_DIS_LA_Current,
                        _ => CodedString.Empty,
                    }
                }
            },
            England = new PerformanceData
            {
                NumberOfStudents = level2Qualification switch
                {
                    Level2.TechCert => englandPerformance.TALLPUP_TECHCERT_DIS_Eng_Current_Num_Coded,
                    _ => CodedDouble.Empty,
                },
                ProgressScore = level2Qualification switch
                {
                    Level2.TechCert => englandPerformance.VA_INS_TECHCERT_DIS_Eng_Current_Num_Coded,
                    _ => CodedDouble.Empty,
                },
                ConfidenceLevelUpper = level2Qualification switch
                {
                    Level2.TechCert => englandPerformance.UCI_INS_TECHCERT_DIS_Eng_Current_Num_Coded,
                    _ => CodedDouble.Empty,
                },
                ConfidenceLevelLower = level2Qualification switch
                {
                    Level2.TechCert => englandPerformance.LCI_INS_TECHCERT_DIS_Eng_Current_Num_Coded,
                    _ => CodedDouble.Empty,
                },
                Result = new PerformanceResult
                {
                    Points = level2Qualification switch
                    {
                        Level2.TechCert => englandPerformance.TALLPPE_TECHCERT_DIS_Eng_Current_Num_Coded,
                        _ => CodedDouble.Empty,
                    },
                    Grade = level2Qualification switch
                    {
                        Level2.TechCert => englandPerformance.TALLPPEGRD_TECHCERT_DIS_Eng_Current,
                        _ => CodedString.Empty,
                    }
                }
            },
        };
    }

    private static PerformanceSummaryModel GetNonDisadvantagedStudentsData(
        Level2 level2Qualification,
        KS5LAPerformance laPerformance,
        KS5EnglandPerformance englandPerformance)
    {
        return new PerformanceSummaryModel
        {
            LocalAuthority = new PerformanceData
            {
                NumberOfStudents = level2Qualification switch
                {
                    Level2.TechCert => laPerformance.TALLPUP_TECHCERT_NOTDIS_LA_Current_Num_Coded,
                    _ => CodedDouble.Empty,
                },
                ProgressScore = level2Qualification switch
                {
                    Level2.TechCert => laPerformance.VA_INS_TECHCERT_NOTDIS_LA_Current_Num_Coded,
                    _ => CodedDouble.Empty,
                },
                ConfidenceLevelUpper = level2Qualification switch
                {
                    Level2.TechCert => laPerformance.UCI_INS_TECHCERT_NOTDIS_LA_Current_Num_Coded,
                    _ => CodedDouble.Empty,
                },
                ConfidenceLevelLower = level2Qualification switch
                {
                    Level2.TechCert => laPerformance.LCI_INS_TECHCERT_NOTDIS_LA_Current_Num_Coded,
                    _ => CodedDouble.Empty,
                },
                Result = new PerformanceResult
                {
                    Points = level2Qualification switch
                    {
                        Level2.TechCert => laPerformance.TALLPPE_TECHCERT_NOTDIS_LA_Current_Num_Coded,
                        _ => CodedDouble.Empty,
                    },
                    Grade = level2Qualification switch
                    {
                        Level2.TechCert => laPerformance.TALLPPEGRD_TECHCERT_NOTDIS_LA_Current,
                        _ => CodedString.Empty,
                    }
                }
            },
            England = new PerformanceData
            {
                NumberOfStudents = level2Qualification switch
                {
                    Level2.TechCert => englandPerformance.TALLPUP_TECHCERT_NOTDIS_Eng_Current_Num_Coded,
                    _ => CodedDouble.Empty,
                },
                ProgressScore = level2Qualification switch
                {
                    Level2.TechCert => englandPerformance.VA_INS_TECHCERT_NOTDIS_Eng_Current_Num_Coded,
                    _ => CodedDouble.Empty,
                },
                ConfidenceLevelUpper = level2Qualification switch
                {
                    Level2.TechCert => englandPerformance.UCI_INS_TECHCERT_NOTDIS_Eng_Current_Num_Coded,
                    _ => CodedDouble.Empty,
                },
                ConfidenceLevelLower = level2Qualification switch
                {
                    Level2.TechCert => englandPerformance.LCI_INS_TECHCERT_NOTDIS_Eng_Current_Num_Coded,
                    _ => CodedDouble.Empty,
                },
                Result = new PerformanceResult
                {
                    Points = level2Qualification switch
                    {
                        Level2.TechCert => englandPerformance.TALLPPE_TECHCERT_NOTDIS_Eng_Current_Num_Coded,
                        _ => CodedDouble.Empty,
                    },
                    Grade = level2Qualification switch
                    {
                        Level2.TechCert => englandPerformance.TALLPPEGRD_TECHCERT_NOTDIS_Eng_Current,
                        _ => CodedString.Empty,
                    }
                }
            },
        };
    }
}
