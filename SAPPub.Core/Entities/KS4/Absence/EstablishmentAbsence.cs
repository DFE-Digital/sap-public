using SAPPub.Core.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Entities.KS4.Absence
{
    [ExcludeFromCodeCoverage]
    public class EstablishmentAbsence
    {
        // Keys / context
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Total Enrolments
        /// </summary>
        public CodedDouble Enrolments_Tot_Est_Current_Num_Coded { get; set; } = new();
        public double? Enrolments_Tot_Est_Current_Num { get; set; }
        public string? Enrolments_Tot_Est_Current_Num_Reason { get; set; }

        //
        // Persistent absence %
        //
        public CodedDouble Abs_Persistent_Est_Current_Num_Coded { get; set; } = new();
        public double? Abs_Persistent_Est_Current_Num { get; set; }
        public string? Abs_Persistent_Est_Current_Num_Reason { get; set; }

        public CodedDouble Abs_Persistent_Est_Current_Pct_Coded { get; set; } = new();
        public double? Abs_Persistent_Est_Current_Pct { get; set; }
        public string? Abs_Persistent_Est_Current_Pct_Reason { get; set; }

        //
        // Overall absence %
        //
        public CodedDouble Abs_Tot_Est_Current_Pct_Coded { get; set; } = new();
        public double? Abs_Tot_Est_Current_Pct { get; set; }
        public string? Abs_Tot_Est_Current_Pct_Reason { get; set; }
    }
}
