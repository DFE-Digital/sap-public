using Microsoft.Extensions.Logging;
using SAPPub.Core.Common.CleanArchitecture.UseCases;
using SAPPub.Core.Entities.Measures.Common;
using SAPPub.Core.Interfaces.Repositories.KS4.Performance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.UseCases.Measures.KS4.Progress8
{
    public sealed class GetProgress8UseCase :IUseCase<GetProgress8UseCase, GetProgress8UseCaseResponse>
    {
        private readonly IEstablishmentPerformanceRepository _establishmentPerformanceRepository;
        private readonly ILAPerformanceRepository _laPerformanceRepository;
        private readonly IEnglandPerformanceRepository _englandPerformanceRepository;

        private readonly ILogger<GetProgress8UseCase> _logger;

        public GetProgress8UseCase(
            IEstablishmentPerformanceRepository establishmentPerformanceRepository,
            ILAPerformanceRepository laPerformanceRepository,
            IEnglandPerformanceRepository englandPerformanceRepository,
            ILogger<GetProgress8UseCase> logger)
        {
            _establishmentPerformanceRepository = establishmentPerformanceRepository;
            _laPerformanceRepository = laPerformanceRepository;
            _englandPerformanceRepository = englandPerformanceRepository;
            _logger = logger;
        }

        public async Task<GetProgress8UseCaseResponse> HandleRequest(MeasureUseCaseRequest request)
        {
            var establishmentPerformance = _establishmentPerformanceRepository.GetEstablishmentPerformance(request.URN);
            var localAuthorityPerformance = _laPerformanceRepository.GetLAPerformance(request.LocalAuthorityCode);


            var progress8 = new Entities.Measures.KS4.Progress8
            {
                Id = request.URN,
                LocalAuthorityCode = request.LocalAuthorityCode,
                Progress8_Score_Est_Current = null,
                Progress8_Score_LA_Current = null,
                Progress8_Score_Est_Previous = establishmentPerformance.Prog8_Tot_Est_Previous_Num,
                Progress8_Score_Est_Previous2 = establishmentPerformance.Prog8_Tot_Est_Previous2_Num,
                Progress8_Score_LA_Previous = localAuthorityPerformance.Prog8_Avg_LA_Previous_Num,
                Progress8_Score_LA_Previous2 = localAuthorityPerformance.Prog8_Avg_LA_Previous2_Num,
            };



            var response = new GetProgress8UseCaseResponse(BasicResponseStatus )
            {
                Progress8 = progress8
            };
            return await Task.FromResult(response);
        }


    }
}
