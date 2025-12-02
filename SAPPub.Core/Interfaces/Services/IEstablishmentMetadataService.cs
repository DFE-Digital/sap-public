using SAPPub.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Interfaces.Services
{
    public interface IEstablishmentMetadataService
    {
        IEnumerable<EstablishmentMetadata> GetAllEstablishmentMetadata();
        EstablishmentMetadata GetEstablishmentMetadata(string urn);
    }
}
