namespace SAPPub.Core.Entities.KS4.SubjectEntries;

public class EstablishmentCoreSubjectEntries
{
    public IReadOnlyCollection<SubjectEntry> SubjectEntries { get; set; } = Array.Empty<SubjectEntry>();

    public class SubjectEntry
    {
        // CML - TODO this uses 'SubEntCore' so can't re-use for non-core - has to be like this because it's the dynamic mapping bit?
        public string? SubEntCore_Sub_Est_Current_Num { get; set; }
        public string? SubEntCore_Qual_Est_Current_Num { get; set; }
        public double? SubEntCore_Entr_Est_Current_Num { get; set; }
    }
}
