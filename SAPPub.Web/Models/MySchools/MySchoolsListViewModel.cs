namespace SAPPub.Web.Models.MySchools;

public class MySchoolsListViewModel
{
    public IReadOnlyCollection<MySchoolModel> MySchools { get; set; } = [];
    public List<string> SelectedEstablishmentUrns { get; set; } = [];
    public int SelectedCount => SelectedEstablishmentUrns.Count;
}
