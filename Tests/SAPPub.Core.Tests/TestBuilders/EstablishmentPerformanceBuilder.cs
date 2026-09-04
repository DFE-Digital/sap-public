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

    // attainment 8 measures
    private double? _attainment8_Tot_Est_Current_Num;
    private double? _attainment8_Tot_Est_Previous_Num;
    private double? _attainment8_Tot_Est_Previous2_Num;

    // progress 8
    // current year progress 8 data is not available
    private double? _prog8_TotPup_Est_Current_Num_Coded = null;
    private double? _prog8_Tot_Est_Current_Num_Coded = null;
    private double? _prog8_CI_Lower_Est_Current_Num_Coded = null;
    private double? _prog8_CI_Upper_Est_Current_Num_Coded = null;
    private string? _prog8_Banding_Est_Current = null;

    private double? _prog8_TotPup_Est_Previous_Num;
    private double? _prog8_Tot_Est_Previous_Num;
    private double? _prog8_CI_Lower_Est_Previous_Num;
    private double? _prog8_CI_Upper_Est_Previous_Num;
    private string? _prog8_Banding_Est_Previous;

    private double? _prog8_TotPup_Est_Previous2_Num;
    private double? _prog8_Tot_Est_Previous2_Num;
    private double? _prog8_CI_Lower_Est_Previous2_Num;
    private double? _prog8_CI_Upper_Est_Previous2_Num;
    private string? _prog8_Banding_Est_Previous2;


    // disadvantaged measures
    private double? _attainment8_Dis_Est_Current_Num;
    private double? _attainment8_Dis_Est_Previous_Num;
    private double? _attainment8_Dis_Est_Previous2_Num;

    public EstablishmentPerformanceBuilder WithUrn(string id)
    {
        _id = id;
        return this;
    }

    public EstablishmentPerformanceBuilder WithAttainment8()
    {
        _attainment8_Tot_Est_Current_Num = Math.Round(_faker.Random.Double(30, 80), 1);
        _attainment8_Tot_Est_Previous_Num = Math.Round(_faker.Random.Double(30, 80), 1);
        _attainment8_Tot_Est_Previous2_Num = Math.Round(_faker.Random.Double(30, 80), 1);
        return this;
    }

    public EstablishmentPerformanceBuilder WithProgress8()
    {
        // no progress 8 data for current year, so set to null
        //_prog8_TotPup_Est_Current_Num_Coded = Math.Round(_faker.Random.Double(-1,1),2);
        //_prog8_Tot_Est_Current_Num_Coded = Math.Round(_faker.Random.Double(-1,1),2);
        //_prog8_CI_Lower_Est_Current_Num_Coded = Math.Round(_faker.Random.Double(-1,1),2);
        //_prog8_CI_Upper_Est_Current_Num_Coded = Math.Round(_faker.Random.Double(-1,1),2);
        //_prog8_Banding_Est_Current = string.Empty;
        _prog8_TotPup_Est_Previous_Num = Math.Round(_faker.Random.Double(-1,1),2);
        _prog8_Tot_Est_Previous_Num = Math.Round(_faker.Random.Double(-1,1),2);
        _prog8_CI_Lower_Est_Previous_Num = Math.Round(_faker.Random.Double(-1,1),2);
        _prog8_CI_Upper_Est_Previous_Num = Math.Round(_faker.Random.Double(-1,1),2);
        _prog8_Banding_Est_Previous = "Average";
        _prog8_TotPup_Est_Previous2_Num = Math.Round(_faker.Random.Double(-1,1),2);
        _prog8_Tot_Est_Previous2_Num = Math.Round(_faker.Random.Double(-1,1),2);
        _prog8_CI_Lower_Est_Previous2_Num = Math.Round(_faker.Random.Double(-1,1),2);
        _prog8_CI_Upper_Est_Previous2_Num = Math.Round(_faker.Random.Double(-1,1),2);
        _prog8_Banding_Est_Previous2 = "Average";
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

    public EstablishmentPerformanceBuilder WithDisadvantagedMeasures()
    {
        _attainment8_Dis_Est_Current_Num = Math.Round(_faker.Random.Double(30, 80),1);
        _attainment8_Dis_Est_Previous_Num = Math.Round(_faker.Random.Double(30, 80),1);
        _attainment8_Dis_Est_Previous2_Num = Math.Round(_faker.Random.Double(30, 80),1);
        return this;
    }

    public EstablishmentPerformance Build()
    {
        return new EstablishmentPerformance()
        {
            Id = _id ?? _faker.Random.Int(100000, 999999).ToString(),
            Pup_Tot_Est_Current_Num_Coded = CreateCodedDouble(_faker.Random.Int(30,200)),
            Pup_Tot_Est_Previous_Num_Coded = CreateCodedDouble(_faker.Random.Int(30, 200)),
            Pup_Tot_Est_Previous2_Num_Coded = CreateCodedDouble(_faker.Random.Int(30, 200)),

            // attainment 8
            Attainment8_Tot_Est_Current_Num = _attainment8_Tot_Est_Current_Num,
            Attainment8_Tot_Est_Current_Num_Coded = CreateCodedDouble(_attainment8_Tot_Est_Current_Num),
            Attainment8_Tot_Est_Previous_Num = _attainment8_Tot_Est_Previous_Num,
            Attainment8_Tot_Est_Previous_Num_Coded = CreateCodedDouble(_attainment8_Tot_Est_Previous_Num),
            Attainment8_Tot_Est_Previous2_Num = _attainment8_Tot_Est_Previous2_Num,
            Attainment8_Tot_Est_Previous2_Num_Coded = CreateCodedDouble(_attainment8_Tot_Est_Previous2_Num),

            // progress 8
            Prog8_TotPup_Est_Current_Num_Coded = CreateCodedDouble(_prog8_TotPup_Est_Current_Num_Coded),
            Prog8_Tot_Est_Current_Num_Coded = CreateCodedDouble(_prog8_Tot_Est_Current_Num_Coded),
            Prog8_Banding_Est_Current = _prog8_Banding_Est_Current,
            Prog8_CI_Lower_Est_Current_Num_Coded = CreateCodedDouble(_prog8_CI_Lower_Est_Current_Num_Coded),
            Prog8_CI_Upper_Est_Current_Num_Coded = CreateCodedDouble(_prog8_CI_Upper_Est_Current_Num_Coded),

            Prog8_TotPup_Est_Previous_Num = _prog8_TotPup_Est_Previous_Num,
            Prog8_Tot_Est_Previous_Num = _prog8_Tot_Est_Previous_Num,
            Prog8_Tot_Est_Previous_Num_Coded = CreateCodedDouble(_prog8_Tot_Est_Previous_Num),
            Prog8_Banding_Est_Previous = _prog8_Banding_Est_Previous,
            Prog8_CI_Lower_Est_Previous_Num = _prog8_CI_Lower_Est_Previous_Num,
            Prog8_CI_Lower_Est_Previous_Num_Coded = CreateCodedDouble(_prog8_CI_Lower_Est_Previous_Num),
            Prog8_CI_Upper_Est_Previous_Num = _prog8_CI_Upper_Est_Previous_Num,
            Prog8_CI_Upper_Est_Previous_Num_Coded = CreateCodedDouble(_prog8_CI_Upper_Est_Previous_Num),

            Prog8_TotPup_Est_Previous2_Num = _prog8_TotPup_Est_Previous2_Num,
            Prog8_Tot_Est_Previous2_Num = _prog8_Tot_Est_Previous2_Num,
            Prog8_Tot_Est_Previous2_Num_Coded = CreateCodedDouble(_prog8_Tot_Est_Previous2_Num),
            Prog8_Banding_Est_Previous2 = _prog8_Banding_Est_Previous2,
            Prog8_CI_Lower_Est_Previous2_Num = _prog8_CI_Lower_Est_Previous2_Num,
            Prog8_CI_Lower_Est_Previous2_Num_Coded = CreateCodedDouble(_prog8_CI_Lower_Est_Previous2_Num),
            Prog8_CI_Upper_Est_Previous2_Num = _prog8_CI_Upper_Est_Previous2_Num,
            Prog8_CI_Upper_Est_Previous2_Num_Coded = CreateCodedDouble(_prog8_CI_Upper_Est_Previous2_Num),

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
            ExamEntriesKS4_Tot_Est_Current_Num_Coded = new CodedDouble(_examEntriesKS4_Tot_Current_Num, "", _examEntriesKS4_Tot_Current_Num.ToString() ?? ""),

            // disadvantaged measures
            Attainment8_Dis_Est_Current_Num_Coded = CreateCodedDouble(_attainment8_Dis_Est_Current_Num),
            Attainment8_Dis_Est_Previous_Num_Coded = CreateCodedDouble(_attainment8_Dis_Est_Previous_Num),
            Attainment8_Dis_Est_Previous2_Num_Coded = CreateCodedDouble(_attainment8_Dis_Est_Previous2_Num)
        };
    }

    private static CodedDouble CreateCodedDouble(double? value)
    {
        if (value != null)
            return new CodedDouble(value, "", value?.ToString() ?? "");
        else return new CodedDouble(null, "Not available", "z");
    }
}
