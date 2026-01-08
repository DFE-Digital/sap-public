using SAPPub.Core.Entities;

namespace SAPPub.Web.Models.Search
{
    public class SearchViewModel
    {
        public string URN { get; set; } = string.Empty;
        public string EstablishmentName { get; set; } = string.Empty;
        public string EstablishmentNameUrl { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public static SearchViewModel FromEstablishmentCoreEntity(Establishment establishment)
        {
            return new SearchViewModel
            {
                URN = establishment.URN,
                EstablishmentName = establishment.EstablishmentName,
                EstablishmentNameUrl = establishment.EstablishmentNameClean,
                Address = establishment.Address
            };
        }

        public static List<SearchViewModel> FromEstablishmentCoreEntity(IEnumerable<Establishment> establishments)
        {
            var list = new List<SearchViewModel>();
            foreach (var establishment in establishments)
            {
                list.Add(FromEstablishmentCoreEntity(establishment));
            }
            return list;
        }
    }
}
