using Microsoft.AspNetCore.Mvc;

namespace SAPPub.Web.Areas.Compare.Filters;

public class SecondaryComparisonQueryValidationAttribute : TypeFilterAttribute
{
    public SecondaryComparisonQueryValidationAttribute() : base(typeof(SecondaryComparisonQueryValidationFilter)) { }
}
