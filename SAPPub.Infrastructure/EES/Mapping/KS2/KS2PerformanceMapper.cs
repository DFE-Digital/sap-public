using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Entities.Performance;
using SAPPub.Infrastructure.EES.Entities.DTOs.KS2;
using SAPPub.Infrastructure.EES.NonEES.DTOs;
using SAPPub.Infrastructure.EES.NonEES.Helpers;
using SAPPub.Infrastructure.Mapping.ValueCodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAPPub.Infrastructure.EES.Mapping.KS2
{
    public class KS2PerformanceMapper
    {
        public static KS2EstablishmentPerformance Map(
            CodedLightweightParser _lightweightParser,
            KS2EstablishmentInformationDTO establishmentInformation,
            KS2EstablishmentPerformanceDTO establishmentPerformance,
            IEnumerable<CSCPProgKS2> mappedProgressBandings)
        {
            

            return new KS2EstablishmentPerformance
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

                MATPROG_Est_Previous2_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.MATPROG_Est_Previous2_Num_Coded),
                READPROG_Est_Previous2_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.READPROG_Est_Previous2_Num_Coded),
                WRITPROG_Est_Previous2_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.WRITPROG_Est_Previous2_Num_Coded),

                MATPROG_DESCR_Est_Previous2_Num_Coded = _lightweightParser.ParseString(
                        mappedProgressBandings.Where(x => x.LAESTAB == establishmentInformation.LAESTAB).FirstOrDefault()?.MATPROG_DESCR),
                READPROG_DESCR_Est_Previous2_Num_Coded = _lightweightParser.ParseString(
                    mappedProgressBandings.Where(x => x.LAESTAB == establishmentInformation.LAESTAB).FirstOrDefault()?.READPROG_DESCR),
                WRITPROG_DESCR_Est_Previous2_Num_Coded = _lightweightParser.ParseString(
                    mappedProgressBandings.Where(x => x.LAESTAB == establishmentInformation.LAESTAB).FirstOrDefault()?.WRITPROG_DESCR),

                MAT_AVERAGE_Est_Current_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.MAT_AVERAGE_Est_Current_Num_Coded),
                MAT_AVERAGE_Est_Previous2_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.MAT_AVERAGE_Est_Previous2_Num_Coded),
                MAT_AVERAGE_Est_Previous_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.MAT_AVERAGE_Est_Previous_Num_Coded),
                MAT_AVERAGE_3YR_Est_Current_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.MAT_AVERAGE_3YR_Est_Current_Num_Coded),
                MAT_AVERAGE_B_Est_Current_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.MAT_AVERAGE_B_Est_Current_Num_Coded),
                MAT_AVERAGE_EAL_Est_Current_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.MAT_AVERAGE_EAL_Est_Current_Num_Coded),
                MAT_AVERAGE_FSM6CLA1A_Est_Current_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.MAT_AVERAGE_FSM6CLA1A_Est_Current_Num_Coded),
                MAT_AVERAGE_G_Est_Current_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.MAT_AVERAGE_G_Est_Current_Num_Coded),
                MAT_AVERAGE_MOBN_Est_Current_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.MAT_AVERAGE_MOBN_Est_Current_Num_Coded),
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
                READPROG_LOWER_Est_Previous2_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.READPROG_LOWER_Est_Previous2_Num_Coded),
                READPROG_UPPER_Est_Previous2_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.READPROG_UPPER_Est_Previous2_Num_Coded),
                WRITPROG_LOWER_Est_Previous2_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.WRITPROG_LOWER_Est_Previous2_Num_Coded),
                WRITPROG_UPPER_Est_Previous2_Num_Coded = _lightweightParser.ParseDouble(establishmentPerformance.WRITPROG_UPPER_Est_Previous2_Num_Coded)


            };
        }

        public static KS2LAPerformance Map(
            CodedLightweightParser _lightweightParser,
            KS2LAInformationDTO laInformation,
            KS2LAPerformanceDTO laPerformance)
        {
            return new KS2LAPerformance
            {
                Id = laInformation.Id,
                TELIG_LA_Current_Num_Coded = _lightweightParser.ParseDouble(laInformation.TELIG_LA_Current_Num_Coded),
                TPUPYEAR_LA_Current_Num_Coded = _lightweightParser.ParseDouble(laInformation.TPUPYEAR_LA_Current_Num_Coded),
                TFSM6CLA1A_LA_Current_Num_Coded = _lightweightParser.ParseDouble(laInformation.TFSM6CLA1A_LA_Current_Num_Coded),
                TNOTFSM6CLA1A_LA_Current_Num_Coded = _lightweightParser.ParseDouble(laInformation.TNOTFSM6CLA1A_LA_Current_Num_Coded),

                MAT_AVERAGE_LA_Current_Num_Coded = _lightweightParser.ParseDouble(laPerformance.MAT_AVERAGE_LA_Current_Num_Coded),
                MAT_AVERAGE_LA_Previous2_Num_Coded = _lightweightParser.ParseDouble(laPerformance.MAT_AVERAGE_LA_Previous2_Num_Coded),
                MAT_AVERAGE_LA_Previous_Num_Coded = _lightweightParser.ParseDouble(laPerformance.MAT_AVERAGE_LA_Previous_Num_Coded),
                MAT_AVERAGE_3YR_LA_Current_Num_Coded = _lightweightParser.ParseDouble(laPerformance.MAT_AVERAGE_3YR_LA_Current_Num_Coded),
                MAT_AVERAGE_FSM6CLA1A_LA_Current_Num_Coded = _lightweightParser.ParseDouble(laPerformance.MAT_AVERAGE_FSM6CLA1A_LA_Current_Num_Coded),
                PTGPS_EXP_LA_Current_Pct_Coded = _lightweightParser.ParseDouble(laPerformance.PTGPS_EXP_LA_Current_Pct_Coded),
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
                MATPROG_LA_Previous2_Num_Coded = _lightweightParser.ParseDouble(laPerformance.MATPROG_LA_Previous2_Num_Coded),
                READPROG_LA_Previous2_Num_Coded = _lightweightParser.ParseDouble(laPerformance.READPROG_LA_Previous2_Num_Coded),
                WRITPROG_LA_Previous2_Num_Coded = _lightweightParser.ParseDouble(laPerformance.WRITPROG_LA_Previous2_Num_Coded),
                MAT_AVERAGE_NOTFSM6CLA1A_LA_Current_Num_Coded = _lightweightParser.ParseDouble(laPerformance.MAT_AVERAGE_NOTFSM6CLA1A_LA_Current_Num_Coded),
                PTRWM_EXP_NOTFSM6CLA1A_LA_Current_Pct_Coded = _lightweightParser.ParseDouble(laPerformance.PTRWM_EXP_NOTFSM6CLA1A_LA_Current_Pct_Coded),
                PTRWM_HIGH_NOTFSM6CLA1A_LA_Current_Pct_Coded = _lightweightParser.ParseDouble(laPerformance.PTRWM_HIGH_NOTFSM6CLA1A_LA_Current_Pct_Coded),
                READ_AVERAGE_NOTFSM6CLA1A_LA_Current_Num_Coded = _lightweightParser.ParseDouble(laPerformance.READ_AVERAGE_NOTFSM6CLA1A_LA_Current_Num_Coded)
            };
        }

        public static KS2EnglandPerformance Map(
            CodedLightweightParser _lightweightParser,
            KS2EnglandInformationDTO engInformation,
            KS2EnglandPerformanceDTO engPerformance)
        {
            return new KS2EnglandPerformance
            {
                Id = engInformation.Id,
                PSENELE_Eng_Current_Pct_Coded = _lightweightParser.ParseDouble(engInformation.PSENELE_Eng_Current_Pct_Coded),
                PSENELK_Eng_Current_Pct_Coded = _lightweightParser.ParseDouble(engInformation.PSENELK_Eng_Current_Pct_Coded),
                TELIG_Eng_Current_Num_Coded = _lightweightParser.ParseDouble(engInformation.TELIG_Eng_Current_Num_Coded),
                TPUPYEAR_Eng_Current_Num_Coded = _lightweightParser.ParseDouble(engInformation.TPUPYEAR_Eng_Current_Num_Coded),
                TFSM6CLA1A_Eng_Current_Num_Coded = _lightweightParser.ParseDouble(engInformation.TFSM6CLA1A_Eng_Current_Num_Coded),
                TNOTFSM6CLA1A_Eng_Current_Num_Coded = _lightweightParser.ParseDouble(engInformation.TNOTFSM6CLA1A_Eng_Current_Num_Coded),
                TOTPUPS_Eng_Current_Num_Coded = _lightweightParser.ParseDouble(engInformation.TOTPUPS_Eng_Current_Num_Coded),


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
                MAT_AVERAGE_FSM6CLA1A_ENG_Current_Num_Coded = _lightweightParser.ParseDouble(engPerformance.MAT_AVERAGE_FSM6CLA1A_ENG_Current_Num_Coded),
                MAT_AVERAGE_NOTFSM6CLA1A_Eng_Current_Num_Coded = _lightweightParser.ParseDouble(engPerformance.MAT_AVERAGE_NOTFSM6CLA1A_Eng_Current_Num_Coded),
                PTRWM_EXP_NOTFSM6CLA1A_Eng_Current_Pct_Coded = _lightweightParser.ParseDouble(engPerformance.PTRWM_EXP_NOTFSM6CLA1A_Eng_Current_Pct_Coded),
                PTRWM_HIGH_NOTFSM6CLA1A_Eng_Current_Pct_Coded = _lightweightParser.ParseDouble(engPerformance.PTRWM_HIGH_NOTFSM6CLA1A_Eng_Current_Pct_Coded),
                READ_AVERAGE_FSM6CLA1A_ENG_Current_Num_Coded = _lightweightParser.ParseDouble(engPerformance.READ_AVERAGE_FSM6CLA1A_ENG_Current_Num_Coded),
                READ_AVERAGE_NOTFSM6CLA1A_Eng_Current_Num_Coded = _lightweightParser.ParseDouble(engPerformance.READ_AVERAGE_NOTFSM6CLA1A_Eng_Current_Num_Coded),

                // THIS IS ALL TEMPORARY, NEED TO REVIEW IF EVER NEAR LIVE!!
                ProgBand_Read_Band1_Eng_Previous2_Pct_Coded = _lightweightParser.ParseDouble("10"),
                ProgBand_Read_Band2_Eng_Previous2_Pct_Coded = _lightweightParser.ParseDouble("9"),
                ProgBand_Read_Band3_Eng_Previous2_Pct_Coded = _lightweightParser.ParseDouble("61"),
                ProgBand_Read_Band4_Eng_Previous2_Pct_Coded = _lightweightParser.ParseDouble("6"),
                ProgBand_Read_Band5_Eng_Previous2_Pct_Coded = _lightweightParser.ParseDouble("8"),
                ProgBand_Read_Band1_Eng_Previous2_Desc = _lightweightParser.ParseString("the score is greater than or equal to 3.1, and the entire confidence interval is above 0"),
                ProgBand_Read_Band2_Eng_Previous2_Desc = _lightweightParser.ParseString("the score is greater than 0 but lower than 3.1, and the entire confidence interval is above 0"),
                ProgBand_Read_Band3_Eng_Previous2_Desc = _lightweightParser.ParseString("the confidence interval for the progress score includes 0"),
                ProgBand_Read_Band4_Eng_Previous2_Desc = _lightweightParser.ParseString("the score is lower than 0 but greater than or equal to -2.7, and the entire confidence interval is below 0"),
                ProgBand_Read_Band5_Eng_Previous2_Desc = _lightweightParser.ParseString("the score is lower than -2.7 and the entire confidence interval is below 0"),
                ProgBand_Writ_Band1_Eng_Previous2_Pct_Coded = _lightweightParser.ParseDouble("9"),
                ProgBand_Writ_Band2_Eng_Previous2_Pct_Coded = _lightweightParser.ParseDouble("9"),
                ProgBand_Writ_Band3_Eng_Previous2_Pct_Coded = _lightweightParser.ParseDouble("67"),
                ProgBand_Writ_Band4_Eng_Previous2_Pct_Coded = _lightweightParser.ParseDouble("6"),
                ProgBand_Writ_Band5_Eng_Previous2_Pct_Coded = _lightweightParser.ParseDouble("9"),
                ProgBand_Writ_Band1_Eng_Previous2_Desc = _lightweightParser.ParseString("the score is greater than or equal to 2.9, and the entire confidence interval is above 0"),
                ProgBand_Writ_Band2_Eng_Previous2_Desc = _lightweightParser.ParseString("the score is greater than 0 but lower than 2.9, and the entire confidence interval is above 0"),
                ProgBand_Writ_Band3_Eng_Previous2_Desc = _lightweightParser.ParseString("the confidence interval for the progress score includes 0"),
                ProgBand_Writ_Band4_Eng_Previous2_Desc = _lightweightParser.ParseString("the score is lower than 0 but greater than or equal to -2.7, and the entire confidence interval is below 0"),
                ProgBand_Writ_Band5_Eng_Previous2_Desc = _lightweightParser.ParseString("the score is lower than -2.7 and the entire confidence interval is below 0"),
                ProgBand_Math_Band1_Eng_Previous2_Pct_Coded = _lightweightParser.ParseDouble("10"),
                ProgBand_Math_Band2_Eng_Previous2_Pct_Coded = _lightweightParser.ParseDouble("11"),
                ProgBand_Math_Band3_Eng_Previous2_Pct_Coded = _lightweightParser.ParseDouble("59"),
                ProgBand_Math_Band4_Eng_Previous2_Pct_Coded = _lightweightParser.ParseDouble("11"),
                ProgBand_Math_Band5_Eng_Previous2_Pct_Coded = _lightweightParser.ParseDouble("9"),
                ProgBand_Math_Band1_Eng_Previous2_Desc = _lightweightParser.ParseString("the score is greater than or equal to 3.2, and the entire confidence interval is above 0"),
                ProgBand_Math_Band2_Eng_Previous2_Desc = _lightweightParser.ParseString("the score is greater than 0 but lower than 3.2, and the entire confidence interval is above 0"),
                ProgBand_Math_Band3_Eng_Previous2_Desc = _lightweightParser.ParseString("the confidence interval for the progress score includes 0"),
                ProgBand_Math_Band4_Eng_Previous2_Desc = _lightweightParser.ParseString("the score is lower than 0 but greater than or equal to -3.3, and the entire confidence interval is below 0"),
                ProgBand_Math_Band5_Eng_Previous2_Desc = _lightweightParser.ParseString("the score is lower than -3.3 and the entire confidence interval is below 0")


            };
        }
    }
}
