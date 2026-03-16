namespace JobAgent.Api
{
    public interface IJobBoardActor
    {
        /// <summary>
        /// Orchestrates the browser-based application process.
        /// </summary>
        /// <param name="jobUrl">The direct URL to the job posting.</param>
        /// <param name="tailoredResumePath">Local path to the QuestPDF generated file.</param>
        /// <param name="summary">The AI-generated tailored summary based on your 13-year profile.</param>
        /// <param name="bullets">The high-impact achievement bullets (e.g., 97% telemetry speed-up).</param>
        Task ApplyAsync(string jobUrl, string tailoredResumePath, string summary, List<string> bullets);
    }
}
