namespace JobAgent.Api.Services
{
    using Microsoft.SemanticKernel;
    using System.ComponentModel;

    public class ScoringPlugin
    {
        [KernelFunction]
        [Description("Analyzes the match between the Job Description and the Senior Engineer profile.")]
        public string CalculateMatch(
            [Description("The full text of the job description")] string jd,
            [Description("The full text of the master resume")] string resume)
        {
            // Architect's Note: In a production RAG flow, this method can also 
            // perform local pre-processing like extracting years of experience.
            return $"Analyzing JD against 13-year profile for {resume.Substring(0, 15)}...";
        }
    }
}
