using Moq;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Compare.Secondary;

[Collection("WebAppCollection")]
public class AdditionalMeasures : PageTestsBase
{
    private string _pageUrl = "compare/secondary/additional-measures";
    private List<string> _urns = ["100279", "145179"];
    private string QueryString => string.Join("&", _urns.Select(urn => $"urns={urn}"));

    private readonly List<EstablishmentServiceModel> _establishments = [];
    private readonly AdditionalMeasuresModel _additionalMeasuresResult;
    private readonly Mock<IEstablishmentService> _establishmentService = new();
    private readonly Mock<IAdditionalMeasuresService> _additionalMeasuresService = new();

    public AdditionalMeasures(WebAppFixture fixture) : base(fixture)
    {
        _establishmentService = UseMock<IEstablishmentService>();
        _additionalMeasuresService = UseMock<IAdditionalMeasuresService>();

        _establishments =
        [
            new EstablishmentTestBuilder().WithURN(_urns[0]).WithEstablishmentName($"Test school {_urns[0]}").WithIsKeyStage4(true).BuildServiceModel(),
            new EstablishmentTestBuilder().WithURN(_urns[1]).WithEstablishmentName($"Test school {_urns[1]}").WithIsKeyStage4(true).BuildServiceModel(),
        ];

        _establishmentService
            .Setup(s => s.GetEstablishmentsAsync(_urns, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_establishments);


    }

    [Fact]
    public async Task AdditionalMeasuresPage_HasCorrectTitle()
    {

    }
}
