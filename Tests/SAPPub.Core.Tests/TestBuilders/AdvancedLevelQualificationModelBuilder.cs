using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Core.ServiceModels.Performance;

namespace SAPPub.Core.Tests.TestBuilders;

public class AdvancedLevelQualificationModelBuilder
{
    private string? _urn;
    private string? _establishmentName;
    private bool _isKs5;
    private Level3? _qualificationType;
    private double? _totalNoOfStudentsCompletedQualification;
    private double? _progressScore;
    private Optional<double?> _englandProgressAverageScore = new Optional<double?>();

    public AdvancedLevelQualificationModelBuilder WithUrn(string urn)
    {
        _urn = urn;
        return this;
    }

    public AdvancedLevelQualificationModelBuilder WithEstablishmentName(string establishmentName)
    {
        _establishmentName = establishmentName;
        return this;
    }
        
    public AdvancedLevelQualificationModelBuilder WithEnglandPercentage(double? englandPercentage)
    {
        _englandProgressAverageScore.SetValue(englandPercentage);
        return this;
    }
        
    public AdvancedLevelQualificationModelBuilder WithKS5(bool isKS5)
    {
        _isKs5 = isKS5;
        return this;
    }

    public AdvancedLevelQualificationModelBuilder WithQualificationType(Level3 qualificationType)
    {
        _qualificationType = qualificationType;
        return this;
    }

    public AdvancedLevelQualificationModelBuilder WithQualificationType(double totalNoOfStudentsCompletedQualification)
    {
        _totalNoOfStudentsCompletedQualification = totalNoOfStudentsCompletedQualification;
        return this;
    }

    public AdvancedLevelQualificationModelBuilder WithProgressScore(double progressScore)
    {
        _progressScore = progressScore;
        return this;
    }

    public AdvancedLevelQualificationModel Build()
    {
        var faker = new Bogus.Faker();
        return new AdvancedLevelQualificationModel
        {
            Urn = _urn ?? string.Empty,
            SchoolName = _establishmentName ?? string.Empty,
            IsKS2 = false,
            IsKS4 = false,
            IsKS5 = _isKs5,
            QualificationType = _qualificationType ?? Level3.ALevel,
            TotalNoOfStudentCompletedQualification = _totalNoOfStudentsCompletedQualification ?? 150,
            ProgressScore = new ProgressScoreModel()
            {
                Score = _progressScore ?? 95.55,
                BandingRating = "Average",
                ConfidenceLevelLower = 1.0,
                ConfidenceLevelUpper = 5.5,
                EnglandAverageScore = _englandProgressAverageScore.IsSet ? _englandProgressAverageScore.Value : 1.5,
            }            
        };
    }
}
