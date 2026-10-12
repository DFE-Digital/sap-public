using System.ComponentModel.DataAnnotations;

namespace SAPPub.Web.Models.Search;

public class SearchParamsModel
{
    private string? _locationSearchTerm;

    private const string PostcodeSearchValidationRegex = """^([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]))))\s?[0-9][A-Za-z]{2})$""";
    public string? NameSearchTerm { get; set; }

    [RegularExpression(PostcodeSearchValidationRegex, ErrorMessage = "Enter a full postcode")]
    public string? LocationSearchTerm
    {
        get { return _locationSearchTerm?.Trim(); }
        set { _locationSearchTerm = value; }
    }
    public int Distance { get; set; } = 3;
    public int? PageNumber { get; set; }
    public string[]? Phase { get; set; }
    public string[]? SchoolType { get; set; }


    public string[] PhasesOfEducation { get; set; } = ["Primary", "Secondary", "16 to 19", "All-through"];

    // Ticket 1101. Currently excludes independent schools (type 10 & 11). This means they are not pulled into the database and the filter is not required for "Independent schools"
    public string[] TypesOfSchool { get; set; } = ["Academy", "Maintained school", "Special school", "College"];
}
