using Bogus;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Tests.TestBuilders;

public class EnglandPerformanceBuilder
{
    private readonly Faker _faker = new Faker("en_GB");

    private double? _anyQual_Tot_Current_Pct;
    private double? _tripSci_Tot_Current_Pct;
    private double? _more1FL_Tot_Current_Pct;
    private double? _examEntriesGSCE_Tot_Current_Num;
    private double? _examEntriesKS4_Tot_Current_Num;

    // disadvantaged and non-disadvantaged measures
    private double? _attainment8_Dis_Eng_Current_Num;
    private double? _attainment8_Dis_Eng_Previous_Num;
    private double? _attainment8_Dis_Eng_Previous2_Num;
    private double? _attainment8_NDi_Eng_Current_Num;

    public EnglandPerformanceBuilder WithAdditionalMeasures()
    {
        _anyQual_Tot_Current_Pct = Math.Round(_faker.Random.Double(10, 100), 1);
        _tripSci_Tot_Current_Pct = Math.Round(_faker.Random.Double(10, 100), 1);
        _more1FL_Tot_Current_Pct = Math.Round(_faker.Random.Double(10, 100), 1);
        _examEntriesGSCE_Tot_Current_Num = Math.Round(_faker.Random.Double(50, 300), 0);
        _examEntriesKS4_Tot_Current_Num = Math.Round(_faker.Random.Double(50, 300), 0);
        return this;
    }

    public EnglandPerformanceBuilder WithDisadvantagedMeasures()
    {
        _attainment8_Dis_Eng_Current_Num = Math.Round(_faker.Random.Double(30, 80),1);
        _attainment8_Dis_Eng_Previous_Num = Math.Round(_faker.Random.Double(30, 80),1);
        _attainment8_Dis_Eng_Previous2_Num = Math.Round(_faker.Random.Double(30, 80),1);
        _attainment8_NDi_Eng_Current_Num = Math.Round(_faker.Random.Double(30, 80),1);
        return this;
    }

    public EnglandPerformance Build()
    {
        return new EnglandPerformance()
        {
            Id = "National",
            EngMaths49_Tot_Eng_Current_Pct = Math.Round(_faker.Random.Double(10, 100), 1),
            EngMaths59_Tot_Eng_Current_Pct = Math.Round(_faker.Random.Double(10, 100), 1),
            EngMaths79_Tot_Eng_Current_Pct = Math.Round(_faker.Random.Double(10, 100), 1),
            EngMaths49_Boy_Eng_Current_Pct = Math.Round(_faker.Random.Double(10, 100), 1),
            EngMaths59_Boy_Eng_Current_Pct = Math.Round(_faker.Random.Double(10, 100), 1),
            EngMaths79_Boy_Eng_Current_Pct = Math.Round(_faker.Random.Double(10, 100), 1),
            EngMaths49_Grl_Eng_Current_Pct = Math.Round(_faker.Random.Double(10, 100), 1),
            EngMaths59_Grl_Eng_Current_Pct = Math.Round(_faker.Random.Double(10, 100), 1),
            EngMaths79_Grl_Eng_Current_Pct = Math.Round(_faker.Random.Double(10, 100), 1),
            EngMaths49_Tot_Eng_Previous_Pct = Math.Round(_faker.Random.Double(10, 100), 1),
            EngMaths59_Tot_Eng_Previous_Pct = Math.Round(_faker.Random.Double(10, 100), 1),
            EngMaths79_Tot_Eng_Previous_Pct = Math.Round(_faker.Random.Double(10, 100), 1),
            EngMaths49_Tot_Eng_Previous2_Pct = Math.Round(_faker.Random.Double(10, 100), 1),
            EngMaths59_Tot_Eng_Previous2_Pct = Math.Round(_faker.Random.Double(10, 100), 1),
            EngMaths79_Tot_Eng_Previous2_Pct = Math.Round(_faker.Random.Double(10, 100), 1),

            // Pupils achieving at least 1 qualification
            AnyQual_Tot_Eng_Current_Pct_Coded = new CodedDouble(_anyQual_Tot_Current_Pct, "", _anyQual_Tot_Current_Pct.ToString() ?? ""),
            // Pupils entering for triple science
            TripSci_Tot_Eng_Current_Pct_Coded = new CodedDouble(_tripSci_Tot_Current_Pct, "", _tripSci_Tot_Current_Pct.ToString() ?? ""),
            // Pupils entering for more than one foreign language
            More1FL_Tot_Eng_Current_Pct_Coded = new CodedDouble(_more1FL_Tot_Current_Pct, "", _more1FL_Tot_Current_Pct?.ToString() ?? ""),
            // Exam entries per pupil, GCSEs
            ExamEntriesGSCE_Tot_Eng_Current_Num_Coded = new CodedDouble(_examEntriesGSCE_Tot_Current_Num, "", _examEntriesGSCE_Tot_Current_Num.ToString() ?? ""),
            // Exam entries per pupil, all KS4 qualifications
            ExamEntriesKS4_Tot_Eng_Current_Num_Coded = new CodedDouble(_examEntriesKS4_Tot_Current_Num, "", _examEntriesKS4_Tot_Current_Num.ToString() ?? ""),

            // disadvantaged and non-disadvantaged measures
            Attainment8_Dis_Eng_Current_Num_Coded = CreateCodedDouble(_attainment8_Dis_Eng_Current_Num),
            Attainment8_Dis_Eng_Previous_Num_Coded = CreateCodedDouble(_attainment8_Dis_Eng_Previous_Num),
            Attainment8_Dis_Eng_Previous2_Num_Coded = CreateCodedDouble(_attainment8_Dis_Eng_Previous2_Num),
            Attainment8_NDi_Eng_Current_Num_Coded = CreateCodedDouble(_attainment8_NDi_Eng_Current_Num)
        };
    }

    private static CodedDouble CreateCodedDouble(double? value)
    {
        if (value != null)
            return new CodedDouble(value, "", value?.ToString() ?? "");
        else return new CodedDouble(null, "Not available", "z");
    }
}
