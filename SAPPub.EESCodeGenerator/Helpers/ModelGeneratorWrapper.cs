using Newtonsoft.Json;
using SAPPub.Infrastructure.EES.Config;
using SAPPub.Infrastructure.EES.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAPPub.EESCodeGenerator.Helpers
{
    internal interface IModelGeneratorWrapper
    {
        Task GenerateAllModels();
        Task GenerateModelFromMetadata(EESMappingType mappingType);
    }

    internal class ModelGeneratorWrapper(IEESAPIService eesAPI) : IModelGeneratorWrapper
    {
        private readonly IEESAPIService _eesAPI = eesAPI;

        public async Task GenerateAllModels()
        {
            await GenerateModelFromMetadata(EESMappingType.KS2PerformanceEstablishment);
            await GenerateModelFromMetadata(EESMappingType.KS2InformationEstablishment);
            await GenerateModelFromMetadata(EESMappingType.KS2PerformanceLocalAuthority);
            await GenerateModelFromMetadata(EESMappingType.KS2InformationLocalAuthority);
            await GenerateModelFromMetadata(EESMappingType.KS2PerformanceEngland);
            await GenerateModelFromMetadata(EESMappingType.KS2InformationEngland);
        }


        public async Task GenerateModelFromMetadata(EESMappingType mappingType)
        {
            var metadataModel = await _eesAPI.GetMetadataModel(mappingType);
            if (metadataModel != null)
            {
                var mapping = EESMapping.GetMapping(mappingType);
                if (mapping != null)
                {
                    ModelGeneratorService.GenerateModelFromMetadata(metadataModel, mapping);
                    Console.WriteLine($"Metadata model {mapping.Name} generated");
                }
            }

        }
    }
}
