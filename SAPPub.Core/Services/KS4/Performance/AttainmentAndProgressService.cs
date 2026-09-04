using SAPPub.Core.Entities;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Services.KS4.Performance;

public class AttainmentAndProgressService(
    IEstablishmentService establishmentService,
    IEstablishmentPerformanceService establishmentPerformanceService,
    ILAPerformanceService lAPerformanceService,
    IEnglandPerformanceService englandPerformanceService) : IAttainmentAndProgressService
{
    public async Task<AttainmentAndProgressModel> GetAttainmentAndProgressAsync(
        string urn,
        CancellationToken ct = default)
    {
        // Need establishment first to get LAId/LAName (and to check if URN is valid)
        var establishment = await establishmentService.GetEstablishmentMinimumAsync(urn, ct);

        if (string.IsNullOrWhiteSpace(establishment.URN))
            return new AttainmentAndProgressModel { 
                Urn = urn,
                IsKS2 = false,
                IsKS4 = false,
                IsKS5 = false,
                EnglandAttainment8DisadvantagedScore = EmptyRelativeYearValues,
                EnglandAttainment8Score = EmptyRelativeYearValues,
                EstablishmentAttainment8DisadvantagedScore = EmptyRelativeYearValues,
                EstablishmentAttainment8Score = EmptyRelativeYearValues,
                EstablishmentProgress8Banding = new RelativeYearValues<string?> { CurrentYear = null, PreviousYear = null, TwoYearsAgo = null },
                EstablishmentProgress8CILower = EmptyRelativeYearValues,
                EstablishmentProgress8CIUpper = EmptyRelativeYearValues,
                EstablishmentProgress8Score = EmptyRelativeYearValues,
                EstablishmentProgress8TotalPupils = EmptyRelativeYearValues,
                EstablishmentTotalPupils = EmptyRelativeYearValues,
                LocalAuthorityAttainment8DisadvantagedScore = EmptyRelativeYearValues,
                LocalAuthorityAttainment8Score = EmptyRelativeYearValues,
                LocalAuthorityProgress8Score = EmptyRelativeYearValues
            };

        // Now we can run the remaining calls concurrently
        var establishmentPerformance = await establishmentPerformanceService.GetEstablishmentPerformanceAsync(urn, ct);

        var laId = establishment.LAId ?? string.Empty;
        var laPerformance = await lAPerformanceService.GetLAPerformanceAsync(laId, ct);

        var englandPerformance = await englandPerformanceService.GetEnglandPerformanceAsync(ct);

        return new AttainmentAndProgressModel
        {
            Urn = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            IsKS2 = establishment.IsKS2,
            IsKS4 = establishment.IsKS4,
            IsKS5 = establishment.IsKS5,
            EstablishmentProgress8Score = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = establishmentPerformance.Prog8_Tot_Est_Current_Num_Coded,
                PreviousYear = establishmentPerformance.Prog8_Tot_Est_Previous_Num_Coded,
                TwoYearsAgo = establishmentPerformance.Prog8_Tot_Est_Previous2_Num_Coded,
            },
            EstablishmentProgress8CILower = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = establishmentPerformance.Prog8_CI_Lower_Est_Current_Num_Coded,
                PreviousYear = establishmentPerformance.Prog8_CI_Lower_Est_Previous_Num_Coded,
                TwoYearsAgo = establishmentPerformance.Prog8_CI_Lower_Est_Previous2_Num_Coded,
            },
            EstablishmentProgress8CIUpper = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = establishmentPerformance.Prog8_CI_Upper_Est_Current_Num_Coded,
                PreviousYear = establishmentPerformance.Prog8_CI_Upper_Est_Previous_Num_Coded,
                TwoYearsAgo = establishmentPerformance.Prog8_CI_Upper_Est_Previous2_Num_Coded,
            },
            EstablishmentProgress8Banding = new RelativeYearValues<string?>
            {
                CurrentYear = establishmentPerformance.Prog8_Banding_Est_Current,
                PreviousYear = establishmentPerformance.Prog8_Banding_Est_Previous,
                TwoYearsAgo = establishmentPerformance.Prog8_Banding_Est_Previous2,
            },
            LocalAuthorityProgress8Score = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = laPerformance.Prog8_Avg_LA_Current_Num_Coded,
                PreviousYear = laPerformance.Prog8_Avg_LA_Previous_Num_Coded,
                TwoYearsAgo = laPerformance.Prog8_Avg_LA_Previous2_Num_Coded,
            },
            EstablishmentAttainment8Score = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = establishmentPerformance.Attainment8_Tot_Est_Current_Num_Coded,
                PreviousYear = establishmentPerformance.Attainment8_Tot_Est_Previous_Num_Coded,
                TwoYearsAgo = establishmentPerformance.Attainment8_Tot_Est_Previous2_Num_Coded,
            },
            EstablishmentAttainment8DisadvantagedScore = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = establishmentPerformance.Attainment8_Dis_Est_Current_Num_Coded,
                PreviousYear = establishmentPerformance.Attainment8_Dis_Est_Previous_Num_Coded,
                TwoYearsAgo = establishmentPerformance.Attainment8_Dis_Est_Previous2_Num_Coded,
            },
            LocalAuthorityAttainment8Score = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = laPerformance.Attainment8_Tot_LA_Current_Num_Coded,
                PreviousYear = laPerformance.Attainment8_Tot_LA_Previous_Num_Coded,
                TwoYearsAgo = laPerformance.Attainment8_Tot_LA_Previous2_Num_Coded,
            },
            LocalAuthorityAttainment8DisadvantagedScore = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = laPerformance.Attainment8_Dis_LA_Current_Num_Coded,
                PreviousYear = laPerformance.Attainment8_Dis_LA_Previous_Num_Coded,
                TwoYearsAgo = laPerformance.Attainment8_Dis_LA_Previous2_Num_Coded,
            },
            EnglandAttainment8Score = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = englandPerformance.Attainment8_Tot_Eng_Current_Num_Coded,
                PreviousYear = englandPerformance.Attainment8_Tot_Eng_Previous_Num_Coded,
                TwoYearsAgo = englandPerformance.Attainment8_Tot_Eng_Previous2_Num_Coded,
            },
            EnglandAttainment8DisadvantagedScore = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = englandPerformance.Attainment8_Dis_Eng_Current_Num_Coded,
                PreviousYear = englandPerformance.Attainment8_Dis_Eng_Previous_Num_Coded,
                TwoYearsAgo = englandPerformance.Attainment8_Dis_Eng_Previous2_Num_Coded,
            },
            EstablishmentProgress8TotalPupils = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = establishmentPerformance.Prog8_TotPup_Est_Current_Num_Coded,
                PreviousYear = establishmentPerformance.Prog8_TotPup_Est_Previous_Num_Coded,
                TwoYearsAgo = establishmentPerformance.Prog8_TotPup_Est_Previous2_Num_Coded,
            },
            EstablishmentTotalPupils = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = establishmentPerformance.Pup_Tot_Est_Current_Num_Coded,
                PreviousYear = establishmentPerformance.Pup_Tot_Est_Previous_Num_Coded,
                TwoYearsAgo = establishmentPerformance.Pup_Tot_Est_Previous2_Num_Coded,
            },
            LocalAuthorityAttainment8NonDisadvantagedScore = laPerformance.Attainment8_NDi_LA_Current_Num_Coded,
            EnglandAttainment8NonDisadvantagedScore = englandPerformance.Attainment8_NDi_Eng_Current_Num_Coded,
        };
    }
    private readonly RelativeYearValues<CodedDouble> EmptyRelativeYearValues = new RelativeYearValues<CodedDouble>
    {
        CurrentYear = CodedDouble.Empty,
        PreviousYear = CodedDouble.Empty,
        TwoYearsAgo = CodedDouble.Empty,
    };
}
