using AngleSharp.Dom;
using System.Net;

namespace SAPPub.Web.Tests.Unit.Page.Helpers;


public class PageResponse
{
    public required IDocument? Document { get; set; }
    public string? RedirectionLocation { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}
