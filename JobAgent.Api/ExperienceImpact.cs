namespace JobAgent.Api
{
    public class ExperienceImpact
    {
        public string Role { get; set; }
        public string Team { get; set; }
        public List<string> Bullets { get; set; } // The AI will rewrite these
        public string Environment { get; set; } // Key for tech-stack matching [cite: 23]
    }
}
