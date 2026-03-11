using SAPPub.Core.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Entities.KS4.Absence
{
    [ExcludeFromCodeCoverage]
    public class EstablishmentAbsence
    {
        // Keys / context
        public string Id { get; set; } = string.Empty;

        //
        // Persistent absence %
        //
        public CodedDouble Abs_Persistent_Est_Current_Pct_Coded { get; set; } = new();
        public double? Abs_Persistent_Est_Current_Pct { get; set; }
        public string? Abs_Persistent_Est_Current_Pct_Reason { get; set; }

        public CodedDouble Abs_Persistent_Est_Previous_Pct_Coded { get; set; } = new();
        public double? Abs_Persistent_Est_Previous_Pct { get; set; }
        public string? Abs_Persistent_Est_Previous_Pct_Reason { get; set; }

        public CodedDouble Abs_Persistent_Est_Previous2_Pct_Coded { get; set; } = new();
        public double? Abs_Persistent_Est_Previous2_Pct { get; set; }
        public string? Abs_Persistent_Est_Previous2_Pct_Reason { get; set; }

        //
        // Overall absence %
        //
        public CodedDouble Abs_Tot_Est_Current_Pct_Coded { get; set; } = new();
        public double? Abs_Tot_Est_Current_Pct { get; set; }
        public string? Abs_Tot_Est_Current_Pct_Reason { get; set; }

        public CodedDouble Abs_Tot_Est_Previous_Pct_Coded { get; set; } = new();
        public double? Abs_Tot_Est_Previous_Pct { get; set; }
        public string? Abs_Tot_Est_Previous_Pct_Reason { get; set; }

        public CodedDouble Abs_Tot_Est_Previous2_Pct_Coded { get; set; } = new();
        public double? Abs_Tot_Est_Previous2_Pct { get; set; }
        public string? Abs_Tot_Est_Previous2_Pct_Reason { get; set; }

        //
        // Authorised / Unauthorised absence %
        // (view currently provides current year only)
        //
        public CodedDouble Auth_Tot_Est_Current_Pct_Coded { get; set; } = new();
        public double? Auth_Tot_Est_Current_Pct { get; set; }
        public string? Auth_Tot_Est_Current_Pct_Reason { get; set; }

        public CodedDouble UnAuth_Tot_Est_Current_Pct_Coded { get; set; } = new();
        public double? UnAuth_Tot_Est_Current_Pct { get; set; }
        public string? UnAuth_Tot_Est_Current_Pct_Reason { get; set; }
    }
}
