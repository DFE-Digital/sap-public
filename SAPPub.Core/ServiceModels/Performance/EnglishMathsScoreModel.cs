using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.ServiceModels.Performance;

public class EnglishMathsScoreModel
{
    public required CodedDouble NumberOfStudents { get; init; }

    public required CodedDouble SchoolOrCollege { get; init; }

    public required CodedDouble LaAverage { get; init; }

    public required CodedDouble EnglandAverage { get; init; }
}
