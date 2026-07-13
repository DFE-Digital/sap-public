namespace SAPPub.Web.Constants;

public static class Constants
{
    public const string Yes = "Yes";
    public const string No = "No";
    public const string LocalCouncilAverage = "Local council average";
    public const string Average = "average";
    public const string EnglandAverage = "England average";
    public const string NotRecorded = "Not recorded";
    public const string NotAvailable = "Not available";
    public const string ActionCompare = "Compare";
    public const string ActionRemove = "Remove";

    // TempData Key Constants
    public const string CookiesConfirmation = "CookiesConfirmation";
    public const string BannerAddSuccess = "BannerAddSuccess";
    public const string BannerRemoveSuccess = "BannerRemoveSuccess";
    public const string ComparisionLimtReached = "ComparisionLimtReached";
    public const string SelectedEstablishmentUrns = "SelectedEstablishmentUrns";
    public const string SelectedSchoolsForRemoval = "SelectedSchoolsForRemoval";
    public const string RemovedSchoolsCount = "RemovedSchoolsCount";
    public const string BannerModel = "BannerModel";
    public const string SubmitAction = "SubmitAction";

    // HttpContext Key Constants
    public const string Establishments = "Establishments";

    // Chart colours
    public const string EstablishmentChartColour = "#A285D1";
    public const string EnglandAverageChartColour = "#28A197";
    public static readonly string[] ChartColours = { "#A285D1", "#12436D", "#28A197" };

    // School comparison labels/text
    public const string Save = "Save";
    public const string Saved = "Saved";
    public const string MySchoolsSave = "Save to my schools list";
    public const string MySchoolsSaved = "Saved to my schools list";

    public const string MySchoolsListLimitExceeded = "You can only add 100 schools to your list. To add another, you must first remove a school.";
    public const string EditYourSchoolsList = "Edit your schools list";
    public const string CompareYourSchools = "Comparing your schools";
    // Managed Feature Names
    public const string EstablishmentComparisonEnabled = "EstablishmentComparisonEnabled";
    public const string FESearchEnabled = "FESearchEnabled";
}
