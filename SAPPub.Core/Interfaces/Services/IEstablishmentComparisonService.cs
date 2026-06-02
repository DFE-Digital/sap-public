namespace SAPPub.Core.Interfaces.Services
{
    public interface IEstablishmentComparisonService
    {
        IReadOnlyCollection<string> GetSavedEstablishments();

        bool IsSaved(string urn);

        void Toggle(string urn);

        void RemoveAll();

        bool IsComparisonLimitReached();

        string GetAddedSchoolListPageUrl();
    }
}
