using Bogus;
using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Core.ServiceModels.Common;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Tests.TestBuilders;

public class Level3QualificationsModelBuilder
{
    private Faker _faker = new("en_GB");
    private string? _urn;
    private string? _establishmentName;
    private string? _laName;
    private bool _isKs5;
    private Level3? _qualificationType;
    private double? _totalNoOfStudentsCompletedQualification;
    private double? _progressScore;
    private Optional<double?> _englandProgressAverageScore = new Optional<double?>();

    public Level3QualificationsModelBuilder WithUrn(string urn)
    {
        _urn = urn;
        return this;
    }

    public Level3QualificationsModelBuilder WithEstablishmentName(string establishmentName)
    {
        _establishmentName = establishmentName;
        return this;
    }

    public Level3QualificationsModelBuilder WithLAName(string laName)
    {
        _laName = laName;
        return this;
    }

    public Level3QualificationsModelBuilder WithEnglandPercentage(double? englandPercentage)
    {
        _englandProgressAverageScore.SetValue(englandPercentage);
        return this;
    }
        
    public Level3QualificationsModelBuilder WithKS5(bool isKS5)
    {
        _isKs5 = isKS5;
        return this;
    }

    public Level3QualificationsModelBuilder WithQualificationType(Level3 qualificationType)
    {
        _qualificationType = qualificationType;
        return this;
    }

    public Level3QualificationsModelBuilder WithQualificationType(double totalNoOfStudentsCompletedQualification)
    {
        _totalNoOfStudentsCompletedQualification = totalNoOfStudentsCompletedQualification;
        return this;
    }

    public Level3QualificationsModelBuilder WithProgressScore(double progressScore)
    {
        _progressScore = progressScore;
        return this;
    }

    public Level3QualificationModel Build()
    {
        var isAlevelQual = _qualificationType == Level3.ALevel;
        var isAcademicQual = _qualificationType == Level3.Academic;

        return new Level3QualificationModel
        {
            Urn = _urn ?? string.Empty,
            SchoolName = _establishmentName ?? string.Empty,
            LAName = _laName ?? string.Empty,
            IsKS2 = false,
            IsKS4 = false,
            IsKS5 = _isKs5,
            QualificationType = _qualificationType ?? Level3.ALevel,
            TotalNoOfStudentCompletedQualification = new CodedDouble(Value: _totalNoOfStudentsCompletedQualification ?? 150, string.Empty, string.Empty),
            ProgressScore = new ProgressScoreModel()
            {
                Score = new CodedDouble(Value: _progressScore ?? 95.55, string.Empty, string.Empty),
                BandingRating = new CodedString("Average", string.Empty, string.Empty),
                ConfidenceLevelLower = new CodedDouble(1.0, string.Empty, string.Empty),
                ConfidenceLevelUpper = new CodedDouble(5.5, string.Empty, string.Empty),
                EnglandAverageScore = new CodedDouble(Value: _englandProgressAverageScore.IsSet ? _englandProgressAverageScore.Value : 1.5, string.Empty, string.Empty),
            },
            AverageResult = new AverageResultModel
            {
                Establishment = new PerformanceResult
                {
                    Grade = new CodedString("A", string.Empty, string.Empty),
                    Points = new CodedDouble(Math.Round(_faker.Random.Double(10, 100), 1), string.Empty, string.Empty)
                },
                LocalAuthority = new PerformanceResult
                {
                    Grade = new CodedString("B", string.Empty, string.Empty),
                    Points = new CodedDouble(Math.Round(_faker.Random.Double(10, 100), 1), string.Empty, string.Empty)
                },
                England = new PerformanceResult
                {
                    Grade = new CodedString("A", string.Empty, string.Empty),
                    Points = new CodedDouble(Math.Round(_faker.Random.Double(10, 100), 1), string.Empty, string.Empty)
                }
            },
            AdditionalData = isAlevelQual ? new AdditionalDataModel
            {
                TotalNoOfStudentsIncludedInThisMeasure = new CodedDouble(100, string.Empty, string.Empty),
                Establishment = new PerformanceResult
                {
                    Grade = new CodedString("C", string.Empty, string.Empty),
                    Points = new CodedDouble(Math.Round(_faker.Random.Double(10, 100), 1), string.Empty, string.Empty)
                },
                LocalAuthority = new PerformanceResult
                {
                    Grade = new CodedString("A", string.Empty, string.Empty),
                    Points = new CodedDouble(Math.Round(_faker.Random.Double(10, 100), 1), string.Empty, string.Empty)
                },
                England = new PerformanceResult
                {
                    Grade = new CodedString("B", string.Empty, string.Empty),
                    Points = new CodedDouble(Math.Round(_faker.Random.Double(10, 100), 1), string.Empty, string.Empty)
                }
            } : null,
            AdvancedLevelMathsQualificationData = isAcademicQual ? new SimpleCodedDoubleTableModel
            {
                SchoolOrCollege = new CodedDouble(78.94, string.Empty, string.Empty),
                LocalAuthority = new CodedDouble(61.96, string.Empty, string.Empty),
                England = new CodedDouble(73.45, string.Empty, string.Empty),
            } : null
        };
    }
}
