using System.Reflection.PortableExecutable;

namespace SAPPub.Playwright.Testing.KS2.Performance.AdditionalMeasures;

public class PageConstants
{
    public static readonly IReadOnlyDictionary<string, string> ContentIds =
    new Dictionary<string, string>()
    {
        ["additional-measures-breakdown-table"] = "additional-measures-breakdown-table",
        ["pupils-eoks2-table"] = "pupils-eoks2-table",
        ["ks2-population-breakdown-table"] = "ks2-population-breakdown-table",
        ["disadvantaged-pupils-population-table"] = "disadvantaged-pupils-population-table",
        ["whole-school-population-table"] = "whole-school-population-table",
        ["sen-population-table"] = "sen-population-table",
        ["ehcp-population-table"] = "ehcp-population-table",
        ["pupil-population-accordion-by-characteristics"] = "pupil-population-accordion-by-characteristics",
        ["pupil-population-accordion"] = "pupil-population-accordion"
    };
}
