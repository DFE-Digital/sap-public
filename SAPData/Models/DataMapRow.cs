using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPData.Models
{

    public class DataMapRow
    {
        public string Range { get; set; } = "";               // e.g., SCHOOL
        public string Ref { get; set; } = "";               // e.g., 24_KS4_AT8_TOT
        public string PropertyName { get; set; } = "";
        public string PropertyDescription { get; set; } = "";
        public string Source { get; set; } = "";             // e.g., EES
        public string Type { get; set; } = "";           // e.g., KS4_Performance
        public string Subtype { get; set; } = "";            // e.g., Performance
        public string Year { get; set; } = "";               // e.g., 2024-2025
        public string YearDesc { get; set; } = "";
        public string FileName { get; set; } = "";           // e.g., 202425_performance_tables_schools_provisional.csv
        public string Field { get; set; } = "";              // e.g., attainment8_average
        public string DataType { get; set; } = "";           // e.g., int
        public string DataTypeAlt { get; set; } = "";
        public string RecordFilterBy { get; set; } = "";     // e.g., URN


        public string Filter { get; set; } = "";             // e.g., breakdown
        public string FilterValue { get; set; } = "";        // e.g., Total
        public string Filter2 { get; set; } = "";            // optional
        public string Filter2Value { get; set; } = "";       // optional
        public string Filter3 { get; set; } = "";            // optional
        public string Filter3Value { get; set; } = "";       // optional
        public string Filter4 { get; set; } = "";            // optional
        public string Filter4Value { get; set; } = "";       // optional
        public string Filter5 { get; set; } = "";            // optional
        public string Filter5Value { get; set; } = "";       // optional
        public string Filter6 { get; set; } = "";            // optional
        public string Filter6Value { get; set; } = "";       // optional
        public string Filter7 { get; set; } = "";            // optional
        public string Filter7Value { get; set; } = "";       // optional
        public string Filter8 { get; set; } = "";            // optional
        public string Filter8Value { get; set; } = "";       // optional
        public string Filter9 { get; set; } = "";            // optional
        public string Filter9Value { get; set; } = "";       // optional
        public string Filter10 { get; set; } = "";            // optional
        public string Filter10Value { get; set; } = "";       // optional
        public string Filter11 { get; set; } = "";             // optional
        public string Filter11Value { get; set; } = "";        // optional
        public string Filter12 { get; set; } = "";            // optional
        public string Filter12Value { get; set; } = "";       // optional
        public string Filter13 { get; set; } = "";            // optional
        public string Filter13Value { get; set; } = "";       // optional

        public string Filter14 { get; set; } = "";            // optional
        public string Filter14Value { get; set; } = "";       // optional
        public string Filter15 { get; set; } = "";            // optional
        public string Filter15Value { get; set; } = "";       // optional
        public string Filter16 { get; set; } = "";            // optional
        public string Filter16Value { get; set; } = "";       // optional
        public string Filter17 { get; set; } = "";            // optional
        public string Filter17Value { get; set; } = "";       // optional
        public string Filter18 { get; set; } = "";            // optional
        public string Filter18Value { get; set; } = "";       // optional
        public string Filter19 { get; set; } = "";            // optional
        public string Filter19Value { get; set; } = "";       // optional
        public string Filter20 { get; set; } = "";            // optional
        public string Filter20Value { get; set; } = "";       // optional



        public string ShouldBeNormalised { get; set; } = "";   // e.g., No -> false
        public string NormalisedLookup { get; set; } = "";   // optional
        public string CompoundFields { get; set; } = "";
        public string IgnoreMapping { get; set; } = "";

    }


    public class DataMapMapping : ClassMap<DataMapRow>
    {
        public DataMapMapping()
        {
            Map(m => m.Range).Name("Range");
            Map(m => m.Ref).Name("REF");
            Map(m => m.PropertyName).Name("PropertyName");
            Map(m => m.PropertyDescription).Name("PropertyDescription");
            Map(m => m.Source).Name("Source");
            Map(m => m.Type).Name("Type");
            Map(m => m.Subtype).Name("Subtype");
            Map(m => m.Year).Name("Year");
            Map(m => m.YearDesc).Name("YearDesc");
            Map(m => m.FileName).Name("File");
            Map(m => m.Field).Name("Field");
            Map(m => m.DataType).Name("DataType");
            Map(m => m.RecordFilterBy).Name("RecordFilterBy");
            Map(m => m.Filter).Name("Filter");
            Map(m => m.FilterValue).Name("FilterValue");
            Map(m => m.Filter2).Name("Filter2");
            Map(m => m.Filter2Value).Name("Filter2Value");
            Map(m => m.Filter3).Name("Filter3");
            Map(m => m.Filter3Value).Name("Filter3Value");
            Map(m => m.Filter4).Name("Filter4");
            Map(m => m.Filter4Value).Name("Filter4Value");
            Map(m => m.Filter5).Name("Filter5");
            Map(m => m.Filter5Value).Name("Filter5Value");
            Map(m => m.Filter6).Name("Filter6");
            Map(m => m.Filter6Value).Name("Filter6Value");
            Map(m => m.Filter7).Name("Filter7");
            Map(m => m.Filter7Value).Name("Filter7Value");
            Map(m => m.Filter8).Name("Filter8");
            Map(m => m.Filter8Value).Name("Filter8Value");
            Map(m => m.Filter9).Name("Filter9");
            Map(m => m.Filter9Value).Name("Filter9Value");
            Map(m => m.Filter10).Name("Filter10");
            Map(m => m.Filter10Value).Name("Filter10Value");
            Map(m => m.Filter11).Name("Filter11");
            Map(m => m.Filter11Value).Name("Filter11Value");
            Map(m => m.Filter12).Name("Filter12");
            Map(m => m.Filter12Value).Name("Filter12Value");
            Map(m => m.Filter13).Name("Filter13");
            Map(m => m.Filter13Value).Name("Filter13Value");
            Map(m => m.Filter14).Name("Filter14");
            Map(m => m.Filter14Value).Name("Filter14Value");
            Map(m => m.Filter15).Name("Filter15");
            Map(m => m.Filter15Value).Name("Filter15Value");
            Map(m => m.Filter16).Name("Filter16");
            Map(m => m.Filter16Value).Name("Filter16Value");
            Map(m => m.Filter17).Name("Filter17");
            Map(m => m.Filter17Value).Name("Filter17Value");
            Map(m => m.Filter18).Name("Filter18");
            Map(m => m.Filter18Value).Name("Filter18Value");
            Map(m => m.Filter19).Name("Filter19");
            Map(m => m.Filter19Value).Name("Filter19Value");
            Map(m => m.Filter20).Name("Filter20");
            Map(m => m.Filter20Value).Name("Filter20Value");
            Map(m => m.ShouldBeNormalised).Name("ShouldBeNormalised");
            Map(m => m.NormalisedLookup).Name("NormalisedLookup");
            Map(m => m.CompoundFields).Name("CompoundFields");
            Map(m => m.IgnoreMapping).Name("IgnoreMapping");
        }


    }

    public class DataMapLookup
    {
        public string Key { get; set; } = "";
        public string Value { get; set; } = "";
    }
}
