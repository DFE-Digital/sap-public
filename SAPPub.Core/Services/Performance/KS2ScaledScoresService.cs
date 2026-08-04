using SAPPub.Core.Entities;
using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Interfaces.Repositories.Performance;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Services.Performance
{
    public class KS2ScaledScoresService(
        IEstablishmentService establishmentService, 
        IKS2PerformanceRepository ks2PerformanceRepository) : IKS2ScaledScoreService
    {

        public async Task<KS2ScaledScoreModel> GetScaledScoreModel(string urn, CancellationToken ct = default)
        {
            var establishment = await establishmentService.GetEstablishmentAsync(urn, ct);
            var establishmentPerformanceTask = ks2PerformanceRepository.GetEstablishmentPerformanceAsync(urn, ct);
            var localAuthorityPerformanceTask = ks2PerformanceRepository.GetLaPerformanceAsync(establishment.LAId, ct);
            var englandPerformanceTask = ks2PerformanceRepository.GetEnglandPerformanceAsync(ct);

            await Task.WhenAll(establishmentPerformanceTask, localAuthorityPerformanceTask, englandPerformanceTask);

            var establishmentPerformance = await establishmentPerformanceTask;
            var englandPerformance = await englandPerformanceTask;
            var laPerformance = await localAuthorityPerformanceTask;

            return new KS2ScaledScoreModel
            {
                Urn = establishment.URN,
                SchoolName = establishment.EstablishmentName,
                LAName = establishment.LAName,
                IsKS2 = establishment.IsKS2,
                IsKS4 = establishment.IsKS4,
                IsKS5 = establishment.IsKS5,
                Read_Average_Establishment = GetEstablishmentReadAverage(establishmentPerformance),
                Read_Average_England = GetEngReadAverage(englandPerformance),
                Read_Average_LA = GetLAReadAverage(laPerformance)
            };
        }

        private static RelativeYearValues<CodedDouble> GetEstablishmentReadAverage(KS2EstablishmentPerformance performance)
        {
            return new RelativeYearValues<CodedDouble>
            {
                CurrentYear = performance.READ_AVERAGE_Est_Current_Num_Coded,
                PreviousYear = performance.READ_AVERAGE_Est_Previous_Num_Coded,
                TwoYearsAgo = performance.READ_AVERAGE_Est_Previous2_Num_Coded,
            };
        }

        private static RelativeYearValues<CodedDouble> GetLAReadAverage(KS2LAPerformance performance)
        {
            return new RelativeYearValues<CodedDouble>
            {
                CurrentYear = performance.READ_AVERAGE_LA_Current_Num_Coded,
                PreviousYear = performance.READ_AVERAGE_LA_Previous_Num_Coded,
                TwoYearsAgo = performance.READ_AVERAGE_LA_Previous2_Num_Coded,
            };
        }

        private static RelativeYearValues<CodedDouble> GetEngReadAverage(KS2EnglandPerformance performance)
        {
            return new RelativeYearValues<CodedDouble>
            {
                CurrentYear = performance.READ_AVERAGE_Eng_Current_Num_Coded,
                PreviousYear = performance.READ_AVERAGE_Eng_Previous_Num_Coded,
                TwoYearsAgo = performance.READ_AVERAGE_Eng_Previous2_Num_Coded,
            };
        }

    }
}
