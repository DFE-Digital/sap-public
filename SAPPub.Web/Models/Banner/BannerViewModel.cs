namespace SAPPub.Web.Models.Banner
{
    public class BannerViewModel
    {
        public string Title { get; set; } = string.Empty;

        public string? HeaderContent { get; set; }

        public string? BodyContent { get; set; }

        public string Type { get; set; } = "info";

        public string? Id { get; set; }

        public string Role { get; set; } = "status";
    }
}