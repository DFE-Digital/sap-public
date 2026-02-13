using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Entities.KS4.SubjectEntries
{
    [ExcludeFromCodeCoverage]
    public sealed class EstablishmentSubjectEntryRow
    {
        public string? school_urn { get; set; }
        public string? subject { get; set; }
        public string? qualification_type { get; set; }
        public string? qualification_detailed { get; set; }
        public string? grade { get; set; }
        public int? number_achieving { get; set; }
    }
}

