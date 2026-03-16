using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

public class JobMatchService
{
    private readonly Kernel _kernel;

    // The DI container automatically provides the registered Kernel here
    public JobMatchService(Kernel kernel)
    {
        _kernel = kernel;
    }

    public async Task<string> ExecuteMatchAsync(string jd, string resume)
    {
        // Use the injected _kernel to run your semantic functions
        var result = await _kernel.InvokePromptAsync("...", new() { ["jd"] = jd, ["resume"] = resume });
        return result.ToString();
    }

    // Change "ExecuteMatchAsync" to "EvaluateJobAsync"
    public async Task<string> EvaluateJobAsync(string jd, string resume)
    {
        var scoringFunction = _kernel.CreateFunctionFromPrompt(
            @"Score this JD: {{$jd}} against Resume: {{$resume}}. 
          Provide a score (0-100) and identify alignment with Zero Trust or Azure Synapse.",
            new OpenAIPromptExecutionSettings { MaxTokens = 1000, Temperature = 0.2 }
        );

        var result = await _kernel.InvokeAsync(scoringFunction, new()
        {
            ["jd"] = jd,
            ["resume"] = resume
        });

        return result.ToString();
    }
}