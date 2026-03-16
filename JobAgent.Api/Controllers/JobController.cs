using JobAgent.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobAgent.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobController : ControllerBase
    {
        private readonly JobMatchService _matchService;
        private readonly PdfGeneratorService _pdfService;
        private readonly IJobBoardActor _browserActor;

        // Architect's Note: Update this string with your actual resume text 
        // or load it from a file/database in a real production scenario.
        private readonly string _masterResumeText = "Senior Software Engineer with 13 years experience at Microsoft...";

        // Constructor fixes the 'CS0103: name does not exist' errors
        public JobController(
            JobMatchService matchService,
            PdfGeneratorService pdfService,
            IJobBoardActor browserActor)
        {
            _matchService = matchService;
            _pdfService = pdfService;
            _browserActor = browserActor;
        }

        [HttpPost("process-application")]
        public async Task<IActionResult> ProcessApplication([FromBody] ApplicationJob job)
        {
            // This leverages your 97% telemetry speed improvement expertise
            var analysis = await _matchService.EvaluateJobAsync(job.Description, _masterResumeText);
            return Ok(analysis);
        }

        [HttpPost("execute-apply")]
        public async Task<IActionResult> ExecuteApply([FromBody] ApprovedApplication approved)
        {
            // Architect's Note: Using Path.Combine ensures cross-platform compatibility (Windows vs. Linux/Container)
            var tempPdfPath = Path.Combine(Path.GetTempPath(), $"Resume_{Guid.NewGuid():N}.pdf");

            try
            {
                // 1. GENERATE: Create the QuestPDF document with your 13-year impact metrics
                _pdfService.GenerateTailoredResume(tempPdfPath, approved.Summary, approved.Bullets);

                // 2. ORCHESTRATE: Pass the PDF path AND the raw text components to the Actor
                // This allows the Actor to fill both the file-upload and text-area fields.
                await _browserActor.ApplyAsync(
                    approved.JobUrl,
                    tempPdfPath,
                    approved.Summary,
                    approved.Bullets
                );

                return Ok(new
                {
                    Status = "Success",
                    Message = "Playwright session active. Check the Chromium window to finalize.",
                    PdfLocation = tempPdfPath
                });
            }
            catch (Exception ex)
            {
                // 13-year Senior Tip: Always log the specific failure point for easier IcM debugging
                return StatusCode(500, new { Error = "Execution failed", Details = ex.Message });
            }
        }
    }

    // These Record definitions fix the 'CS0246: type could not be found' errors
    public record ApplicationJob(string Description);
    public record ApprovedApplication(string JobUrl, string Summary, List<string> Bullets, string CoverLetter);
}
