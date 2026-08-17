using SAPPub.Core.ServiceModels;

namespace SAPPub.Web.Areas.Profiles.Filters;

public interface IEstablishment
{
    public EstablishmentMinimumServiceModel Establishment { get; set; }
}
