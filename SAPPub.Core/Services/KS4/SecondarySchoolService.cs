using SAPPub.Core.Entities;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4;
using SAPPub.Core.Interfaces.Services.KS4.Destinations;

namespace SAPPub.Core.Services.KS4;

public class SecondarySchoolService(
    IEstablishmentService establishmentService,
    IEstablishmentDestinationsService establishmentDestinationsService,
    ILADestinationsService lADestinationsService,
    IEnglandDestinationsService englandDestinationsService) : ISecondarySchoolService
{
    private readonly IEstablishmentService _establishmentService = establishmentService;
    private readonly IEstablishmentDestinationsService _establishmentDestinationsService = establishmentDestinationsService;
    private readonly ILADestinationsService _lADestinationsService = lADestinationsService;
    private readonly IEnglandDestinationsService _englandDestinationsService = englandDestinationsService;

    public DestinationsDetails GetDestinationsDetails(string urn)
    {
        var establishment = _establishmentService.GetEstablishment(urn);
        var establishmentDestinations = _establishmentDestinationsService.GetEstablishmentDestinations(urn);
        var lADestinations = _lADestinationsService.GetLADestinations(urn);
        var englandDestinations = _englandDestinationsService.GetEnglandDestinations();
        
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
        };

        return model;
    }
}
