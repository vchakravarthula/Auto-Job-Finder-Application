using JobAgent.Api;
using JobAgent.Api.Services;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Google;

var builder = WebApplication.CreateBuilder(args);

// 1. SERVICES: Register your 13-year Senior Architect profile logic
// Example using a local Ollama or LM Studio endpoint
builder.Services.AddTransient<Kernel>(sp =>
{
    var config = builder.Configuration.GetSection("Gemini");
    string apiKey = config["ApiKey"] ?? throw new InvalidOperationException("Gemini API Key missing.");
    string modelId = config["ModelId"] ?? "gemini-2.0-flash";

    var kernelBuilder = Kernel.CreateBuilder();

    // Switch from Azure OpenAI to Google Gemini
    kernelBuilder.AddGoogleAIGeminiChatCompletion(
        modelId: "gemini-3-flash-preview",
        apiKey: apiKey,
        apiVersion: GoogleAIVersion.V1_Beta
    );

    return kernelBuilder.Build();
});

builder.Services.AddScoped<JobMatchService>();
builder.Services.AddScoped<PdfGeneratorService>();
builder.Services.AddScoped<IJobBoardActor, LinkedInActor>();

builder.Services.AddControllers(); // CRITICAL: Ensures JobController is discovered
builder.Services.AddOpenApi();

var app = builder.Build();

// 2. MIDDLEWARE
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers(); // CRITICAL: Routes requests to your endpoints

app.UseDeveloperExceptionPage();

// 3. START: This keeps the process alive and gives you the URL
app.Run();