using SAPPub.EESCodeGenerator.Helpers;
using SAPPub.Infrastructure.EES.Config;
using SAPPub.Infrastructure.EES.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAPPub.EESCodeGenerator
{
    public class ModelGeneratorService
    {
        public static readonly string _outputPath = "C:\\Working\\sap-public\\SAPPub.EESCodeGenerator\\GeneratedModels\\";
        public static readonly string _namespaceName = "EES.API.POC";

        public static readonly string[] _ignoredFilters = new[]
        {
            "AgeRange",
            "ClosedFlag",
            "ConverterAcademyName",
            "ConverterAcademyOpenDate",
            "ConverterAcademyOpenDate",
            "ConverterAcademyURN",
            "FullAddressOfTheSchool",
            "SchoolTelephoneNumber"

        };

        public static void GenerateModelFromMetadata(
            EESMetadataModel metadata,
            EESMapping mapping)
        {
            var saveVersion = mapping.CurrentMajorVersion?.Replace(".", "_").Replace("*", "x");

            var sb = new StringBuilder();
            sb.AppendLine("namespace " + _namespaceName + "." + mapping.AutoClassRoot + ".v" + saveVersion);
            sb.AppendLine("{");
            sb.AppendLine("    public static class " + mapping.AutoClassName);
            sb.AppendLine("    {");
            sb.AppendLine("        public static class Filters");
            sb.AppendLine("        {");

            foreach (var filter in metadata.Filters ?? [])
            {
                var filterClassName = ModelGeneratorTransforms.ToValidIdentifier(filter.Label ?? filter.Column ?? filter.Id ?? "Filter");
                if (_ignoredFilters.Contains(filterClassName))
                {
                    continue;
                }
                var enclosingFilterTypeName = $"{filterClassName}";
                sb.AppendLine($"            public static class {enclosingFilterTypeName}");
                sb.AppendLine("            {");

                var usedOptionNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (var option in filter.Options ?? [])
                {
                    if (string.IsNullOrWhiteSpace(option.Id) || string.IsNullOrWhiteSpace(option.Label))
                    {
                        continue;
                    }

                    var baseOptionName = ModelGeneratorTransforms.ToValidIdentifier(option.Label);
                    if (string.Equals(baseOptionName, enclosingFilterTypeName, StringComparison.OrdinalIgnoreCase))
                    {
                        baseOptionName += "Value";
                    }

                    var optionName = ModelGeneratorTransforms.GetUniqueName(baseOptionName, usedOptionNames);
                    sb.AppendLine($"                public const string {optionName} = \"{ModelGeneratorTransforms.EscapeString(option.Id)}\";");
                }

                sb.AppendLine("            }");
                sb.AppendLine();
            }

            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public static class Identifiers");
            sb.AppendLine("        {");

            var usedIdentifierNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var indicator in metadata.Indicators ?? [])
            {
                if (string.IsNullOrWhiteSpace(indicator.Id) || string.IsNullOrWhiteSpace(indicator.Label))
                {
                    continue;
                }

                var identifierName = ModelGeneratorTransforms.GetUniqueName(ModelGeneratorTransforms.ToValidIdentifier(indicator.Label), usedIdentifierNames);
                sb.AppendLine($"            public const string {identifierName} = \"{ModelGeneratorTransforms.EscapeString(indicator.Id)}\";");
            }

            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            var pathCreate = Path.Combine(_outputPath, mapping.AutoClassRoot, $"v{saveVersion}");
            Directory.CreateDirectory(pathCreate);
            File.WriteAllText(Path.Combine(pathCreate, mapping.AutoClassName + ".cs"), sb.ToString());
        }
    }
}
