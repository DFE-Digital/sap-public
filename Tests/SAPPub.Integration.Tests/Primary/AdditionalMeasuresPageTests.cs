using SAPPub.Playwright.Testing;
using SAPPub.Playwright.Testing.KS2.Performance.AdditionalMeasures;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAPPub.Integration.Tests.Primary;

public class AdditionalMeasuresPageTests : BasePageTest
{
    private string BasePageUrl(string urn) => $"/school/{urn}";
    private string pageUnderTest => "primary-performance/additional-measures";

    [Theory]
    [InlineData("100019", "87", "52")]
    [InlineData("100241", "67", "23")]
    [InlineData("100353", "98", "72")]
    [InlineData("100448", "82", "25")]
    [InlineData("100500", "97", "45")]
    [InlineData("100674", "89", "24")]
    [InlineData("100684", "89", "43")]
    public async Task ShowGrammarPunctuationAndSpelling_Expected(string urn, string expectedPercent, string higherPercent)
    {
        // Arrange && Act
        var response = await Page.GotoAsync(BasePageUrl(urn));
        Assert.NotNull(response);
        response = await Page.GotoPage(response.Url, pageUnderTest);

        // Assert
        var gpsSchoolData = await Page.GetTableRowValuesAsync(PageConstants.ContentIds["additional-measures-breakdown-table"], 0);
        Assert.Equal($"{expectedPercent}%", gpsSchoolData[0]);
        Assert.Equal($"{higherPercent}%", gpsSchoolData[1]);
    }

    [Theory]
    [InlineData("100019", "46", "18", "28", "27", "40", "2", "17", "10")]
    [InlineData("100241", "57", "30", "27", "28", "53", "5", "12", "26")]
    [InlineData("100353", "57", "24", "33", "21", "57", "4", "12", "9")]
    [InlineData("100448", "28", "12", "16", "6", "25", "4", "36", "20")]
    [InlineData("100500", "29", "14", "15", "16", "27", "10", "17", "13")]
    [InlineData("100674", "55", "27", "28", "10", "52", "5", "27", "18")]
    [InlineData("100684", "87", "33", "54", "25", "85", "5", "16", "18")]
    public async Task ShowPupilPopulationBreakdowns_Expected(
            string urn,
            string telig,
            string belig,
            string gelig,
            string tealgrp2,
            string tmobn,
            string psenele,
            string psenelk,
            string tfsm6cla1a)
    {
        // Arrange && Act
        var response = await Page.GotoAsync(BasePageUrl(urn));
        Assert.NotNull(response);
        response = await Page.GotoPage(response.Url, pageUnderTest);

        // Assert
        var schoolNumberPupilsEndKS2Data = await Page.GetTableRowValuesAsync(PageConstants.ContentIds["pupils-eoks2-table"], 0);
        Assert.Equal(telig, schoolNumberPupilsEndKS2Data[0]);

        await Page.ExpandAccordionByIdAsync(PageConstants.ContentIds["pupil-population-accordion-by-characteristics"]);

        var schoolNumberPupilsBreakdownData = await Page.GetTableRowValuesAsync(PageConstants.ContentIds["ks2-population-breakdown-table"], "Girls");
        Assert.Equal(gelig, schoolNumberPupilsBreakdownData[0]);
        schoolNumberPupilsBreakdownData = await Page.GetTableRowValuesAsync(PageConstants.ContentIds["ks2-population-breakdown-table"], "Boys");
        Assert.Equal(belig, schoolNumberPupilsBreakdownData[0]);
        schoolNumberPupilsBreakdownData = await Page.GetTableRowValuesAsync(PageConstants.ContentIds["ks2-population-breakdown-table"], 2);
        Assert.Equal(tealgrp2, schoolNumberPupilsBreakdownData[0]);
        schoolNumberPupilsBreakdownData = await Page.GetTableRowValuesAsync(PageConstants.ContentIds["ks2-population-breakdown-table"], 3);
        Assert.Equal(tmobn, schoolNumberPupilsBreakdownData[0]);

        var disadvantagedPupilsData = await Page.GetTableRowValuesAsync(PageConstants.ContentIds["disadvantaged-pupils-population-table"], 0);
        Assert.Equal(tfsm6cla1a, disadvantagedPupilsData[0]);

        // not checking this value as it comes from the main establishment info data source (GIAS) and should be tested on the 'about the school page'
        var numPupilsTable = await Page.GetTableRowValuesAsync(PageConstants.ContentIds["whole-school-population-table"], 0);

        await Page.ExpandAccordionByIdAsync(PageConstants.ContentIds["pupil-population-accordion"]);

        var senData = await Page.GetTableRowValuesAsync(PageConstants.ContentIds["sen-population-table"], 0);
        Assert.Equal($"{psenelk}%", senData[0]);

        var ehcpData = await Page.GetTableRowValuesAsync(PageConstants.ContentIds["ehcp-population-table"], 0);
        Assert.Equal($"{psenele}%", ehcpData[0]);
    }
}
