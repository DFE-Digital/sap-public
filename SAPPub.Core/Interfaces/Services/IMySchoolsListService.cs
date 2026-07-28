namespace SAPPub.Core.Interfaces.Services
{
    public interface IMySchoolsListService
    {
        IReadOnlyCollection<string> GetSavedEstablishments();

        bool IsSaved(string urn);

        /// <summary>
        /// Toggles the saved state of an establishment.
        /// If it is currently saved, it's removed, otherwise it gets added to the cookie
        /// </summary>
        /// <param name="urn">The URN of the establishment to toggle.</param>
        /// <returns>The new saved state - <c>true</c> if the establishment is now saved, otherwise <c>false</c> if it was removed</returns>
        bool Toggle(string urn);

        void RemoveAll();

        void Remove(IEnumerable<string> urns);

        bool IsListLimitReached();
    }
}