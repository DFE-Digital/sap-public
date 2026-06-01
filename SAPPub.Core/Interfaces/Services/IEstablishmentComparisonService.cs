namespace SAPPub.Core.Interfaces.Services
{
    public interface IEstablishmentComparisonService
    {
        IReadOnlyCollection<string> GetSavedEstablishments();

        bool IsSaved(string urn);

        void Save(string urn);

        void Remove(string urn);

        void Toggle(string urn);

        void RemoveAll();

        bool IsComparisonLimitReached();

        string GetComparisonPageUrl();

        Task<bool> IsFeatureEnabled();
    }
}
