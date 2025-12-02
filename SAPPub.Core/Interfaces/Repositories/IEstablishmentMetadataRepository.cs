using SAPPub.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Interfaces.Repositories
{
    public interface IEstablishmentMetadataRepository
    {
        IEnumerable<EstablishmentMetadata> GetAllEstablishmentMetadata();
        EstablishmentMetadata GetEstablishmentMetadata(string urn);
    }
}
