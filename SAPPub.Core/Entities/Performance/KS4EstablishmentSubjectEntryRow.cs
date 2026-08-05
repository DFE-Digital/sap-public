using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Entities.Performance
{
    [ExcludeFromCodeCoverage]
    public sealed class KS4EstablishmentSubjectEntryRow
    {
        public string? school_urn { get; set; }
        public string? pupil_count { get; set; }
        public string? subject { get; set; }
        public string? subject_discount_group { get; set; }
        public string? qualification_type { get; set; }
        public string? qualification_detailed { get; set; }
        public string? grade { get; set; }
        public string? number_achieving { get; set; }
    }
}
