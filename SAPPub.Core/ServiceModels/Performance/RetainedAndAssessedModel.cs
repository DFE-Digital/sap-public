using SAPPub.Core.Entities;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.ServiceModels.Performance;

public class RetainedAndAssessedModel
{
    public required RelativeYearValues<CodedDouble> NumberOfStudents { get; init; }

    public required RelativeYearValues<CodedDouble> EstablishmentAll { get; init; }

    public required RelativeYearValues<CodedDouble> LocalAuthorityAll { get; init; }

    public required RelativeYearValues<CodedDouble> EnglandAll { get; init; }
}