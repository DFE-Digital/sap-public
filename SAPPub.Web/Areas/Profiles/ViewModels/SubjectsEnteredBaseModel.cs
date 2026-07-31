using SAPPub.Web.Models;

namespace SAPPub.Web.Areas.Profiles.ViewModels;

public class SubjectsEnteredBaseModel : BaseViewModel
{
    protected static string GetNumberOfEntries(string? totalNumberOfEntries)
    {
        if (string.IsNullOrWhiteSpace(totalNumberOfEntries))
        {
            return "N/A"!;
        }

        if (int.TryParse(totalNumberOfEntries, out int numberOfEntries))
        {
            return numberOfEntries.ToString("F0");
        }

        return "N/A";
    }
}
