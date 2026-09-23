using System;
using System.Collections.Generic;
using System.Text;

namespace SAPPub.Integration.Tests.SecondarySchoolTests;

public class EnglishAndMathsPageTests : BasePageTest
{
    [Theory]
    [MemberData(nameof(GetGrade5AndAboveData))]
    public async Task Grade5AndAboveData_Expected()
    {

    }
}
