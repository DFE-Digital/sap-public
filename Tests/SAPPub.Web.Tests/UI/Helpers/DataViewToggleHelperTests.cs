using Microsoft.AspNetCore.Http;
using NuGet.Protocol.Plugins;
using SAPPub.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAPPub.Web.Tests.UI.Helpers;

public class DataViewToggleHelperTests
{
    private static HttpRequest BuildRequest(string path, string queryString)
    {
        var context = new DefaultHttpContext();
        context.Request.Path = path;
        context.Request.QueryString = new QueryString(queryString);
        return context.Request;
    }

    [Theory]
    [InlineData("", false)]
    [InlineData("?all-gcse-view=over-time", true)]
    [InlineData("?all-gcse-view=OVER-TIME", true)]
    [InlineData("?all-gcse-view=current", false)]
    public void IsOverTimeView_ReadViewForIdPrefix_FromQuerystring(string queryString, bool isExpected)
    {
        var request = BuildRequest("/page", queryString);
        Assert.Equal(isExpected, DataViewToggleHelper.IsOverTimeView(request, "all-gcse"));
    }

    [Fact]
    public void ViewUrl_OverTime_AddsViewAndKeepsOtherQueryValue()
    {
        // Arrange
        var request = BuildRequest("/compare/secondary/english-and-maths-results", "?urns=1&urns=2");

        // Act
        var url = DataViewToggleHelper.ViewUrl(request, "all-gcse", true);

        // Assert
        Assert.Equal("/compare/secondary/english-and-maths-results?urns=1&urns=2&all-gcse-view=over-time#all-gcse-chart-table-toggle", url);
    }

    [Fact]
    public void ViewUrl_OverTime_RemovesViewAndKeepsOtherQueryValue()
    {
        // Arrange
        var request = BuildRequest("/page", "?SelectedGrade=Grade4AndAbove&all-gcse-view=over-time#all-gcse-chart-table-toggle");

        // Act
        var url = DataViewToggleHelper.ViewUrl(request, "all-gcse", false);

        // Assert
        Assert.Equal("/page?SelectedGrade=Grade4AndAbove#all-gcse-chart-table-toggle", url);
    }
}
