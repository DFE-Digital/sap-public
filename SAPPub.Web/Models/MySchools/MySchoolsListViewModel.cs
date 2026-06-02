namespace SAPPub.Web.Models.MySchools;

public class MySchoolsListViewModel
{
    public IReadOnlyCollection<MySchoolModel> MySchools { get; set; } = Array.Empty<MySchoolModel>();
    public List<string>? SelectedSchools { get; set; }
}
