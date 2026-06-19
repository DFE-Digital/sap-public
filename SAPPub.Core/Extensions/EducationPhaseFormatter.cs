namespace SAPPub.Core.Extensions;

public static class EducationPhaseFormatter
{
    public static string? Format(bool isKS2, bool isKS4, bool isKS5)
    { 
        var phaseList = new List<string>();

        if (isKS2)
        {
            phaseList.Add("Primary");
        }

        if (isKS4)
        {
            phaseList.Add("Secondary");
        }

        if (isKS5)
        {
            phaseList.Add("16 to 18");
        }

        return phaseList.Count switch
        {
            0 => null,
            1 => phaseList[0],
            2 => $"{phaseList[0]} and {phaseList[1]}",
            _ => $"{phaseList[0]}, {phaseList[1]} and {phaseList[2]}"
        };
    }
}
