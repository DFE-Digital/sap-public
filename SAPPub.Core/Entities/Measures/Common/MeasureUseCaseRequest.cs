using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Entities.Measures.Common
{
    public sealed class MeasureUseCaseRequest
    {
        public string URN { get; init; } = string.Empty;
        public string LocalAuthorityCode { get; init; } = string.Empty;
    }
}
