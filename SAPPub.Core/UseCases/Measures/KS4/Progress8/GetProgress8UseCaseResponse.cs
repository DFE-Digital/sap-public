using SAPPub.Core.Common.CleanArchitecture;
using SAPPub.Core.Entities.Measures.KS4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.UseCases.Measures.KS4.Progress8
{
    public sealed class GetProgress8UseCaseResponse
    {
        public GetProgress8UseCaseResponse(BasicResponseStatus basicResponseStatus)
        {
            BasicResponseStatus = BasicResponseStatus;
        }
        public BasicResponseStatus BasicResponseStatus { get; }

        public required SAPPub.Core.Entities.Measures.KS4.Progress8 Progress8 { get; init; }

    }
}
