namespace SAPPub.Core.ServiceModels;

public enum LinkType
{
    Predecessor = 1, // Predecessor covers all the predecessor categories
    //PredecessorAmalgamated = 2,
    //PredecessorMerged = 3,
    //PredecessorSplitSchool = 4,
    Successor = 5, // Successor covers all the successor categories
    //SuccessorAmalgamated = 6,
    //SuccessorMerged = 7,
    //SuccessorSplitSchool = 8
}

public class EstablishmentLink
{
    public required string Urn { get; set; }
    public LinkType LinkType { get; set; }
}

public class EstablishmentLinks
{
    public List<EstablishmentLink>? PredecessorLinks { get; set; } = new();
    public List<EstablishmentLink>? SuccessorLinks { get; set; } = new();
}

public static class AcademicYearsHelper
{
    public static bool IsWithinLastThreeAcademicYears(DateOnly openDate)
    {
        var today = DateTime.Today;
        var previousSept12 = (today.Month < 9 || (today.Month == 9 && today.Day < 12))
            ? new DateTime(today.Year - 1, 9, 12)
            : new DateTime(today.Year, 9, 12);

        var cutoffDate = previousSept12.AddYears(-3);
        return openDate.ToDateTime(TimeOnly.MinValue) >= cutoffDate;
    }
}