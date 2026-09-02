using Bogus;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Tests.TestBuilders;

public class LaPerformanceBuilder
{
    private readonly Faker _faker = new Faker("en_GB");
    private string? _id;
    private double? _anyQual_Tot_Current_Pct;
    private double? _tripSci_Tot_Current_Pct;
    private double? _more1FL_Tot_Current_Pct;
    private double? _examEntriesGSCE_Tot_Current_Num;
    private double? _examEntriesKS4_Tot_Current_Num;

    public LaPerformanceBuilder WithId(string id)
    {
        _id = id;
        return this;
    }

    public LaPerformanceBuilder WithAdditionalMeasures()
    {
        _anyQual_Tot_Current_Pct = Math.Round(_faker.Random.Double(10, 100), 1);
        _tripSci_Tot_Current_Pct = Math.Round(_faker.Random.Double(10, 100), 1);
        _more1FL_Tot_Current_Pct = Math.Round(_faker.Random.Double(10, 100), 1);
        _examEntriesGSCE_Tot_Current_Num = Math.Round(_faker.Random.Double(5000, 30000), 0);
        _examEntriesKS4_Tot_Current_Num = Math.Round(_faker.Random.Double(5000, 30000), 0);
        return this;
    }

    public LAPerformance Build()
    {
        return new LAPerformance()
        {
            Id = _id ?? _faker.Random.Int(1000, 9999).ToString(),

            // english and maths
            EngMaths49_Tot_LA_Current_Pct = Math.Round(_faker.Random.Double(5, 90), 1),
            EngMaths59_Tot_LA_Current_Pct = Math.Round(_faker.Random.Double(5, 90), 1),
            EngMaths79_Tot_LA_Current_Pct = Math.Round(_faker.Random.Double(5, 90), 1),
            EngMaths49_Boy_LA_Current_Pct = Math.Round(_faker.Random.Double(5, 90), 1),
            EngMaths59_Boy_LA_Current_Pct = Math.Round(_faker.Random.Double(5, 90), 1),
            EngMaths79_Boy_LA_Current_Pct = Math.Round(_faker.Random.Double(5, 90), 1),
            EngMaths49_Grl_LA_Current_Pct = Math.Round(_faker.Random.Double(5, 90), 1),
            EngMaths59_Grl_LA_Current_Pct = Math.Round(_faker.Random.Double(5, 90), 1),
            EngMaths79_Grl_LA_Current_Pct = Math.Round(_faker.Random.Double(5, 90), 1),
            EngMaths49_Tot_LA_Previous_Pct = Math.Round(_faker.Random.Double(5, 90), 1),
            EngMaths59_Tot_LA_Previous_Pct = Math.Round(_faker.Random.Double(5, 90), 1),
            EngMaths79_Tot_LA_Previous_Pct = Math.Round(_faker.Random.Double(5, 90), 1),
            EngMaths49_Tot_LA_Previous2_Pct = Math.Round(_faker.Random.Double(5, 90), 1),
            EngMaths59_Tot_LA_Previous2_Pct = Math.Round(_faker.Random.Double(5, 90), 1),
            EngMaths79_Tot_LA_Previous2_Pct = Math.Round(_faker.Random.Double(5, 90), 1),

            // Pupils achieving at least 1 qualification
            AnyQual_Tot_LA_Current_Pct_Coded = new CodedDouble(_anyQual_Tot_Current_Pct, "", _anyQual_Tot_Current_Pct.ToString() ?? ""),
            // Pupils entering for triple science
            TripSci_Tot_LA_Current_Pct_Coded = new CodedDouble(_tripSci_Tot_Current_Pct, "", _tripSci_Tot_Current_Pct.ToString() ?? ""),
            // Pupils entering for more than one foreign language
            More1FL_Tot_LA_Current_Pct_Coded = new CodedDouble(_more1FL_Tot_Current_Pct, "", _more1FL_Tot_Current_Pct?.ToString() ?? ""),
            // Exam entries per pupil, GCSEs
            ExamEntriesGSCE_Tot_LA_Current_Num_Coded = new CodedDouble(_examEntriesGSCE_Tot_Current_Num, "", _examEntriesGSCE_Tot_Current_Num.ToString() ?? ""),
            // Exam entries per pupil, all KS4 qualifications
            ExamEntriesKS4_Tot_LA_Current_Num_Coded = new CodedDouble(_examEntriesKS4_Tot_Current_Num, "", _examEntriesKS4_Tot_Current_Num.ToString() ?? "")
        };
    }
}
