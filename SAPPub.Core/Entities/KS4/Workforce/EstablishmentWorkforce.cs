using SAPPub.Core.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Entities.KS4.Workforce
{
    [ExcludeFromCodeCoverage]
    public class EstablishmentWorkforce
    {
        public string Id { get; set; } = string.Empty;


        ///
        /// Workforce Pupil Teacher Ratio filtered by Establishment for Current year
        ///
        public CodedDouble Workforce_PupTeaRatio_Est_Current_Num_Coded { get; set; } = new();
        public double? Workforce_PupTeaRatio_Est_Current_Num { get; set; }
        public string? Workforce_PupTeaRatio_Est_Current_Num_Reason { get; set; }

        ///
        /// Workforce Total Pupils filtered by Establishment for Current year
        ///
        public CodedDouble Workforce_TotPupils_Est_Current_Num_Coded { get; set; } = new();
        public double? Workforce_TotPupils_Est_Current_Num { get; set; }
        public string? Workforce_TotPupils_Est_Current_Num_Reason { get; set; }
    }
}
