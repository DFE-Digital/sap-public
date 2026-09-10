using Bogus;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Core.ValueObjects;

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
    private double? _averageGCSEExamEntriesPerDisadvantagedPupil;
    private double? _averageKS4ExamEntriesPerDisadvantagedPupil;
    private double? _averageGCSEExamEntriesPerNonDisadvantagedPupil;
    private double? _averageKS4ExamEntriesPerNonDisadvantagedPupil;

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
        _averageGCSEExamEntriesPerDisadvantagedPupil = Math.Round(_faker.Random.Double(10, 100), 1);
        _averageKS4ExamEntriesPerDisadvantagedPupil = Math.Round(_faker.Random.Double(10, 100), 1);
        _averageGCSEExamEntriesPerNonDisadvantagedPupil = Math.Round(_faker.Random.Double(10, 100), 1);
        _averageKS4ExamEntriesPerNonDisadvantagedPupil = Math.Round(_faker.Random.Double(10, 100), 1);
        return this;
    }

    public AdditionalMeasures Build()
    {
        return new AdditionalMeasures
        {
            PercentAchievingAtLeastOneQualification = GetCodedDouble(_achievingAtLeastOneQualificationPct),
            PercentEnteredForTripleScience = GetCodedDouble(_enteredForTripleSciencePct),
            PercentEnteredMoreThanOneForeignLanguage = GetCodedDouble(_enteredMoreThanOneForeignLanguagePct),
            AverageGCSEExamEntriesPerPupil = GetCodedDouble(_gcseExamEntriesPerPupilNum),
            AverageAllKS4QualificationsExamEntriesPerPupil = GetCodedDouble(_allKS4QualificationsExamEntriesPerPupilNum),
            NumberOfPupilsAtTheEndOfKS4 = GetCodedDouble(_pupilsAtTheEndOfKS4Num),
            AverageGCSEExamEntriesPerDisadvantagedPupil = GetCodedDouble(_averageGCSEExamEntriesPerDisadvantagedPupil),
            AverageAllKS4QualificationsExamEntriesPerDisadvantagedPupil = GetCodedDouble(_averageKS4ExamEntriesPerDisadvantagedPupil),
            AverageGCSEExamEntriesPerNonDisadvantagedPupil = GetCodedDouble(_averageGCSEExamEntriesPerNonDisadvantagedPupil),
            AverageAllKS4QualificationsExamEntriesPerNonDisadvantagedPupil = GetCodedDouble(_averageKS4ExamEntriesPerNonDisadvantagedPupil)
        };
    }

    private static CodedDouble GetCodedDouble(double? val)
    {
        return new CodedDouble(val!, string.Empty, val is null ? null! : val.ToString()!);
    }
}
