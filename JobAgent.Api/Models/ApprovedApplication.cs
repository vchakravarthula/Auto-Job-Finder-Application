namespace JobAgent.Api.Models
{
    public class ApprovedApplication
    {
        public string JobUrl { get; set; }
        public string Summary { get; set; }
        public List<string> Bullets { get; set; }
        public string CoverLetter { get; set; } // Ensure this matches $executeBody
    }
}
