namespace SAPPub.Core.Entities.KS4.SubjectEntries;

public class EstablishmentAdditionalSubjectEntries
{
    public IReadOnlyCollection<SubjectEntry> SubjectEntries { get; set; } = Array.Empty<SubjectEntry>();

    public class SubjectEntry
    {
        // CML - TODO this uses 'SubEntAdd' so can't re-use for core - has to be like this because it's the dynamic mapping bit?
        public string? SubEntAdd_Sub_Est_Current_Num { get; set; }
        public string? SubEntAdd_Qual_Est_Current_Num { get; set; }
        public double? SubEntAdd_Entr_Est_Current_Num { get; set; }
    }
}
