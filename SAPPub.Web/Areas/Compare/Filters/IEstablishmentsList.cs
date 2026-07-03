using SAPPub.Core.ServiceModels;

namespace SAPPub.Web.Areas.Compare.Filters;

public interface IEstablishmentsList
{
    List<EstablishmentServiceModel> Establishments { get; set; }
}
