using SAPPub.Core.ValueObjects;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SAPPub.Core.Entities.KS4.Absence
{
    [ExcludeFromCodeCoverage]
    public class EnglandAbsence
    {
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Persistent Absence Total filtered by England for Current year 
        /// </summary>
        public CodedDouble Abs_Persistent_Eng_Current_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? Abs_Persistent_Eng_Current_Pct { get; set; }
        [IgnoreDataMember]
        public string Abs_Persistent_Eng_Current_Pct_Reason { get; set; } = string.Empty;

        /// <summary>
        /// Absence Total filtered by England for Current year 
        /// </summary>
        public CodedDouble Abs_Tot_Eng_Current_Pct_Coded { get; set; } = new();
        [IgnoreDataMember]
        public double? Abs_Tot_Eng_Current_Pct { get; set; }
        [IgnoreDataMember]
        public string Abs_Tot_Eng_Current_Pct_Reason { get; set; } = string.Empty;

    }
}
