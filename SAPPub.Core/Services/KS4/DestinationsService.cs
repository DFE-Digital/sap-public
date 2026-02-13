using SAPPub.Core.Entities;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4;
using SAPPub.Core.Interfaces.Services.KS4.Destinations;

namespace SAPPub.Core.Services.KS4;

public class DestinationsService(
    IEstablishmentService establishmentService,
    IEstablishmentDestinationsService establishmentDestinationsService,
    ILADestinationsService lADestinationsService,
    IEnglandDestinationsService englandDestinationsService) : IDestinationsService
{
    public DestinationsDetails GetDestinationsDetails(string urn)
    {
        var establishment = establishmentService.GetEstablishment(urn);
        var establishmentDestinations = establishmentDestinationsService.GetEstablishmentDestinations(urn);
        var lADestinations = lADestinationsService.GetLADestinations(establishment.LAId);
        var englandDestinations = englandDestinationsService.GetEnglandDestinations();
        
        var model = new DestinationsDetails
        {
            Urn = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            LocalAuthorityName = establishment.LAName,
            SchoolAll = new RelativeYearValues<double?>
            {
                CurrentYear = establishmentDestinations.AllDest_Tot_Est_Current_Pct,
                PreviousYear = establishmentDestinations.AllDest_Tot_Est_Previous_Pct,
                TwoYearsAgo = establishmentDestinations.AllDest_Tot_Est_Previous2_Pct,
            },
            LocalAuthorityAll = new RelativeYearValues<double?>
            {
                CurrentYear = lADestinations.AllDest_Tot_LA_Current_Pct,
                PreviousYear = lADestinations.AllDest_Tot_LA_Previous_Pct,
                TwoYearsAgo = lADestinations.AllDest_Tot_LA_Previous2_Pct,
            },
            EnglandAll = new RelativeYearValues<double?>
            {
                CurrentYear = englandDestinations.AllDest_Tot_Eng_Current_Pct,
                PreviousYear = englandDestinations.AllDest_Tot_Eng_Previous_Pct,
                TwoYearsAgo = englandDestinations.AllDest_Tot_Eng_Previous2_Pct,
            },
            SchoolEducation = new RelativeYearValues<double?>
            {
                CurrentYear = establishmentDestinations.Education_Tot_Est_Current_Pct,
                PreviousYear = establishmentDestinations.Education_Tot_Est_Previous_Pct,
                TwoYearsAgo = establishmentDestinations.Education_Tot_Est_Previous2_Pct,
            },
            LocalAuthorityEducation = new RelativeYearValues<double?>
            {
                CurrentYear = lADestinations.Education_Tot_LA_Current_Pct,
                PreviousYear = lADestinations.Education_Tot_LA_Previous_Pct,
                TwoYearsAgo = lADestinations.Education_Tot_LA_Previous2_Pct,
            },
            EnglandEducation = new RelativeYearValues<double?>
            {
                CurrentYear = englandDestinations.Education_Tot_Eng_Current_Pct,
                PreviousYear = englandDestinations.Education_Tot_Eng_Previous_Pct,
                TwoYearsAgo = englandDestinations.Education_Tot_Eng_Previous2_Pct,
            },
            SchoolEmployment = new RelativeYearValues<double?>
            {
                CurrentYear = establishmentDestinations.Employment_Tot_Est_Current_Pct,
                PreviousYear = establishmentDestinations.Employment_Tot_Est_Previous_Pct,
                TwoYearsAgo = establishmentDestinations.Employment_Tot_Est_Previous2_Pct,
            },
            LocalAuthorityEmployment = new RelativeYearValues<double?>
            {
                CurrentYear = lADestinations.Employment_Tot_LA_Current_Pct,
                PreviousYear = lADestinations.Employment_Tot_LA_Previous_Pct,
                TwoYearsAgo = lADestinations.Employment_Tot_LA_Previous2_Pct,
            },
            EnglandEmployment = new RelativeYearValues<double?>
            {
                CurrentYear = englandDestinations.Employment_Tot_Eng_Current_Pct,
                PreviousYear = englandDestinations.Employment_Tot_Eng_Previous_Pct,
                TwoYearsAgo = englandDestinations.Employment_Tot_Eng_Previous2_Pct,
            },
            SchoolApprentice = new RelativeYearValues<double?>
            {
                CurrentYear = establishmentDestinations.Apprentice_Tot_Est_Current_Pct,
                PreviousYear = establishmentDestinations.Apprentice_Tot_Est_Previous_Pct,
                TwoYearsAgo = establishmentDestinations.Apprentice_Tot_Est_Previous2_Pct,
            },
            LocalAuthorityApprentice = new RelativeYearValues<double?>
            {
                CurrentYear = lADestinations.Apprentice_Tot_LA_Current_Pct,
                PreviousYear = lADestinations.Apprentice_Tot_LA_Previous_Pct,
                TwoYearsAgo = lADestinations.Apprentice_Tot_LA_Previous2_Pct,
            },
            EnglandApprentice = new RelativeYearValues<double?>
            {
                CurrentYear = englandDestinations.Apprentice_Tot_Eng_Current_Pct,
                PreviousYear = englandDestinations.Apprentice_Tot_Eng_Previous_Pct,
                TwoYearsAgo = englandDestinations.Apprentice_Tot_Eng_Previous2_Pct,
            },
        };

        return model;
    }
}
