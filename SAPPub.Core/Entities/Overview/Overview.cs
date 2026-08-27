using SAPPub.Core.Entities.Destinations;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Entities.Performance;
using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Entities.Overview;

[ExcludeFromCodeCoverage]
public class Overview
{
    public Establishment? Establishment { get; init; }

    public EstablishmentPerformance? KS4Performance { get; init; }

    public LAPerformance? KS4LAPerformance { get; init; }

    public EnglandPerformance? KS4EnglandPerformance { get; init; }

    public KS4EstablishmentDestinations? Destinations { get; init; }

    public KS4LADestinations? LADestinations { get; init; }

    public KS4EnglandDestinations? EnglandDestinations { get; init; }

    public KS2EstablishmentPerformance? KS2Performance { get; init; }

    public KS2LAPerformance? KS2LAPerformance { get; init; }

    public KS2EnglandPerformance? KS2EnglandPerformance { get; init; }
}