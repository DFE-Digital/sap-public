using Bogus;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Tests.TestBuilders;

public class EstablishmentPerformanceBuilder
{
    private readonly Faker _faker = new Faker("en_GB");

    private string? _id;
    private double? _anyQual_Tot_Current_Pct;
    private double? _tripSci_Tot_Current_Pct;
    private double? _more1FL_Tot_Current_Pct;
    private double? _examEntriesGSCE_Tot_Current_Num;
    private double? _examEntriesKS4_Tot_Current_Num;

    public EstablishmentPerformanceBuilder WithUrn(string id)
    {
        _id = id;
        return this;
    }

    public EstablishmentPerformanceBuilder WithAdditionalMeasures()
    {
        _anyQual_Tot_Current_Pct = Math.Round(_faker.Random.Double(10, 100), 1);
        _tripSci_Tot_Current_Pct = Math.Round(_faker.Random.Double(10, 100), 1);
        _more1FL_Tot_Current_Pct = Math.Round(_faker.Random.Double(10, 100), 1);
        _examEntriesGSCE_Tot_Current_Num = Math.Round(_faker.Random.Double(50, 300), 0);
        _examEntriesKS4_Tot_Current_Num = Math.Round(_faker.Random.Double(50, 300), 0);
        return this;
    }

    public EstablishmentPerformance Build()
    {
        return new EstablishmentPerformance()
        {
            Id = _id ?? _faker.Random.Int(100000, 999999).ToString(),

            // english and maths
            EngMaths49_Tot_Est_Current_Pct = Math.Round(_faker.Random.Double(5, 70), 1),
            EngMaths59_Tot_Est_Current_Pct = Math.Round(_faker.Random.Double(5, 70), 1),
            EngMaths79_Tot_Est_Current_Pct = Math.Round(_faker.Random.Double(5, 70), 1),
            EngMaths49_Boy_Est_Current_Pct = Math.Round(_faker.Random.Double(5, 70), 1),
            EngMaths59_Boy_Est_Current_Pct = Math.Round(_faker.Random.Double(5, 70), 1),
            EngMaths79_Boy_Est_Current_Pct = Math.Round(_faker.Random.Double(5, 70), 1),
            EngMaths49_Grl_Est_Current_Pct = Math.Round(_faker.Random.Double(5, 70), 1),
            EngMaths59_Grl_Est_Current_Pct = Math.Round(_faker.Random.Double(5, 70), 1),
            EngMaths79_Grl_Est_Current_Pct = Math.Round(_faker.Random.Double(5, 70), 1),
            EngMaths49_Tot_Est_Previous_Pct = Math.Round(_faker.Random.Double(5, 70), 1),
            EngMaths59_Tot_Est_Previous_Pct = Math.Round(_faker.Random.Double(5, 70), 1),
            EngMaths79_Tot_Est_Previous_Pct = Math.Round(_faker.Random.Double(5, 70), 1),
            EngMaths49_Tot_Est_Previous2_Pct = Math.Round(_faker.Random.Double(5, 70), 1),
            EngMaths59_Tot_Est_Previous2_Pct = Math.Round(_faker.Random.Double(5, 70), 1),
            EngMaths79_Tot_Est_Previous2_Pct = Math.Round(_faker.Random.Double(5, 70), 1),

            // Pupils achieving at least 1 qualification
            AnyQual_Tot_Est_Current_Pct_Coded = new CodedDouble(_anyQual_Tot_Current_Pct, "", _anyQual_Tot_Current_Pct.ToString() ?? ""),
            // Pupils entering for triple science
            TripSci_Tot_Est_Current_Pct_Coded = new CodedDouble(_tripSci_Tot_Current_Pct, "", _tripSci_Tot_Current_Pct.ToString() ?? ""),
            // Pupils entering for more than one foreign language
            More1FL_Tot_Est_Current_Pct_Coded = new CodedDouble(_more1FL_Tot_Current_Pct, "", _more1FL_Tot_Current_Pct?.ToString() ?? ""),
            // Exam entries per pupil, GCSEs
            ExamEntriesGSCE_Tot_Est_Current_Num_Coded = new CodedDouble(_examEntriesGSCE_Tot_Current_Num, "", _examEntriesGSCE_Tot_Current_Num.ToString() ?? ""),
            // Exam entries per pupil, all KS4 qualifications
            ExamEntriesKS4_Tot_Est_Current_Num_Coded = new CodedDouble(_examEntriesKS4_Tot_Current_Num, "", _examEntriesKS4_Tot_Current_Num.ToString() ?? "")
        };
    }
}
