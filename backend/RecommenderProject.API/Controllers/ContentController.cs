using Microsoft.AspNetCore.Mvc;
using RecommenderProject.API.Services;

namespace RecommenderProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly CSVRecommendations _csvRecommendations;

        public ContentController(CSVRecommendations csvRecommendations)
        {
            _csvRecommendations = csvRecommendations;
        }

        [HttpGet("collaborative/{itemId}")]
        public IActionResult GetCollaborativeRecommendations(string itemId)
        {
            var recommendations = _csvRecommendations.GetCollaborative(itemId);
            if (recommendations == null || recommendations.Count == 0)
                return NotFound("No recommendations found.");

            // Return the top 5 recommendations without including the itemId itself
            return Ok(recommendations);
        }

        [HttpGet("content/{itemId}")]
        public IActionResult GetContentBasedRecommendations(string itemId)
        {
            var recommendations = _csvRecommendations.GetContentBased(itemId);
            if (recommendations == null || recommendations.Count == 0)
                return NotFound("No recommendations found.");

            // Return the top 5 recommendations without including the itemId itself
            return Ok(recommendations);
        }
    }
}
