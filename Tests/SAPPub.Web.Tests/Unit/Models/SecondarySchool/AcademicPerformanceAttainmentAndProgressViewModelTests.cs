using SAPPub.Core.Enums;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Web.Models.SecondarySchool;

namespace SAPPub.Web.Tests.Unit.Models.SecondarySchool;

public class AcademicPerformanceAttainmentAndProgressViewModelTests
{
    [Fact]
    public void Map_MapsDataAsExpected()
    {
        // Arrange
        var testdata = new Core.ServiceModels.KS4.Performance.AttainmentAndProgressModel()
        {
            Urn = "123456",
            SchoolName = "Test School",
            EstablishmentProgress8Score = 0.5,
            EstablishmentProgress8CILower = -0.2,
            EstablishmentProgress8CIUpper = 1.2,
            EstablishmentProgress8Banding = "Average",
            LocalAuthorityProgress8Score = 0.3,
            EstablishmentAttainment8Score = 45.0,
            LocalAuthorityAttainment8Score = 40.0,
            EnglandAttainment8Score = 50.0,
            EstablishmentProgress8TotalPupils = 100,
            EstablishmentTotalPupils = 150
        };

        // Act
        var viewModel = AcademicPerformanceAttainmentAndProgressViewModel.Map(testdata, Core.Enums.AcademicYearSelection.Current);

        // Assert
        Assert.Equal(testdata.Urn, viewModel.URN);
        Assert.Equal(testdata.SchoolName, viewModel.SchoolName);
        Assert.Equal(testdata.EstablishmentProgress8Score, viewModel.EstablishmentProgress8Score);
        Assert.Equal(testdata.EstablishmentProgress8CILower, viewModel.EstablishmentProgress8CILower);
        Assert.Equal(testdata.EstablishmentProgress8CIUpper, viewModel.EstablishmentProgress8CIUpper);
        Assert.Equal(testdata.EstablishmentProgress8Banding, viewModel.EstablishmentProgress8Banding);
        Assert.Equal(testdata.LocalAuthorityProgress8Score, viewModel.LocalAuthorityProgress8Score);
        Assert.Equal(testdata.EstablishmentAttainment8Score, viewModel.EstablishmentAttainment8Score);
        Assert.Equal(testdata.LocalAuthorityAttainment8Score, viewModel.LocalAuthorityAttainment8Score);
        Assert.Equal(testdata.EnglandAttainment8Score, viewModel.EnglandAttainment8Score);
        Assert.Equal(testdata.EstablishmentProgress8TotalPupils, viewModel.EstablishmentProgress8TotalPupils);
        Assert.Equal(testdata.EstablishmentTotalPupils, viewModel.EstablishmentTotalPupils);
    }

    [Theory]
    [InlineData(-1, "Not available")]
    [InlineData(91, "Not available")]
    [InlineData(0, "below a grade 1")]
    [InlineData(0.1, "below a grade 1")]
    [InlineData(9.9, "below a grade 1")]
    [InlineData(10.0, "grade 1")]
    [InlineData(10.9, "grade 1")]
    [InlineData(11.0, "just above grade 1")]
    [InlineData(12.9, "just above grade 1")]
    [InlineData(13.0, "between grade 1 and grade 2")]
    [InlineData(18.0, "between grade 1 and grade 2")]
    [InlineData(18.1, "just below grade 2")]
    [InlineData(19.9, "just below grade 2")]
    [InlineData(20.0, "grade 2")]
    [InlineData(20.9, "grade 2")]
    [InlineData(21.0, "just above grade 2")]
    [InlineData(22.9, "just above grade 2")]
    [InlineData(23.0, "between grade 2 and grade 3")]
    [InlineData(28.0, "between grade 2 and grade 3")]
    [InlineData(28.1, "just below grade 3")]
    [InlineData(29.9, "just below grade 3")]
    [InlineData(30.0, "grade 3")]
    [InlineData(30.9, "grade 3")]
    [InlineData(31.0, "just above grade 3")]
    [InlineData(32.9, "just above grade 3")]
    [InlineData(33.0, "between grade 3 and grade 4")]
    [InlineData(38.0, "between grade 3 and grade 4")]
    [InlineData(38.1, "just below grade 4")]
    [InlineData(39.9, "just below grade 4")]
    [InlineData(40.0, "grade 4")]
    [InlineData(40.9, "grade 4")]
    [InlineData(41.0, "just above grade 4")]
    [InlineData(42.9, "just above grade 4")]
    [InlineData(43.0, "between grade 4 and grade 5")]
    [InlineData(48.0, "between grade 4 and grade 5")]
    [InlineData(48.1, "just below grade 5")]
    [InlineData(49.9, "just below grade 5")]
    [InlineData(50.0, "grade 5")]
    [InlineData(50.9, "grade 5")]
    [InlineData(51.0, "just above grade 5")]
    [InlineData(52.9, "just above grade 5")]
    [InlineData(53.0, "between grade 5 and grade 6")]
    [InlineData(58.0, "between grade 5 and grade 6")]
    [InlineData(58.1, "just below grade 6")]
    [InlineData(59.9, "just below grade 6")]
    [InlineData(60.0, "grade 6")]
    [InlineData(60.9, "grade 6")]
    [InlineData(61.0, "just above grade 6")]
    [InlineData(62.9, "just above grade 6")]
    [InlineData(63.0, "between grade 6 and grade 7")]
    [InlineData(68.0, "between grade 6 and grade 7")]
    [InlineData(68.1, "just below grade 7")]
    [InlineData(69.9, "just below grade 7")]
    [InlineData(70.0, "grade 7")]
    [InlineData(70.9, "grade 7")]
    [InlineData(71.0, "just above grade 7")]
    [InlineData(72.9, "just above grade 7")]
    [InlineData(73.0, "between grade 7 and grade 8")]
    [InlineData(78.0, "between grade 7 and grade 8")]
    [InlineData(78.1, "just below grade 8")]
    [InlineData(79.9, "just below grade 8")]
    [InlineData(80.0, "grade 8")]
    [InlineData(80.9, "grade 8")]
    [InlineData(81.0, "just above grade 8")]
    [InlineData(82.9, "just above grade 8")]
    [InlineData(83.0, "between grade 8 and grade 9")]
    [InlineData(88.0, "between grade 8 and grade 9")]
    [InlineData(88.1, "just below grade 9")]
    [InlineData(89.9, "just below grade 9")]
    [InlineData(90.0, "grade 9")]
    public void Map_EstablishmentAttainment8ScoreDescription_IsExpected(double establishmentAttainment8Score, string expected)
    {
        // Arrange
        var testdata = new Core.ServiceModels.KS4.Performance.AttainmentAndProgressModel()
        {
            Urn = "123456",
            SchoolName = "Test School",
            EstablishmentProgress8Score = 0.5,
            LocalAuthorityProgress8Score = 0.3,
            EstablishmentAttainment8Score = establishmentAttainment8Score,
            LocalAuthorityAttainment8Score = 40.0,
            EnglandAttainment8Score = 50.0,
            EstablishmentProgress8TotalPupils = 100,
            EstablishmentTotalPupils = 150
        };

        // Act
        var viewModel = AcademicPerformanceAttainmentAndProgressViewModel.Map(testdata, Core.Enums.AcademicYearSelection.Current);

        // Assert
        var expectedContextStatement = expected != "Not available" ?
            $"This means that pupils generally scored the equivalent of {expected} in their 8 best GCSE-level subjects."
            : "Not available";
        Assert.Equal(expectedContextStatement, viewModel.EstablishmentAttainment8ScoreContextDescription.DisplayText());
        if (expected == "Not available")
        {
            Assert.False(viewModel.EstablishmentAttainment8ScoreContextDescription.IsAvailable);
            Assert.True(viewModel.EstablishmentAttainment8ScoreContextDescription.IsNotAvailable);
        }
        else
        {
            Assert.True(viewModel.EstablishmentAttainment8ScoreContextDescription.IsAvailable);
            Assert.False(viewModel.EstablishmentAttainment8ScoreContextDescription.IsNotAvailable);
        }
    }

    [Theory]
    [InlineData(70.0, 50.0, "20.0 points higher than", "above")]
    [InlineData(53.0, 50.0, "3.0 points higher than", "above")]
    [InlineData(52.9, 50.0, "2.9 points higher than", "just above")]
    [InlineData(51.0, 50.0, "1.0 points higher than", "just above")]
    [InlineData(50.9, 50.0, "0.9 points higher than", "similar to")]
    [InlineData(50.0, 50.0, "the same as", "similar to")]
    [InlineData(49.9, 50.0, "0.1 points lower than", "similar to")]
    [InlineData(49.1, 50.0, "0.9 points lower than", "similar to")]
    [InlineData(49.0, 50.0, "1.0 points lower than", "just below")]
    [InlineData(47.1, 50.0, "2.9 points lower than", "just below")]
    [InlineData(47.0, 50.0, "3.0 points lower than", "below")]
    [InlineData(40.0, 50.0, "10.0 points lower than", "below")]
    [InlineData(40.0, null, "Not available", "Not available")]
    [InlineData(null, 34.1, "Not available", "Not available")]
    public void Map_LocalAuthorityAttainment8ScoreDescription_IsExpected(double? establishmentAttainment8Score, double? localAuthorityAttainment8Score, string expected1, string expected2)
    {
        // Arrange
        var testdata = new Core.ServiceModels.KS4.Performance.AttainmentAndProgressModel()
        {
            Urn = "123456",
            SchoolName = "Test School",
            EstablishmentAttainment8Score = establishmentAttainment8Score,
            LocalAuthorityAttainment8Score = localAuthorityAttainment8Score
        };

        // Act
        var viewModel = AcademicPerformanceAttainmentAndProgressViewModel.Map(testdata, Core.Enums.AcademicYearSelection.Current);

        // Assert
        var expectedContextStatement1 = expected1 != "Not available" ?
            $"An Attainment 8 score of {establishmentAttainment8Score:0.0} is {expected1} the local council average of {localAuthorityAttainment8Score:0.0}."
            : "Not available";
        var expectedContextStatement2 = expected2 != "Not available" ?
            $"This means pupils are performing {expected2} pupils at other schools in the area."
            : "Not available";
        Assert.Contains(expectedContextStatement1, viewModel.LocalAuthorityAttainment8ScoreContextDescription.DisplayText());
        Assert.Contains(expectedContextStatement2, viewModel.LocalAuthorityAttainment8ScoreContextDescription.DisplayText());
        if (expected1 == "Not available")
        {
            Assert.False(viewModel.LocalAuthorityAttainment8ScoreContextDescription.IsAvailable);
            Assert.True(viewModel.LocalAuthorityAttainment8ScoreContextDescription.IsNotAvailable);
        }
        else
        {
            Assert.True(viewModel.LocalAuthorityAttainment8ScoreContextDescription.IsAvailable);
            Assert.False(viewModel.LocalAuthorityAttainment8ScoreContextDescription.IsNotAvailable);
        }
    }

    [Theory]
    [InlineData(70.0, 50.0, "20.0 points higher than", "above")]
    [InlineData(53.0, 50.0, "3.0 points higher than", "above")]
    [InlineData(52.9, 50.0, "2.9 points higher than", "just above")]
    [InlineData(51.0, 50.0, "1.0 points higher than", "just above")]
    [InlineData(50.9, 50.0, "0.9 points higher than", "similar to")]
    [InlineData(50.0, 50.0, "the same as", "similar to")]
    [InlineData(49.9, 50.0, "0.1 points lower than", "similar to")]
    [InlineData(49.1, 50.0, "0.9 points lower than", "similar to")]
    [InlineData(49.0, 50.0, "1.0 points lower than", "just below")]
    [InlineData(47.1, 50.0, "2.9 points lower than", "just below")]
    [InlineData(47.0, 50.0, "3.0 points lower than", "below")]
    [InlineData(40.0, 50.0, "10.0 points lower than", "below")]
    [InlineData(40.0, null, "Not available", "Not available")]
    [InlineData(null, 34.1, "Not available", "Not available")]
    public void Map_NationalAttainment8ScoreDescription_IsExpected(double? establishmentAttainment8Score, double? nationalAttainment8Score, string expected1, string expected2)
    {
        // Arrange
        var testdata = new Core.ServiceModels.KS4.Performance.AttainmentAndProgressModel()
        {
            Urn = "123456",
            SchoolName = "Test School",
            EstablishmentAttainment8Score = establishmentAttainment8Score,
            EnglandAttainment8Score = nationalAttainment8Score
        };

        // Act
        var viewModel = AcademicPerformanceAttainmentAndProgressViewModel.Map(testdata, Core.Enums.AcademicYearSelection.Current);

        // Assert
        var expectedContextStatement1 = expected1 != "Not available" ?
            $"It's {expected1} the national average of {nationalAttainment8Score}"
            : "Not available";
        var expectedContextStatement2 = expected2 != "Not available" ?
            $"meaning pupils are performing {expected2} the national average."
            : "Not available";
        Assert.Contains(expectedContextStatement1, viewModel.EnglandAttainment8ScoreContextDescription.DisplayText());
        Assert.Contains(expectedContextStatement2, viewModel.EnglandAttainment8ScoreContextDescription.DisplayText());
        if (expected1 == "Not available")
        {
            Assert.False(viewModel.EnglandAttainment8ScoreContextDescription.IsAvailable);
            Assert.True(viewModel.EnglandAttainment8ScoreContextDescription.IsNotAvailable);
        }
        else
        {
            Assert.True(viewModel.EnglandAttainment8ScoreContextDescription.IsAvailable);
            Assert.False(viewModel.EnglandAttainment8ScoreContextDescription.IsNotAvailable);
        }
    }

    [Theory]
    [InlineData(null, false, true, "Not available")]
    [InlineData("Well above average", true, false, "This is well above average.")]
    [InlineData("Above average", true, false, "This is above average.")]
    [InlineData("Average", true, false, "This is average.")]
    [InlineData("Below average", true, false, "This is below average.")]
    [InlineData("Well below average", true, false, "This is well below average.")]
    [InlineData("Not available", false, true, "Not available")]
    [InlineData("SUPP", false, true, "Not available")]

    public void Map_EstablishmentProgress8BandingContextDescription_IsExpected(string? banding, bool isAvailable, bool isNotAvailable, string expectedText)
    {
        // Arrange
        var testdata = new AttainmentAndProgressModel()
        {
            Urn = "123456",
            SchoolName = "Test School",
            EstablishmentProgress8Banding = banding
        };

        // Act
        var viewModel = AcademicPerformanceAttainmentAndProgressViewModel.Map(testdata, AcademicYearSelection.Current);

        // Assert
        Assert.Equal(isAvailable, viewModel.EstablishmentProgress8BandingContextDescription.IsAvailable);
        Assert.Equal(isNotAvailable, viewModel.EstablishmentProgress8BandingContextDescription.IsNotAvailable);
        Assert.Contains(expectedText, viewModel.EstablishmentProgress8BandingContextDescription.DisplayText());
    }
}
