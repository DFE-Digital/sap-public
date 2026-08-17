using Bogus;
using SAPPub.Core.Entities;
using SAPPub.Core.Enums;
using SAPPub.Core.Extensions;
using SAPPub.Core.ServiceModels;

namespace SAPPub.Core.Tests.TestBuilders;

public class EstablishmentMinimumTestBuilder
{
    private readonly Establishment _establishment = new();

    public static string GenerateUrn()
    {
        // Generates a random 6-digit URN as a string
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }

    public static string GenerateEstablishmentName()
    {
        // Generates a random establishment name
        var adjectives = new[] { "Green", "Oak", "River", "Hill", "Sunny", "Maple", "Elm", "Cedar" };
        var types = new[] { "Primary", "Secondary", "Academy", "School", "College" };
        var suffixes = new[] { "Academy", "School", "College", "Institute" };

        var random = new Random();
        var adjective = adjectives[random.Next(adjectives.Length)];
        var type = types[random.Next(types.Length)];
        var suffix = suffixes[random.Next(suffixes.Length)];

        return $"{adjective} {type} {suffix}";
    }

    public EstablishmentMinimumTestBuilder WithURN(string urn)
    {
        _establishment.URN = urn;
        return this;
    }

    public EstablishmentMinimumTestBuilder WithEstablishmentName(string name)
    {
        _establishment.EstablishmentName = name;
        return this;
    }

    public EstablishmentMinimumTestBuilder WithLAId(string laId)
    {
        _establishment.LAId = laId;
        return this;
    }

    public EstablishmentMinimumTestBuilder WithLAName(string laName)
    {
        _establishment.LAName = laName;
        return this;
    }

    public EstablishmentMinimumTestBuilder WithIsKeyStage2(bool isKS2)
    {
        _establishment.IsKS2 = isKS2;
        return this;
    }

    public EstablishmentMinimumTestBuilder WithIsKeyStage4(bool isKS4)
    {
        _establishment.IsKS4 = isKS4;
        return this;
    }

    public EstablishmentMinimumTestBuilder WithIsKeyStage5(bool isKS5)
    {
        _establishment.IsKS5 = isKS5;
        return this;
    }

    public Establishment Build()
    {
        // fill basic values automatically if not set
        if (string.IsNullOrEmpty(_establishment.URN))
        {
            _establishment.URN = GenerateUrn();
        }
        if (string.IsNullOrEmpty(_establishment.EstablishmentName))
        {
            _establishment.EstablishmentName = GenerateEstablishmentName();
        }
        return _establishment;
    }

    public EstablishmentMinimumServiceModel BuildServiceModel()
    {
        var est = Build();

        return EstablishmentMinimum.MapToServiceModel(est);
    }
}