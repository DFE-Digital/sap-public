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

    // disadvantaged and non-disadvantaged measures
    private double? _attainment8_Dis_LA_Current_Num;
    private double? _attainment8_Dis_LA_Previous_Num;
    private double? _attainment8_Dis_LA_Previous2_Num;
    private double? _attainment8_NDi_LA_Current_Num;

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

    public LaPerformanceBuilder WithDisadvantagedMeasures()
    {
        _attainment8_Dis_LA_Current_Num = Math.Round(_faker.Random.Double(30, 80),1);
        _attainment8_Dis_LA_Previous_Num = Math.Round(_faker.Random.Double(30, 80),1);
        _attainment8_Dis_LA_Previous2_Num = Math.Round(_faker.Random.Double(30, 80),1);
        _attainment8_NDi_LA_Current_Num = Math.Round(_faker.Random.Double(30, 80),1);
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
            AnyQual_Tot_LA_Current_Pct_Coded = CreateCodedDouble(_anyQual_Tot_Current_Pct),
            // Pupils entering for triple science
            TripSci_Tot_LA_Current_Pct_Coded = CreateCodedDouble(_tripSci_Tot_Current_Pct),
            // Pupils entering for more than one foreign language
            More1FL_Tot_LA_Current_Pct_Coded = CreateCodedDouble(_more1FL_Tot_Current_Pct),
            // Exam entries per pupil, GCSEs
            ExamEntriesGSCE_Tot_LA_Current_Num_Coded = CreateCodedDouble(_examEntriesGSCE_Tot_Current_Num),
            // Exam entries per pupil, all KS4 qualifications
            ExamEntriesKS4_Tot_LA_Current_Num_Coded = CreateCodedDouble(_examEntriesKS4_Tot_Current_Num),

            // disadvantaged and non-disadvantaged measures
            Attainment8_Dis_LA_Current_Num_Coded = CreateCodedDouble(_attainment8_Dis_LA_Current_Num),
            Attainment8_Dis_LA_Previous_Num_Coded = CreateCodedDouble(_attainment8_Dis_LA_Previous_Num),
            Attainment8_Dis_LA_Previous2_Num_Coded = CreateCodedDouble(_attainment8_Dis_LA_Previous2_Num),
            Attainment8_NDi_LA_Current_Num_Coded = CreateCodedDouble(_attainment8_NDi_LA_Current_Num)
        };
    }

    private static CodedDouble CreateCodedDouble(double? value)
    {
        if (value != null)
            return new CodedDouble(value, "", value?.ToString() ?? "");
        else return new CodedDouble(null, "Not available", "z");
    }
}
