using Moq;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Services.KS4.AboutSchool;

namespace SAPPub.Web.Tests.Unit.Services.KS4;

public class AboutSchoolServiceTests
{
    private Mock<IEstablishmentService> _establishmentService = new();
    private Mock<ILaUrlsRepository> _laUrlsRepository = new();
    private Mock<IEstablishmentLinksRepository> _establishmentLinksRepository = new();
    private AboutSchoolService _sut;

    public AboutSchoolServiceTests()
    {
        _sut = new AboutSchoolService(_establishmentService.Object, _laUrlsRepository.Object, _establishmentLinksRepository.Object);
    }


}
