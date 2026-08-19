using Bogus;
using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Tests.TestBuilders;

public class Level2QualificationsModelBuilder
{
    private Faker _faker = new("en_GB");
    private string? _urn;
    private string? _establishmentName;
    private string? _laName;
    private bool _isKs5;
    private Level2? _qualificationType;
    private double? _totalNoOfStudentsCompletedQualification;
    private double? _progressScore;
    private Optional<double?> _englandProgressAverageScore = new Optional<double?>();

    public Level2QualificationsModelBuilder WithUrn(string urn)
    {
        _urn = urn;
        return this;
    }

    public Level2QualificationsModelBuilder WithEstablishmentName(string establishmentName)
    {
        _establishmentName = establishmentName;
        return this;
    }

    public Level2QualificationsModelBuilder WithLAName(string laName)
    {
        _laName = laName;
        return this;
    }

    public Level2QualificationsModelBuilder WithEnglandPercentage(double? englandPercentage)
    {
        _englandProgressAverageScore.SetValue(englandPercentage);
        return this;
    }

    public Level2QualificationsModelBuilder WithKS5(bool isKS5)
    {
        _isKs5 = isKS5;
        return this;
    }

    public Level2QualificationsModelBuilder WithQualificationType(Level2 qualificationType)
    {
        _qualificationType = qualificationType;
        return this;
    }

    public Level2QualificationsModelBuilder WithQualificationType(double totalNoOfStudentsCompletedQualification)
    {
        _totalNoOfStudentsCompletedQualification = totalNoOfStudentsCompletedQualification;
        return this;
    }

    public Level2QualificationsModelBuilder WithProgressScore(double progressScore)
    {
        _progressScore = progressScore;
        return this;
    }

    public Level2QualificationModel Build()
    {
        return new Level2QualificationModel
        {
            Urn = _urn ?? string.Empty,
            SchoolName = _establishmentName ?? string.Empty,
            LAName = _laName ?? string.Empty,
            IsKS2 = false,
            IsKS4 = false,
            IsKS5 = _isKs5,
            QualificationType = _qualificationType ?? Level2.TechCert,
            TotalNoOfStudentCompletedQualification = new CodedDouble(Value: _totalNoOfStudentsCompletedQualification ?? 125, string.Empty, string.Empty),
            ProgressScore = new ProgressScoreModel()
            {
                Score = new CodedDouble(Value: _progressScore ?? 83.29, string.Empty, string.Empty),
                BandingRating = new CodedString("Above Average", string.Empty, string.Empty),
                ConfidenceLevelLower = new CodedDouble(0.1, string.Empty, string.Empty),
                ConfidenceLevelUpper = new CodedDouble(2.7, string.Empty, string.Empty),
                EnglandAverageScore = new CodedDouble(Value: _englandProgressAverageScore.IsSet ? _englandProgressAverageScore.Value : 2.3, string.Empty, string.Empty),
            },
            AverageResult = new AverageResultModel
            {
                Establishment = new PerformanceResult
                {
                    Grade = new CodedString("C", string.Empty, string.Empty),
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
            }
        };
    }
}
