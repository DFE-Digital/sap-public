using Bogus;
using SAPPub.Core.ServiceModels.KS4.Performance;

namespace SAPPub.Core.Tests.TestBuilders;

public class AdditionalMeasuresBuilder
{
    private Faker _faker = new Faker("en_GB");

    private double? _achievingAtLeastOneQualificationPct;
    private double? _enteredForTripleSciencePct;
    private double? _enteredMoreThanOneForeignLanguagePct;
    private double? _gcseExamEntriesPerPupilNum;
    private double? _allKS4QualificationsExamEntriesPerPupilNum;
    private double? _pupilsAtTheEndOfKS4Num;

    public AdditionalMeasuresBuilder WithAchievingAtLeastOneQualification(double? value)
    {
        _achievingAtLeastOneQualificationPct = value;
        return this;
    }

    public AdditionalMeasuresBuilder WithEnteredForTripleScience(double? value)
    {
        _enteredForTripleSciencePct = value;
        return this;
    }

    public AdditionalMeasuresBuilder WithEnteredMoreThanOneForeignLanguage(double? value)
    {
        _enteredMoreThanOneForeignLanguagePct = value;
        return this;
    }

    public AdditionalMeasuresBuilder WithGCSEExamEntriesPerPupil(double? value)
    {
        _gcseExamEntriesPerPupilNum = value;
        return this;
    }

    public AdditionalMeasuresBuilder WithAllKS4Qualifications(double? value)
    {
        _allKS4QualificationsExamEntriesPerPupilNum = value;
        return this;
    }

    public AdditionalMeasuresBuilder WithPupilsAtTheEndOfKS4(double? value)
    {
        _pupilsAtTheEndOfKS4Num = value;
        return this;
    }

    public AdditionalMeasuresBuilder WithAutoPopulatedValues()
    {
        _achievingAtLeastOneQualificationPct = Math.Round(_faker.Random.Double(10, 100), 1);
        _enteredForTripleSciencePct = Math.Round(_faker.Random.Double(10, 100), 1);
        _enteredMoreThanOneForeignLanguagePct = Math.Round(_faker.Random.Double(10, 100), 1);
        _gcseExamEntriesPerPupilNum = _faker.Random.Double(0, 11);
        _allKS4QualificationsExamEntriesPerPupilNum = Math.Round(_faker.Random.Double(0, 11), 1);
        _pupilsAtTheEndOfKS4Num = Math.Round(_faker.Random.Double(1, 240), 0);
        return this;
    }

    public AdditionalMeasures Build()
    {
        return new AdditionalMeasures
        {
            PercentAchievingAtLeastOneQualification = new ValueObjects.CodedDouble(Value: _achievingAtLeastOneQualificationPct, "", ""),
            PercentEnteredForTripleScience = new ValueObjects.CodedDouble(Value: _enteredForTripleSciencePct, "", ""),
            PercentEnteredMoreThanOneForeignLanguage = new ValueObjects.CodedDouble(Value: _enteredMoreThanOneForeignLanguagePct, "", ""),
            AverageGCSEExamEntriesPerPupil = new ValueObjects.CodedDouble(Value: _gcseExamEntriesPerPupilNum, "", ""),
            AverageAllKS4QualificationsExamEntriesPerPupil = new ValueObjects.CodedDouble(Value: _allKS4QualificationsExamEntriesPerPupilNum, "", ""),
            NumberOfPupilsAtTheEndOfKS4 = new ValueObjects.CodedDouble(Value: _pupilsAtTheEndOfKS4Num, "", "")
        };
    }
}
