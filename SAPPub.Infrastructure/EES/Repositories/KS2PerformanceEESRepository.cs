using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Interfaces.Repositories.Performance;
using SAPPub.Core.ValueObjects;
using SAPPub.Infrastructure.EES.Entities.DTOs;
using SAPPub.Infrastructure.EES.Entities.DTOs.KS2;
using SAPPub.Infrastructure.EES.Service;
using SAPPub.Infrastructure.Mapping.ValueCodes;
using System;
using System.Collections.Generic;
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

            if (engInformation == null || engPerformance == null)
            {
                _logger.LogError("Failed to retrieve England performance or information for URN: {URN}", "");
                throw new Exception($"Failed to retrieve England performance or information for URN: {""}");
            }

            var returnModel = new KS2EnglandPerformance
            {
                Id = engInformation.Id,
                PSENELE_Eng_Current_Pct_Coded = _lightweightParser.ParseDouble(engInformation.PSENELE_Eng_Current_Pct_Coded),
                PSENELK_Eng_Current_Pct_Coded = _lightweightParser.ParseDouble(engInformation.PSENELK_Eng_Current_Pct_Coded),
                TELIG_Eng_Current_Num_Coded = _lightweightParser.ParseDouble(engInformation.TELIG_Eng_Current_Num_Coded),
                TPUPYEAR_Eng_Current_Num_Coded = _lightweightParser.ParseDouble(engInformation.TPUPYEAR_Eng_Current_Num_Coded),
                TFSM6CLA1A_Eng_Current_Num_Coded = _lightweightParser.ParseDouble(engInformation.TFSM6CLA1A_Eng_Current_Num_Coded),
                MAT_AVERAGE_Eng_Current_Num_Coded = _lightweightParser.ParseDouble(engPerformance.MAT_AVERAGE_Eng_Current_Num_Coded),
                MAT_AVERAGE_Eng_Previous2_Num_Coded = _lightweightParser.ParseDouble(engPerformance.MAT_AVERAGE_Eng_Previous2_Num_Coded),
                MAT_AVERAGE_Eng_Previous_Num_Coded = _lightweightParser.ParseDouble(engPerformance.MAT_AVERAGE_Eng_Previous_Num_Coded),
                MAT_AVERAGE_3YR_Eng_Current_Num_Coded = _lightweightParser.ParseDouble(engPerformance.MAT_AVERAGE_3YR_Eng_Current_Num_Coded),
                PTGPS_EXP_Eng_Current_Pct_Coded = _lightweightParser.ParseDouble(engPerformance.PTGPS_EXP_Eng_Current_Pct_Coded),
                PTGPS_HIGH_Eng_Current_Pct_Coded = _lightweightParser.ParseDouble(engPerformance.PTGPS_HIGH_Eng_Current_Pct_Coded),
                PTRWM_EXP_Eng_Current_Pct_Coded = _lightweightParser.ParseDouble(engPerformance.PTRWM_EXP_Eng_Current_Pct_Coded),
                PTRWM_EXP_Eng_Previous2_Pct_Coded = _lightweightParser.ParseDouble(engPerformance.PTRWM_EXP_Eng_Previous2_Pct_Coded),
                PTRWM_EXP_Eng_Previous_Pct_Coded = _lightweightParser.ParseDouble(engPerformance.PTRWM_EXP_Eng_Previous_Pct_Coded),
                PTRWM_EXP_3YR_Eng_Current_Pct_Coded = _lightweightParser.ParseDouble(engPerformance.PTRWM_EXP_3YR_Eng_Current_Pct_Coded),
                PTRWM_EXP_FSM6CLA1A_Eng_Current_Pct_Coded = _lightweightParser.ParseDouble(engPerformance.PTRWM_EXP_FSM6CLA1A_Eng_Current_Pct_Coded),
                PTRWM_HIGH_Eng_Current_Pct_Coded = _lightweightParser.ParseDouble(engPerformance.PTRWM_HIGH_Eng_Current_Pct_Coded),
                PTRWM_HIGH_Eng_Previous2_Pct_Coded = _lightweightParser.ParseDouble(engPerformance.PTRWM_HIGH_Eng_Previous2_Pct_Coded),
                PTRWM_HIGH_Eng_Previous_Pct_Coded = _lightweightParser.ParseDouble(engPerformance.PTRWM_HIGH_Eng_Previous_Pct_Coded),
                PTRWM_HIGH_3YR_Eng_Current_Pct_Coded = _lightweightParser.ParseDouble(engPerformance.PTRWM_HIGH_3YR_Eng_Current_Pct_Coded),
                PTRWM_HIGH_FSM6CLA1A_Eng_Current_Pct_Coded = _lightweightParser.ParseDouble(engPerformance.PTRWM_HIGH_FSM6CLA1A_Eng_Current_Pct_Coded),
                READ_AVERAGE_Eng_Current_Num_Coded = _lightweightParser.ParseDouble(engPerformance.READ_AVERAGE_Eng_Current_Num_Coded),
                READ_AVERAGE_Eng_Previous2_Num_Coded = _lightweightParser.ParseDouble(engPerformance.READ_AVERAGE_Eng_Previous2_Num_Coded),
                READ_AVERAGE_Eng_Previous_Num_Coded = _lightweightParser.ParseDouble(engPerformance.READ_AVERAGE_Eng_Previous_Num_Coded),
                READ_AVERAGE_3YR_Eng_Current_Num_Coded = _lightweightParser.ParseDouble(engPerformance.READ_AVERAGE_3YR_Eng_Current_Num_Coded),


            };
            return returnModel;
        }

        public async Task<KS2EstablishmentPerformance> GetEstablishmentPerformanceAsync(string urn, CancellationToken ct = default)
        {
            var establishmentPerformance = await _service.GetData<KS2EstablishmentPerformanceDTO>(urn, ct);
            var establishmentInformation = await _service.GetData<KS2EstablishmentInformationDTO>(urn, ct);

            if (establishmentInformation == null || establishmentPerformance == null)
            {
                _logger.LogError("Failed to retrieve establishment performance or information for URN: {URN}", urn);
                throw new Exception($"Failed to retrieve establishment performance or information for URN: {urn}");
            }

            var returnModel = new KS2EstablishmentPerformance
            {
                Id = establishmentInformation.Id,
                BELIG_Est_Current_Num_Coded = _lightweightParser.ParseDouble(establishmentInformation.BELIG_Est_Current_Num_Coded),
                GELIG_Est_Current_Num_Coded = _lightweightParser.ParseDouble(establishmentInformation.GELIG_Est_Current_Num_Coded),
                PSENELE_Est_Current_Pct_Coded = _lightweightParser.ParseDouble(establishmentInformation.PSENELE_Est_Current_Pct_Coded),
                PSENELK_Est_Current_Pct_Coded = _lightweightParser.ParseDouble(establishmentInformation.PSENELK_Est_Current_Pct_Coded),
                TEALGRP2_Est_Current_Num_Coded = _lightweightParser.ParseDouble(establishmentInformation.TEALGRP2_Est_Current_Num_Coded),
                TELIG_Est_Current_Num_Coded = _lightweightParser.ParseDouble(establishmentInformation.TELIG_Est_Current_Num_Coded),
                TPUPYEAR_Est_Current_Num_Coded = _lightweightParser.ParseDouble(establishmentInformation.TPUPYEAR_Est_Current_Num_Coded),
                TFSM6CLA1A_Est_Current_Num_Coded = _lightweightParser.ParseDouble(establishmentInformation.TFSM6CLA1A_Est_Current_Num_Coded),
                TMOBN_Est_Current_Num_Coded = _lightweightParser.ParseDouble(establishmentInformation.TMOBN_Est_Current_Num_Coded),

                MATPROG_DESCR_Est_Previous2_Num_Coded = _lightweightParser.ParseString(establishmentPerformance.MATPROG_DESCR_Est_Previous2_Num_Coded),
                READPROG_DESCR_Est_Previous2_Num_Coded = _lightweightParser.ParseString(establishmentPerformance.READPROG_DESCR_Est_Previous2_Num_Coded),
                WRITPROG_DESCR_Est_Previous2_Num_Coded = _lightweightParser.ParseString(establishmentPerformance.WRITPROG_DESCR_Est_Previous2_Num_Coded),
                MAT_AVERAGE_Est_Current_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.MAT_AVERAGE_Est_Current_Num_Coded),
                MAT_AVERAGE_Est_Previous2_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.MAT_AVERAGE_Est_Previous2_Num_Coded),
                MAT_AVERAGE_Est_Previous_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.MAT_AVERAGE_Est_Previous_Num_Coded),
                MAT_AVERAGE_3YR_Est_Current_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.MAT_AVERAGE_3YR_Est_Current_Num_Coded),
                MAT_AVERAGE_B_Est_Current_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.MAT_AVERAGE_B_Est_Current_Num_Coded),
                MAT_AVERAGE_EAL_Est_Current_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.MAT_AVERAGE_EAL_Est_Current_Num_Coded),
                MAT_AVERAGE_FSM6CLA1A_Est_Current_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.MAT_AVERAGE_FSM6CLA1A_Est_Current_Num_Coded),
                MAT_AVERAGE_G_Est_Current_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.MAT_AVERAGE_G_Est_Current_Num_Coded),
                MAT_AVERAGE_MOBN_Est_Current_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.MAT_AVERAGE_MOBN_Est_Current_Num_Coded),
                MATPROG_Est_Previous2_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.MATPROG_Est_Previous2_Num_Coded),
                MATPROG_LOWER_Est_Previous2_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.MATPROG_LOWER_Est_Previous2_Num_Coded),
                MATPROG_UPPER_Est_Previous2_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.MATPROG_UPPER_Est_Previous2_Num_Coded),
                PTGPS_EXP_Est_Current_Pct_Coded = _lightweightParser.ParseDouble(establishmentPerformance.PTGPS_EXP_Est_Current_Pct_Coded),
                PTGPS_HIGH_Est_Current_Pct_Coded = _lightweightParser.ParseDouble(establishmentPerformance.PTGPS_HIGH_Est_Current_Pct_Coded),
                PTRWM_EXP_Est_Current_Pct_Coded = _lightweightParser.ParseDouble(establishmentPerformance.PTRWM_EXP_Est_Current_Pct_Coded),
                PTRWM_EXP_Est_Previous2_Pct_Coded = _lightweightParser.ParseDouble(establishmentPerformance.PTRWM_EXP_Est_Previous2_Pct_Coded),
                PTRWM_EXP_Est_Previous_Pct_Coded = _lightweightParser.ParseDouble(establishmentPerformance.PTRWM_EXP_Est_Previous_Pct_Coded),
                PTRWM_EXP_3YR_Est_Current_Pct_Coded = _lightweightParser.ParseDouble(establishmentPerformance.PTRWM_EXP_3YR_Est_Current_Pct_Coded),
                PTRWM_EXP_B_Est_Current_Pct_Coded = _lightweightParser.ParseDouble(establishmentPerformance.PTRWM_EXP_B_Est_Current_Pct_Coded),
                PTRWM_EXP_EAL_Est_Current_Pct_Coded = _lightweightParser.ParseDouble(establishmentPerformance.PTRWM_EXP_EAL_Est_Current_Pct_Coded),
                PTRWM_EXP_FSM6CLA1A_Est_Current_Pct_Coded = _lightweightParser.ParseDouble(establishmentPerformance.PTRWM_EXP_FSM6CLA1A_Est_Current_Pct_Coded),
                PTRWM_EXP_G_Est_Current_Pct_Coded = _lightweightParser.ParseDouble(establishmentPerformance.PTRWM_EXP_G_Est_Current_Pct_Coded),
                PTRWM_EXP_MOBN_Est_Current_Pct_Coded = _lightweightParser.ParseDouble(establishmentPerformance.PTRWM_EXP_MOBN_Est_Current_Pct_Coded),
                PTRWM_HIGH_Est_Current_Pct_Coded = _lightweightParser.ParseDouble(establishmentPerformance.PTRWM_HIGH_Est_Current_Pct_Coded),
                PTRWM_HIGH_Est_Previous2_Pct_Coded = _lightweightParser.ParseDouble(establishmentPerformance.PTRWM_HIGH_Est_Previous2_Pct_Coded),
                PTRWM_HIGH_Est_Previous_Pct_Coded = _lightweightParser.ParseDouble(establishmentPerformance.PTRWM_HIGH_Est_Previous_Pct_Coded),
                PTRWM_HIGH_3YR_Est_Current_Pct_Coded = _lightweightParser.ParseDouble(establishmentPerformance.PTRWM_HIGH_3YR_Est_Current_Pct_Coded),
                PTRWM_HIGH_B_Est_Current_Pct_Coded = _lightweightParser.ParseDouble(establishmentPerformance.PTRWM_HIGH_B_Est_Current_Pct_Coded),
                PTRWM_HIGH_EAL_Est_Current_Pct_Coded = _lightweightParser.ParseDouble(establishmentPerformance.PTRWM_HIGH_EAL_Est_Current_Pct_Coded),
                PTRWM_HIGH_FSM6CLA1A_Est_Current_Pct_Coded = _lightweightParser.ParseDouble(establishmentPerformance.PTRWM_HIGH_FSM6CLA1A_Est_Current_Pct_Coded),
                PTRWM_HIGH_G_Est_Current_Pct_Coded = _lightweightParser.ParseDouble(establishmentPerformance.PTRWM_HIGH_G_Est_Current_Pct_Coded),
                PTRWM_HIGH_MOBN_Est_Current_Pct_Coded = _lightweightParser.ParseDouble(establishmentPerformance.PTRWM_HIGH_MOBN_Est_Current_Pct_Coded),
                READ_AVERAGE_Est_Current_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.READ_AVERAGE_Est_Current_Num_Coded),
                READ_AVERAGE_Est_Previous2_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.READ_AVERAGE_Est_Previous2_Num_Coded),
                READ_AVERAGE_Est_Previous_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.READ_AVERAGE_Est_Previous_Num_Coded),
                READ_AVERAGE_3YR_Est_Current_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.READ_AVERAGE_3YR_Est_Current_Num_Coded),
                READ_AVERAGE_B_Est_Current_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.READ_AVERAGE_B_Est_Current_Num_Coded),
                READ_AVERAGE_EAL_Est_Current_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.READ_AVERAGE_EAL_Est_Current_Num_Coded),
                READ_AVERAGE_FSM6CLA1A_Est_Current_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.READ_AVERAGE_FSM6CLA1A_Est_Current_Num_Coded),
                READ_AVERAGE_G_Est_Current_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.READ_AVERAGE_G_Est_Current_Num_Coded),
                READ_AVERAGE_MOBN_Est_Current_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.READ_AVERAGE_MOBN_Est_Current_Num_Coded),
                READPROG_Est_Previous2_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.READPROG_Est_Previous2_Num_Coded),
                READPROG_LOWER_Est_Previous2_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.READPROG_LOWER_Est_Previous2_Num_Coded),
                READPROG_UPPER_Est_Previous2_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.READPROG_UPPER_Est_Previous2_Num_Coded),
                WRITPROG_Est_Previous2_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.WRITPROG_Est_Previous2_Num_Coded),
                WRITPROG_LOWER_Est_Previous2_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.WRITPROG_LOWER_Est_Previous2_Num_Coded),
                WRITPROG_UPPER_Est_Previous2_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.WRITPROG_UPPER_Est_Previous2_Num_Coded)


            };
            return returnModel;
            //}

            //return new();
        }

        public async Task<KS2LAPerformance> GetLaPerformanceAsync(string laCode, CancellationToken ct = default)
        {
            var laPerformance = await _service.GetData<KS2LAPerformanceDTO>(laCode, ct);
            var laInformation = await _service.GetData<KS2LAInformationDTO>(laCode, ct);

            if (laInformation == null || laPerformance == null)
            {
                _logger.LogError("Failed to retrieve LA performance or information for LA Code: {LACode}", laCode);
                throw new Exception($"Failed to retrieve LA performance or information for LA Code: {laCode}");
            }

            var returnModel = new KS2LAPerformance
            {
                Id = laInformation.Id,
                TELIG_LA_Current_Num_Coded = _lightweightParser.ParseDouble(laInformation.TELIG_LA_Current_Num_Coded),
                TPUPYEAR_LA_Current_Num_Coded = _lightweightParser.ParseDouble(laInformation.TPUPYEAR_LA_Current_Num_Coded),
                TFSM6CLA1A_LA_Current_Num_Coded = _lightweightParser.ParseDouble(laInformation.TFSM6CLA1A_LA_Current_Num_Coded),

                MAT_AVERAGE_LA_Current_Num_Coded = _lightweightParser.ParseDouble(laPerformance.MAT_AVERAGE_LA_Current_Num_Coded),
                MAT_AVERAGE_LA_Previous2_Num_Coded = _lightweightParser.ParseDouble(laPerformance.MAT_AVERAGE_LA_Previous2_Num_Coded),
                MAT_AVERAGE_LA_Previous_Num_Coded = _lightweightParser.ParseDouble(laPerformance.MAT_AVERAGE_LA_Previous_Num_Coded),
                MAT_AVERAGE_3YR_LA_Current_Num_Coded = _lightweightParser.ParseDouble(laPerformance.MAT_AVERAGE_3YR_LA_Current_Num_Coded),
                MAT_AVERAGE_FSM6CLA1A_LA_Current_Num_Coded = _lightweightParser.ParseDouble(laPerformance.MAT_AVERAGE_FSM6CLA1A_LA_Current_Num_Coded),
                MATPROG_LA_Previous2_Num_Coded = _lightweightParser.ParseDouble(laPerformance.MATPROG_LA_Previous2_Num_Coded),
                PTGPS_EXP_LA_Current_Pct_Coded = _lightweightParser.ParseDouble(laPerformance.PTGPS_EXP_LA_Current_Pct_Coded)   ,
                PTGPS_HIGH_LA_Current_Pct_Coded = _lightweightParser.ParseDouble(laPerformance.PTGPS_HIGH_LA_Current_Pct_Coded),
                PTRWM_EXP_LA_Current_Pct_Coded = _lightweightParser.ParseDouble(laPerformance.PTRWM_EXP_LA_Current_Pct_Coded),
                PTRWM_EXP_LA_Previous2_Pct_Coded = _lightweightParser.ParseDouble(laPerformance.PTRWM_EXP_LA_Previous2_Pct_Coded),
                PTRWM_EXP_LA_Previous_Pct_Coded = _lightweightParser.ParseDouble(laPerformance.PTRWM_EXP_LA_Previous_Pct_Coded),
                PTRWM_EXP_3YR_LA_Current_Pct_Coded = _lightweightParser.ParseDouble(laPerformance.PTRWM_EXP_3YR_LA_Current_Pct_Coded),
                PTRWM_EXP_FSM6CLA1A_LA_Current_Pct_Coded = _lightweightParser.ParseDouble(laPerformance.PTRWM_EXP_FSM6CLA1A_LA_Current_Pct_Coded),
                PTRWM_HIGH_LA_Current_Pct_Coded = _lightweightParser.ParseDouble(laPerformance.PTRWM_HIGH_LA_Current_Pct_Coded),
                PTRWM_HIGH_LA_Previous2_Pct_Coded = _lightweightParser.ParseDouble(laPerformance.PTRWM_HIGH_LA_Previous2_Pct_Coded),
                PTRWM_HIGH_LA_Previous_Pct_Coded = _lightweightParser.ParseDouble(laPerformance.PTRWM_HIGH_LA_Previous_Pct_Coded),
                PTRWM_HIGH_3YR_LA_Current_Pct_Coded = _lightweightParser.ParseDouble(laPerformance.PTRWM_HIGH_3YR_LA_Current_Pct_Coded),
                PTRWM_HIGH_FSM6CLA1A_LA_Current_Pct_Coded = _lightweightParser.ParseDouble(laPerformance.PTRWM_HIGH_FSM6CLA1A_LA_Current_Pct_Coded),
                READ_AVERAGE_LA_Current_Num_Coded = _lightweightParser.ParseDouble(laPerformance.READ_AVERAGE_LA_Current_Num_Coded),
                READ_AVERAGE_LA_Previous2_Num_Coded = _lightweightParser.ParseDouble(laPerformance.READ_AVERAGE_LA_Previous2_Num_Coded),
                READ_AVERAGE_LA_Previous_Num_Coded = _lightweightParser.ParseDouble(laPerformance.READ_AVERAGE_LA_Previous_Num_Coded),
                READ_AVERAGE_3YR_LA_Current_Num_Coded = _lightweightParser.ParseDouble(laPerformance.READ_AVERAGE_3YR_LA_Current_Num_Coded),
                READ_AVERAGE_FSM6CLA1A_LA_Current_Num_Coded = _lightweightParser.ParseDouble(laPerformance.READ_AVERAGE_FSM6CLA1A_LA_Current_Num_Coded),
                READPROG_LA_Previous2_Num_Coded = _lightweightParser.ParseDouble(laPerformance.READPROG_LA_Previous2_Num_Coded),
                WRITPROG_LA_Previous2_Num_Coded = _lightweightParser.ParseDouble(laPerformance.WRITPROG_LA_Previous2_Num_Coded)
            };
            return returnModel;
        }
    }
}
