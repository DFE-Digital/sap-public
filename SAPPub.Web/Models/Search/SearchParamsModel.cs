using System.ComponentModel.DataAnnotations;

namespace SAPPub.Web.Models.Search;

public class SearchParamsModel
{
    private const string PostcodeSearchValidationRegex = """^([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]))))\s?[0-9][A-Za-z]{2})$""";
    public string? NameSearchTerm { get; set; }

    [RegularExpression(PostcodeSearchValidationRegex, ErrorMessage = "Enter a full postcode")]
    public string? LocationSearchTerm { get; set; }
    public int Distance { get; set; } = 3;
    public int? PageNumber { get; set; }
    public string[]? Phase { get; set; }
    public string[]? SchoolType { get; set; }


    public string[] PhasesOfEducation { get; set; } = new[] { "Primary", "Secondary", "16 to 19", "All-through" };
    public string[] TypesOfSchool { get; set; } = new[] { "Academy", "Maintained school", "Independent schools", "Special school", "College" };
}
