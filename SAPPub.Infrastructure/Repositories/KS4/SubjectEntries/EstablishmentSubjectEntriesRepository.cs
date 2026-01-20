using SAPPub.Core.Entities.KS4.SubjectEntries;
using SAPPub.Core.Interfaces.Repositories;

namespace SAPPub.Infrastructure.Repositories.KS4.SubjectEntries;

public class EstablishmentSubjectEntriesRepository : IEstablishmentSubjectEntriesRepository
{
    private static List<EstablishmentCoreSubjectEntries.SubjectEntry> CoreSubjects =
            new()
            {
                    new () {
                        SubEntCore_Sub_Est_Current_Num = "English language",
                        SubEntCore_Qual_Est_Current_Num = "GCSE",
                        SubEntCore_Entr_Est_Current_Num = 95.0, // CML TODO  which tier converts the numbers to percentages?
                    },
                    new () {
                        SubEntCore_Sub_Est_Current_Num = "English literature",
                        SubEntCore_Qual_Est_Current_Num = "GCSE",
                        SubEntCore_Entr_Est_Current_Num = 90,
                    },
                    new()
                    {
                        SubEntCore_Sub_Est_Current_Num = "Mathematics",
                        SubEntCore_Qual_Est_Current_Num = "GCSE",
                        SubEntCore_Entr_Est_Current_Num = 97,
                    },
                    new()
                    {
                        SubEntCore_Sub_Est_Current_Num = "Science: Double Award",
                        SubEntCore_Qual_Est_Current_Num = "GCSE",
                        SubEntCore_Entr_Est_Current_Num = 55,
                    },
                    new()
                    {
                        SubEntCore_Sub_Est_Current_Num = "Biology",
                        SubEntCore_Qual_Est_Current_Num = "GCSE",
                        SubEntCore_Entr_Est_Current_Num = 76,
                    }
            };

    public EstablishmentCoreSubjectEntries GetCoreSubjectEntriesByUrn(string urn)
    {
        return new() { SubjectEntries = CoreSubjects };
    }

    public EstablishmentAdditionalSubjectEntries GetAdditionalSubjectEntriesByUrn(string urn)
    {
        return new();
    }
}
