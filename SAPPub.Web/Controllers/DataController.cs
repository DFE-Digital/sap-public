using Microsoft.AspNetCore.Mvc;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Absence;
using SAPPub.Core.Interfaces.Services.KS4.Destinations;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.Interfaces.Services.KS4.Workforce;

namespace SAPPub.Web.Controllers
{
    /// <summary>
    /// EXAMPLE CLASS, POC only.
    /// </summary>
    public class DataController : Controller
    {
        private IEstablishmentService _service;
        private IEstablishmentAbsenceService _absenceService;
        private IEstablishmentPerformanceService _performanceService;
        private IEstablishmentDestinationsService _destinationsService;
        private ILADestinationsService _LAdestinationsService;
        private ILAPerformanceService _LAperformanceService;
        private IEnglandDestinationsService _englandDestinationsService;
        private IEnglandPerformanceService _englandPerformanceService;
        private IEstablishmentWorkforceService _workforceService;

        public DataController(
            ILADestinationsService ladestinationsService,
            ILAPerformanceService laperformanceService,
            IEnglandDestinationsService englandDestinationsService,
            IEnglandPerformanceService englandPerformanceService,
            IEstablishmentDestinationsService destinationsService,
            IEstablishmentAbsenceService absenceService,
            IEstablishmentPerformanceService performanceService,
            IEstablishmentService establishmentService,
            IEstablishmentWorkforceService workforceService
            )
        {

            _service = establishmentService;

            _absenceService = absenceService;
            _performanceService = performanceService;
            _destinationsService = destinationsService;
            _workforceService = workforceService;

            _LAdestinationsService = ladestinationsService;
            _LAperformanceService = laperformanceService;
            _englandDestinationsService = englandDestinationsService;
            _englandPerformanceService = englandPerformanceService;
        }

        public IActionResult Index()
        {
            var data = _service.GetAllEstablishments();
            return View(data);
        }

        public IActionResult Data(string id)
        {
            //This could be wrapped into a ViewModel. Not in scope for this delivery though.

            var dataModel = _service.GetEstablishment(id);

            dataModel.Absence = _absenceService.GetEstablishmentAbsence(id);
            dataModel.KS4Performance = _performanceService.GetEstablishmentPerformance(id);
            dataModel.EstablishmentDestinations = _destinationsService.GetEstablishmentDestinations(id);
            dataModel.Workforce = _workforceService.GetEstablishmentWorkforce(id);


            dataModel.LADestinations = _LAdestinationsService.GetLADestinations(dataModel.LAId);
            dataModel.LAPerformance = _LAperformanceService.GetLAPerformance(dataModel.LAId);

            dataModel.EnglandDestinations = _englandDestinationsService.GetEnglandDestinations();
            dataModel.EnglandPerformance = _englandPerformanceService.GetEnglandPerformance();

            return View(dataModel);

        }
    }
}
