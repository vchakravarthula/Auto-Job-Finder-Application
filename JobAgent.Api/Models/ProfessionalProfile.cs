namespace JobAgent.Api.Models
{
    public class ProfessionalProfile
    {
        public string Name { get; set; } = "Venkataramana Chakravarthula"; // [cite: 1]
        public string Summary { get; set; } // Your 13-year architect summary 
        public List<SkillCategory> SkillSet { get; set; } = new();
        public List<ExperienceImpact> Experience { get; set; } = new();
    }

    // This is the class the compiler was missing
    public class SkillCategory
    {
        public string CategoryName { get; set; } // e.g., "Al & Data Platforms" 
        public List<string> Skills { get; set; } = new(); // e.g., "Azure OpenAI", "RAG" 
    }
}
