using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Interfaces.Repositories.Performance;
using SAPPub.Core.ValueObjects;
using SAPPub.Infrastructure.EES.Entities.DTOs;
using SAPPub.Infrastructure.EES.Entities.DTOs.KS2;
using SAPPub.Infrastructure.EES.Mapping.KS2;
using SAPPub.Infrastructure.EES.Service;
using SAPPub.Infrastructure.Mapping.ValueCodes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SAPPub.Infrastructure.EES.Repositories
{
    public class KS2PerformanceEESRepository(
        IEESAPIService service,
        ILogger<KS2PerformanceEESRepository> logger,
        ICodedValueMapper codedValueMapper,
        CodedLightweightParser lightweightParser) : IKS2PerformanceRepository
    {
        private readonly IEESAPIService _service = service;
        private readonly ILogger<KS2PerformanceEESRepository> _logger = logger;
        private readonly ICodedValueMapper _codedValueMapper = codedValueMapper;
        private readonly CodedLightweightParser _lightweightParser = lightweightParser;


        public async Task<KS2EnglandPerformance> GetEnglandPerformanceAsync(CancellationToken ct = default)
        {
            var engPerformance = await _service.GetData<KS2EnglandPerformanceDTO>("", ct);
            var engInformation = await _service.GetData<KS2EnglandInformationDTO>("", ct);

            if (engInformation.Id == null || engPerformance.Id == null)
            {
                _logger.LogError("Failed to retrieve England performance or information for URN: {URN}", "");
                throw new Exception($"Failed to retrieve England performance or information for URN: {""}");
            }

            var mapped = KS2PerformanceMapper.Map(_lightweightParser, engInformation, engPerformance);
            File.WriteAllText($"C:\\Working\\sap-public\\SAPPub.Infrastructure\\EES\\Data\\Request_engperformance.json", JsonConvert.SerializeObject(mapped, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            return mapped;
        }

        public async Task<KS2EstablishmentPerformance> GetEstablishmentPerformanceAsync(string urn, CancellationToken ct = default)
        {
            var establishmentPerformance = await _service.GetData<KS2EstablishmentPerformanceDTO>(urn, ct);
            var establishmentInformation = await _service.GetData<KS2EstablishmentInformationDTO>(urn, ct);

            if (establishmentInformation.Id == null || establishmentPerformance.Id == null)
            {
                _logger.LogError("Failed to retrieve establishment performance or information for URN: {URN}", urn);
                throw new Exception($"Failed to retrieve establishment performance or information for URN: {urn}");
            }

            var mapped = KS2PerformanceMapper.Map(_lightweightParser, establishmentInformation, establishmentPerformance);

            File.WriteAllText($"C:\\Working\\sap-public\\SAPPub.Infrastructure\\EES\\Data\\Request_estperformance.json", JsonConvert.SerializeObject(mapped, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            return mapped;
        }

        public async Task<KS2LAPerformance> GetLaPerformanceAsync(string laCode, CancellationToken ct = default)
        {
            var laPerformance = await _service.GetData<KS2LAPerformanceDTO>(laCode, ct);
            var laInformation = await _service.GetData<KS2LAInformationDTO>(laCode, ct);

            if (laInformation.Id == null || laPerformance.Id == null)
            {
                _logger.LogError("Failed to retrieve LA performance or information for LA Code: {LACode}", laCode);
                throw new Exception($"Failed to retrieve LA performance or information for LA Code: {laCode}");
            }
            var mapped = KS2PerformanceMapper.Map(_lightweightParser, laInformation, laPerformance);
            File.WriteAllText($"C:\\Working\\sap-public\\SAPPub.Infrastructure\\EES\\Data\\Request_laperformance.json", JsonConvert.SerializeObject(mapped, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            return mapped;
        }
    }
}
