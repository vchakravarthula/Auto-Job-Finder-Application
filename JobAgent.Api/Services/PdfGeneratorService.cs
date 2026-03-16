namespace JobAgent.Api.Services;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

public class PdfGeneratorService
{
    public PdfGeneratorService()
    {
        // Architect's Note: QuestPDF requires a license declaration for version 2022.12+.
        // Set to Community if you are using it for personal/small-scale projects.
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public void GenerateTailoredResume(string filePath, string tailoredSummary, List<string> optimizedBullets)
    {
        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(40);
                page.Header().Column(col =>
                {
                    col.Item().Text("Venkataramana Chakravarthula").FontSize(22).SemiBold().FontColor(Colors.Blue.Medium);
                    col.Item().Text("Sr. Software Engineer | Distributed Systems & Cloud Infrastructure").FontSize(10);
                });

                page.Content().PaddingVertical(10).Column(col =>
                {
                    col.Spacing(12);

                    // 1. ADDED: The Tailored Summary
                    //[cite_start]// This is where we emphasize your 13 years of experience at Microsoft [cite: 3, 8]
                    col.Item().PaddingBottom(5).Text(tailoredSummary).FontSize(11).Italic();

                    col.Item().Text("Professional Experience Highlights").FontSize(14).SemiBold().Underline();

                    // 2. Optimized Bullets
                    //[cite_start]// These should focus on your petabyte-scale migrations and security fabrics [cite: 12, 14]
                    foreach (var bullet in optimizedBullets)
                    {
                        col.Item().Text(t =>
                        {
                            t.Span("• ").SemiBold();
                            t.Span(bullet).FontSize(11);
                        });
                    }
                });

                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Page ");
                    x.CurrentPageNumber();
                });
            });
        }).GeneratePdf(filePath);
    }
}