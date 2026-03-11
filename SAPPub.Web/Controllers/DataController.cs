using Microsoft.AspNetCore.Mvc;
using SAPPub.Core.Entities.KS4.Destinations;
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
        private readonly IEstablishmentService _establishmentService;
        private readonly IEstablishmentAbsenceService _absenceService;
        private readonly ILAAbsenceService _laAbsenceService;
        private readonly IEnglandAbsenceService _englandAbsenceService;
        private readonly IEstablishmentPerformanceService _performanceService;
        private readonly IEstablishmentDestinationsService _destinationsService;
        private readonly ILADestinationsService _laDestinationsService;
        private readonly ILAPerformanceService _laPerformanceService;
        private readonly IEnglandDestinationsService _englandDestinationsService;
        private readonly IEnglandPerformanceService _englandPerformanceService;
        private readonly IEstablishmentWorkforceService _workforceService;

        public DataController(
            ILADestinationsService ladestinationsService,
            ILAPerformanceService laperformanceService,
            IEnglandDestinationsService englandDestinationsService,
            IEnglandPerformanceService englandPerformanceService,
            IEstablishmentDestinationsService destinationsService,
            IEstablishmentAbsenceService absenceService,
            IEstablishmentPerformanceService performanceService,
            IEstablishmentService establishmentService,
            IEstablishmentWorkforceService workforceService,
            IEnglandAbsenceService englandAbsenceService,
            ILAAbsenceService laAbsenceService)
        {
            _establishmentService = establishmentService;

            _absenceService = absenceService;
            _laAbsenceService = laAbsenceService;
            _englandAbsenceService = englandAbsenceService;

            _performanceService = performanceService;
            _destinationsService = destinationsService;
            _workforceService = workforceService;

            _laDestinationsService = ladestinationsService;
            _laPerformanceService = laperformanceService;
            _englandDestinationsService = englandDestinationsService;
            _englandPerformanceService = englandPerformanceService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var data = await _establishmentService.GetAllEstablishmentsAsync(ct);
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Data(string id, CancellationToken ct)
        {
            // This could be wrapped into a ViewModel. Not in scope for this delivery though.
            var dataModel = await _establishmentService.GetEstablishmentAsync(id, ct);

            // If establishment not found (service may throw or return empty), keep POC behavior:
            // the service returns an Establishment; if URN empty, view will show empty model.

            dataModel.Absence = await _absenceService.GetEstablishmentAbsenceAsync(id, ct);
            dataModel.LAAbsence = await _laAbsenceService.GetLAAbsenceAsync(dataModel.LAId, ct);
            dataModel.EnglandAbsence = await _englandAbsenceService.GetEnglandAbsenceAsync(ct);

            dataModel.KS4Performance = await _performanceService.GetEstablishmentPerformanceAsync(id, ct);
            dataModel.EstablishmentDestinations = await _destinationsService.GetEstablishmentDestinationsAsync(id, ct) ?? new EstablishmentDestinations();
            dataModel.Workforce = await _workforceService.GetEstablishmentWorkforceAsync(id, ct);

            dataModel.LADestinations = await _laDestinationsService.GetLADestinationsAsync(dataModel.LAId, ct);
            dataModel.LAPerformance = await _laPerformanceService.GetLAPerformanceAsync(dataModel.LAId, ct);

            dataModel.EnglandDestinations = await _englandDestinationsService.GetEnglandDestinationsAsync(ct);
            dataModel.EnglandPerformance = await _englandPerformanceService.GetEnglandPerformanceAsync(ct);

            return View(dataModel);
        }
    }
}
