using Moq;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.Tests.TestBuilders;

namespace SAPPub.Web.Tests.Unit.Areas.Profiles.Controllers;

public class BaseProfilesTests
{
    protected readonly Mock<IEstablishmentService> mockEstablishmentService;
    protected EstablishmentServiceModel fakeEstablishment;

    public BaseProfilesTests()
    {
        fakeEstablishment = new EstablishmentTestBuilder()
            .WithTrustName("Trust")
            .WithWebsite("https://www.gov.uk/")
            .WithTelephoneNum("012154896")
            .WithAddressStreet("Street")
            .WithAddressLocality("Locality")
            .WithAddressTown("Town")
            .WithAddressPostcode("Postcode")
            .WithLAName("Sheffield")
            .WithLAGssCode("123")
            .WithTypeOfEstablishmentName("EstablishmentName")
            .WithHeadteacherTitle("Title")
            .WithHeadteacherFirstName("FirstName")
            .WithHeadteacherLastName("LastName")
            .WithAgeRangeLow("11")
            .WithAgeRangeHigh("18")
            .WithTotalPupils("1117")
            .WithGenderName("GenderName")
            .WithReligiousCharacterName("ReligiousCharacter")
            .WithSixthForm(false)
            .WithResourcedProvisionName("Resourced provision")
            .WithEstablishmentTypeGroupId("1")
            .WithStatusCode(1)
            .WithOpenReasonId(10)
            .WithOpenDate()
            .WithSenTypes("VI - Visual Impairment, HI - Hearing Impairment")
            .WithIsKeyStage2(true)
            .WithIsKeyStage4(true)
            .BuildServiceModel();

        mockEstablishmentService = new();

        mockEstablishmentService
            .Setup(es => es.GetEstablishmentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeEstablishment);
    }

}
