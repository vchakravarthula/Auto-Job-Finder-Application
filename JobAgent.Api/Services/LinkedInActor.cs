using Microsoft.Playwright;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace JobAgent.Api.Services
{
    public class LinkedInActor : IJobBoardActor
    {
        private readonly IConfiguration _config;
        private IPage _page;
        private IBrowserContext _context;

        public LinkedInActor(IConfiguration config)
        {
            _config = config;
        }

        public async Task ApplyAsync(string jobUrl, string tailoredResumePath, string summary, List<string> bullets)
        {
            await InitializeBrowserAsync();
            await LoginAsync();
            await NavigateToJobAndApply(jobUrl, tailoredResumePath, summary, bullets);
        }

        private async Task InitializeBrowserAsync()
        {
            var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false, // Keep visible for "Human-in-the-Loop" MFA/Captcha
                Args = new[] { "--disable-blink-features=AutomationControlled" }
            });

            _context = await browser.NewContextAsync(new BrowserNewContextOptions
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36"
            });

            _page = await _context.NewPageAsync();
        }

        public async Task LoginAsync()
        {
            // 1. Navigate to Login
            await _page.GotoAsync("https://www.linkedin.com/login", new PageGotoOptions
            {
                WaitUntil = WaitUntilState.NetworkIdle
            });

            // 2. Identify the Email Field (Handling 2026 Dynamic IDs)
            // We use GetByLabel but provide a fallback to the common #username ID
            var emailLocator = _page.GetByLabel("Email or phone");
            if (await emailLocator.CountAsync() == 0)
            {
                emailLocator = _page.Locator("#username");
            }

            // 3. Human-like Input (TypeAsync is safer than FillAsync for bot detection)
            await emailLocator.TypeAsync(_config["LinkedIn:Email"], new() { Delay = 100 });

            var passwordLocator = _page.GetByLabel("Password");
            if (await passwordLocator.CountAsync() == 0)
            {
                passwordLocator = _page.Locator("#password");
            }
            await passwordLocator.TypeAsync(_config["LinkedIn:Password"], new() { Delay = 100 });

            // 4. Submit
            await _page.GetByRole(AriaRole.Button, new() { Name = "Sign in" }).ClickAsync();

            // 5. Wait for Navigation to confirm login success
            // This allows time for manual MFA/Captcha if LinkedIn triggers it
            try
            {
                await _page.WaitForURLAsync("**/feed/**", new PageWaitForURLOptions { Timeout = 60000 });
            }
            catch (Exception)
            {

                // If we aren't at the feed, we might be at a 'Challenge' page
                if (_page.Url.Contains("checkpoint"))
                {
                    // Log this to the console so you see it in VS
                    Console.WriteLine("MFA REQUIRED: Please solve in the browser window.");
                }
            }
        }

        private async Task NavigateToJobAndApply(string jobUrl, string resumePath, string summary, List<string> bullets)
        {
            await _page.GotoAsync(jobUrl);

            // Logic for 'Easy Apply' button clicking and file upload goes here...
            // Architect's Note: Use page.SetInputFilesAsync("input[type='file']", resumePath) 
            // to upload the QuestPDF generated file.
        }
    }
}