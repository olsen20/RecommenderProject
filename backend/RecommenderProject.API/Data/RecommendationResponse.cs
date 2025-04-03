namespace RecommenderProject.API.Data;

public class RecommendationResponse
{
    public List<string> Collaborative { get; set; } = new();
    public List<string> ContentBased { get; set; } = new();
    public List<string> AzureML { get; set; } = new();
}