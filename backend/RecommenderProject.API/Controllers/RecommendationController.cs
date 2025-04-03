using Microsoft.AspNetCore.Mvc;
using RecommenderProject.API.Data;
using RecommenderProject.API.Services;

namespace RecommenderProject.API.Controllers;


[Route("api/[controller]")]
[ApiController]
public class RecommendationController : ControllerBase
{
    private readonly CSVRecommendations _csvService;
    private readonly AzureML _azureService;

    public RecommendationController(CSVRecommendations csvService, AzureML azureService)
    {
        _csvService = csvService;
        _azureService = azureService;
    }

    [HttpPost]
    public async Task<ActionResult<RecommendationResponse>> Post([FromBody] RecommendationRequest request)
    {
        var collab = _csvService.GetCollaborative(request.contentId);
        var content = _csvService.GetContentBased(request.contentId);
        var azure = await _azureService.GetAzureRecommendationsAsync(request.contentId);

        return Ok(new RecommendationResponse
        {
            Collaborative = collab,
            ContentBased = content,
            AzureML = azure
        });
    }
}
