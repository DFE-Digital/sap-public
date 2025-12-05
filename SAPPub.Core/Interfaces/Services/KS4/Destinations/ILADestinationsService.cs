using SAPPub.Core.Entities.KS4.Destinations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Interfaces.Services.KS4.Destinations
{
    public interface ILADestinationsService
    {
        IEnumerable<LADestinations> GetAllLADestinations();
        LADestinations GetLADestinations(string laCode);
    }
}
