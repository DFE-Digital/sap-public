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
}
